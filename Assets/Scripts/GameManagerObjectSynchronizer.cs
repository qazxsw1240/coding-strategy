#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

using CodingStrategy.Animation;
using CodingStrategy.Entities;
using CodingStrategy.Entities.Animations;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Placeable;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Sound;
using CodingStrategy.UI.GameScene;

using Photon.Pun;
using Photon.Realtime;

using Unity.VisualScripting;

using UnityEngine;

namespace CodingStrategy
{
    public class GameManagerObjectSynchronizer : MonoBehaviour
    {
        private readonly IDictionary<IBadSectorDelegate, GameObject> _badSectorDelegateObjects =
            new Dictionary<IBadSectorDelegate, GameObject>();

        private readonly IDictionary<Coordinate, GameObject> _cellObjects = new Dictionary<Coordinate, GameObject>();

        private readonly IDictionary<IPlaceable, GameObject> _placeableObjects =
            new Dictionary<IPlaceable, GameObject>();

        private readonly IDictionary<IRobotDelegate, GameObject> _robotDelegateObjects =
            new Dictionary<IRobotDelegate, GameObject>();

        public GameManager GameManager { get; set; } = null!;

        public void Start()
        {
            GameManager.BoardDelegate.OnRobotAdd.AddListener(AddRobotObject);
            GameManager.BoardDelegate.OnRobotRemove.AddListener(RemoveRobotObject);
            GameManager.BoardDelegate.OnRobotChangePosition.AddListener(MoveRobotObject);
            GameManager.BoardDelegate.OnRobotChangeDirection.AddListener(RotateRobotObject);

            GameManager.BoardDelegate.OnBadSectorAdd.AddListener(AddBadSectorObject);
            GameManager.BoardDelegate.OnBadSectorRemove.AddListener(RemoveBadSectorObject);

            GameManager.BoardDelegate.OnPlaceableAdd.AddListener(AddPlaceableObject);
            GameManager.BoardDelegate.OnPlaceableRemove.AddListener(RemovePlaceableObject);
        }

        public void OnDestroy()
        {
            _robotDelegateObjects.Clear();
            _badSectorDelegateObjects.Clear();
            _placeableObjects.Clear();
        }

        private static Vector3 ConvertToVector(Coordinate coordinate, float heightOffset)
        {
            return new Vector3(coordinate.X, heightOffset, coordinate.Y);
        }

        public void InitializeCells()
        {
            for (int i = 0; i < GameManager.BoardDelegate.Width; i++)
            {
                for (int j = 0; j < GameManager.BoardDelegate.Height; j++)
                {
                    Coordinate coordinate = new Coordinate(i, j);
                    Vector3 position = ConvertToVector(coordinate, 0);
                    _cellObjects[coordinate] = Instantiate(
                        GameManager.boardCellPrefab,
                        position,
                        Quaternion.identity,
                        transform);
                }
            }
        }

        public void AddRobotObject(IRobotDelegate robotDelegate)
        {
            Coordinate position = robotDelegate.Position;
            RobotDirection direction = robotDelegate.Direction;
            Vector3 vectorPosition = ConvertToVector(position, 0.5f);
            Quaternion quaternion = Quaternion.Euler(0, (int) direction * 90f, 0);
            GameObject prefab = FindRobotPrefab(robotDelegate);
            GameObject robotObject = Instantiate(prefab, vectorPosition, quaternion, transform);
            robotObject.name = robotDelegate.ID;
            robotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            LilbotAnimation lilbotAnimation = robotObject.AddComponent<LilbotAnimation>();
            lilbotAnimation.animator = robotObject.GetComponent<Animator>();
            if (robotDelegate.ID == PhotonNetwork.LocalPlayer.UserId)
            {
                lilbotAnimation.playerCamera = Camera.main;
            }

            _robotDelegateObjects[robotDelegate] = robotObject;
            AddRobotAttackListener(robotDelegate, robotObject);
            StartCoroutine(lilbotAnimation.SpawnAnimationCoroutine());
        }

