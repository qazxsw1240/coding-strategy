#nullable enable


using System;
using System.Collections;
using System.Collections.Generic;
using CodingStrategy.Entities;
using CodingStrategy.Entities.Animations;
using CodingStrategy.Entities.BadSector;
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
            LilbotController lilbotController = robotObject.AddComponent<LilbotController>();
            lilbotController.animator = robotObject.GetComponent<Animator>();
            _robotDelegateObjects[robotDelegate] = robotObject;
        }

        public void MoveRobotObject(IRobotDelegate robotDelegate, Coordinate previous, Coordinate next)
        {
            GameObject? robotObject = FindRobotDelegateObject(robotDelegate);

            if (robotObject == null)
            {
                return;
            }

            LilbotController controller = robotObject.GetComponent<LilbotController>();
            Vector3 end = ConvertToVector(next, 0.5f);
            IEnumerator controllerAnimation = controller.Walk(0.5f, (int) end.x, (int) end.z);
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
            GameObject robotObject = Instantiate(GameManager.badSectorPrefab, vectorPosition, Quaternion.identity, transform);
            robotObject.name = badSectorDelegate.Id;
            robotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            LilbotController lilbotController = robotObject.AddComponent<LilbotController>();
            lilbotController.animator = robotObject.GetComponent<Animator>();
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
