using CodingStrategy.UI.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CodingStrategy.UI.InGame
{
    public class InGameUI : MonoBehaviour
    {
        public GameTurn gameturn;
        public PlayerStatusUI[] playerStatusUI;
        public GameObject[] characterList, playerUIList, cameraPosList;
        public GameObject mainCamera, statusUI;
        public ShopUi shopUi;

        // set character visiblity, index: 0 ~ n-1(3)
        public void SetCharacterVisible(int index, bool visibility)
        {
            characterList[index].SetActive(visibility);
        }

        public void SetCameraPosition(int index)
        {
            mainCamera.transform.position = cameraPosList[index].transform.position;
            mainCamera.transform.rotation = cameraPosList[index].transform.rotation;
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}