        public void AddRobotAttackListener(IRobotDelegate robotDelegate, GameObject robotObject)
        {
            robotDelegate.OnRobotAttack.AddListener(
                (_, positions) =>
                {
                    LilbotAnimation lilbotAnimation = robotObject.GetOrAddComponent<LilbotAnimation>();
                    GameManager.AnimationCoroutineManager.AddAnimation(
                        robotObject,
                        lilbotAnimation.AttackLeftAnimationCoroutine());
                    foreach (Coordinate position in positions)
                    {
                        GameObject cellObject = _cellObjects[position];

                        if (GameManager.AnimationCoroutineManager.HasAnimationQueue(cellObject))
                        {
                            return;
                        }

                        TileAttackAnimation tileAttackAnimation = cellObject.GetComponent<TileAttackAnimation>();
                        GameManager.AnimationCoroutineManager.AddAnimation(
                            cellObject,
                            tileAttackAnimation.AttackArea(PlayerStatusUI.Red));
                    }
                });
            robotDelegate.OnHealthPointChange.AddListener(
                (_, previous, next) =>
                {
                    LilbotAnimation lilbotAnimation = robotObject.GetOrAddComponent<LilbotAnimation>();
                    int different = next - previous;
                    if (different < 0)
                    {
                        GameManager.AnimationCoroutineManager.AddAnimation(
                            robotDelegate,
                            lilbotAnimation.HitAnimationCoroutine());
                    }
                    else
                    {
                        if (next > 0)
                        {
                            GameManager.AnimationCoroutineManager.AddAnimation(
                                robotDelegate,
                                lilbotAnimation.SpawnAnimationCoroutine());
                        }
                    }

                    if (next <= 0)
                    {
                        GameManager.AnimationCoroutineManager.AddAnimation(
                            robotDelegate,
                            lilbotAnimation.DeathAnimationCoroutine());
                    }
                });
        }

        public void MoveRobotObject(IRobotDelegate robotDelegate, Coordinate previous, Coordinate next)
        {
            GameObject? robotObject = FindRobotDelegateObject(robotDelegate);

            if (robotObject == null)
            {
                return;
            }

            LilbotAnimation animation = robotObject.GetComponent<LilbotAnimation>();
            Vector3 end = ConvertToVector(next, 0.5f);
            IEnumerator controllerAnimation = animation.Walk(0.5f, (int) end.x, (int) end.z);
            GameManager.AnimationCoroutineManager.AddAnimation(robotDelegate, controllerAnimation);
        }

        public void RotateRobotObject(IRobotDelegate robotDelegate, RobotDirection previous, RobotDirection next)
        {
            GameObject? robotObject = FindRobotDelegateObject(robotDelegate);

            if (robotObject == null)
            {
                return;
            }

            Quaternion start = Quaternion.Euler(0, (int) previous * 90f, 0);
            Quaternion end = Quaternion.Euler(0, (int) next * 90f, 0);
            RotateAnimation rotateAnimation = new RotateAnimation(robotObject, start, end, 0.125f);
            GameManager.AnimationCoroutineManager.AddAnimation(robotDelegate, rotateAnimation);
        }

        public void RemoveRobotObject(IRobotDelegate robotDelegate)
        {
            GameObject? robotGameObject = FindRobotDelegateObject(robotDelegate);
            RemoveRobotDelegateObject(robotDelegate);
            Destroy(robotGameObject);
        }

        private GameObject? FindRobotDelegateObject(IRobotDelegate robotDelegate)
        {
            return !_robotDelegateObjects.TryGetValue(robotDelegate, out GameObject? o) ? null : o;
        }

        private void RemoveRobotDelegateObject(IRobotDelegate robotDelegate)
        {
            _robotDelegateObjects.Remove(robotDelegate);
        }

