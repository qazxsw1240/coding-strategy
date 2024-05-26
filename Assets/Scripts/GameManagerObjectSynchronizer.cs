#nullable enable


using System;
using System.Collections;
using System.Collections.Generic;
using CodingStrategy.Entities;
using CodingStrategy.Entities.Animations;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Placeable;
using CodingStrategy.Entities.Robot;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace CodingStrategy
{
    public class GameManagerObjectSynchronizer : MonoBehaviour
    {
        public GameManager GameManager { get; set; } = null!;

        private readonly IDictionary<IRobotDelegate, GameObject> _robotDelegateObjects =
            new Dictionary<IRobotDelegate, GameObject>();

        private readonly IDictionary<IBadSectorDelegate, GameObject> _badSectorDelegateObjects =
            new Dictionary<IBadSectorDelegate, GameObject>();

        private readonly IDictionary<IPlaceable, GameObject> _placeableObjects =
            new Dictionary<IPlaceable, GameObject>();

        private static Vector3 ConvertToVector(Coordinate coordinate, float heightOffset)
        {
            return new Vector3(coordinate.X, heightOffset, coordinate.Y);
        }

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

        public void AddRobotObject(IRobotDelegate robotDelegate)
        {
            Coordinate position = robotDelegate.Position;
            RobotDirection direction = robotDelegate.Direction;
            Vector3 vectorPosition = ConvertToVector(position, 0.5f);
            Quaternion quaternion = Quaternion.Euler(0, (int) direction * 90f, 0);
            GameObject prefab = FindRobotPrefab(robotDelegate);
            GameObject robotObject = Instantiate(prefab, vectorPosition, quaternion, transform);
            robotObject.name = robotDelegate.Id;
            robotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            LilbotAnimation lilbotAnimation = robotObject.AddComponent<LilbotAnimation>();
            lilbotAnimation.animator = robotObject.GetComponent<Animator>();
            _robotDelegateObjects[robotDelegate] = robotObject;
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
            GameManager.AnimationCoroutineManager.AddAnimation(robotObject, controllerAnimation);
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
            GameManager.AnimationCoroutineManager.AddAnimation(robotObject, rotateAnimation);
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
            Coordinate position = badSectorDelegate.Position;
            Vector3 vectorPosition = ConvertToVector(position, 0.5f);
            GameObject robotObject =
                Instantiate(GameManager.badSectorPrefab, vectorPosition, Quaternion.identity, transform);
            robotObject.name = badSectorDelegate.Id;
            robotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            _badSectorDelegateObjects[badSectorDelegate] = robotObject;
        }

        public void RemoveBadSectorObject(IBadSectorDelegate badSectorDelegate)
        {
            GameObject? robotGameObject = FindBadSectorDelegateObject(badSectorDelegate);
            RemoveBadSectorDelegateObject(badSectorDelegate);
            Destroy(robotGameObject);
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
            bitDelegate.OnRobotTakeInEvents.AddListener(_ => bitGameObject.SetActive(false));
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
            bitAnimation.GetBit();

            _placeableObjects.Remove(placeable);
        }

        private GameObject? FindPlaceableObject(IPlaceable placeable)
        {
            return !_placeableObjects.TryGetValue(placeable, out GameObject? o) ? null : o;
        }

        private GameObject FindRobotPrefab(IRobotDelegate robotDelegate)
        {
            foreach ((int key, Player photonPlayer) in PhotonNetwork.CurrentRoom.Players)
            {
                if (photonPlayer.UserId == robotDelegate.Id)
                {
                    return GameManager.robotPrefabs[key];
                }
            }

            throw new Exception("Cannot find compatible prefab.");
        }
    }
}