        public void AddBadSectorObject(IBadSectorDelegate badSectorDelegate)
        {
            IRobotDelegate robotDelegate = badSectorDelegate.Installer;
            int index = GameManager.PlayerIndexMap[robotDelegate.ID];
            (RobotDirection _, Coordinate _, Color color) = GameManager.StartPositions[index];
            color = new Color(color.r, color.g, color.b, color.a * 0.75f);
            Coordinate position = badSectorDelegate.Position;
            Vector3 vectorPosition = ConvertToVector(position, 0.1f);
            GameObject badSectorObject =
                Instantiate(GameManager.badSectorPrefab, vectorPosition, Quaternion.identity, transform);
            BadSectorAnimation badSectorAnimation = badSectorObject.GetComponent<BadSectorAnimation>();
            EnableBadSectorPrefab(badSectorObject, badSectorDelegate, badSectorAnimation);
            badSectorAnimation.Camera = Camera.main;
            badSectorObject.name = badSectorDelegate.ID;
            badSectorAnimation.ChangeBadSectorColor(color);
            _badSectorDelegateObjects[badSectorDelegate] = badSectorObject;
            GameManager.AnimationCoroutineManager.AddAnimation(badSectorDelegate, badSectorAnimation.AnimateItem());
        }

        private void EnableBadSectorPrefab(
            GameObject badSectorObject,
            IBadSectorDelegate badSectorDelegate,
            BadSectorAnimation badSectorAnimation)
        {
            if (badSectorDelegate is MalwareBadSector)
            {
                Transform image = badSectorObject.transform.GetChild(4);
                image.gameObject.SetActive(true);
            }
        }

        public void RemoveBadSectorObject(IBadSectorDelegate badSectorDelegate)
        {
            GameObject badSectorObject = FindBadSectorDelegateObject(badSectorDelegate)!;
            RemoveBadSectorDelegateObject(badSectorDelegate);
            BadSectorAnimation badSectorAnimation = badSectorObject.GetComponent<BadSectorAnimation>();
            GameManager.AnimationCoroutineManager.AddAnimation(
                badSectorDelegate,
                badSectorAnimation.ActivateBadSector());
        }

        private GameObject? FindBadSectorDelegateObject(IBadSectorDelegate badSectorDelegate)
        {
            return !_badSectorDelegateObjects.TryGetValue(badSectorDelegate, out GameObject? o) ? null : o;
        }

        private void RemoveBadSectorDelegateObject(IBadSectorDelegate badSectorDelegate)
        {
            _badSectorDelegateObjects.Remove(badSectorDelegate);
        }

        public void AddPlaceableObject(IPlaceable placeable)
        {
            if (placeable is not IBitDelegate bitDelegate)
            {
                return;
            }

            Coordinate coordinate = bitDelegate.Position;
            Vector3 position = ConvertToVector(coordinate, 1.5f);
            GameObject bitGameObject =
                Instantiate(GameManager.bitPrefab, position, Quaternion.Euler(90f, 0, 0), transform);
            bitDelegate.OnRobotTakeInEvents.AddListener(
                _ =>
                {
                    bitGameObject.SetActive(false);
                    InGameSoundManager soundManager = FindAnyObjectByType<InGameSoundManager>();
                    StartCoroutine(soundManager.GetCoinSound(0f));
                });
            bitDelegate.OnRobotTakeAwayEvents.AddListener(_ => bitGameObject.SetActive(true));
            _placeableObjects[placeable] = bitGameObject;
        }

        public void RemovePlaceableObject(IPlaceable placeable)
        {
            if (placeable is not IBitDelegate bitDelegate)
            {
                return;
            }

            GameObject? bitGameObject = FindPlaceableObject(bitDelegate);

            if (bitGameObject == null)
            {
                return;
            }

            BitAnimation bitAnimation = bitGameObject.GetComponent<BitAnimation>();
            if (bitGameObject.activeSelf)
            {
                bitAnimation.GetBit();
            }
            else
            {
                Destroy(bitGameObject);
            }

            _placeableObjects.Remove(placeable);
        }

        private GameObject? FindPlaceableObject(IPlaceable placeable)
        {
            return !_placeableObjects.TryGetValue(placeable, out GameObject? o) ? null : o;
        }

        private GameObject FindRobotPrefab(IRobotDelegate robotDelegate)
        {
            foreach (Player photonPlayer in PhotonNetwork.PlayerList)
            {
                if (photonPlayer.UserId == robotDelegate.ID)
                {
                    return GameManager.robotPrefabs[GameManager.PlayerIndexMap[photonPlayer.UserId]];
                }
            }

            throw new Exception("Cannot find compatible prefab.");
        }
    }
}
