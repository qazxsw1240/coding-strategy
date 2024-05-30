using CodingStrategy.UI.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodingStrategy.UI.InGame
{
    public class InGameUI : MonoBehaviour
    {
        public GameTurn gameturn;
        public PlayerStatusUI[] playerStatusUI;
        public GameObject[] characterList, playerUIList, cameraPosList;
        public GameObject mainCamera, statusUI;
        public ShopUi shopUi;
        public ScrollRect shopScrollRect;
        public static readonly float time = 0.2f;
        private int direction = 0;
        public UnityEvent<string> OnBadSectorClickEvent;

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

        public void StopScroll()
        {
            shopScrollRect.StopMovement();
            shopScrollRect.enabled = false;
        }

        public void GotoShop()
        {
            //Debug.Log("GotoShop");
            StopScroll();
            direction = 1;
        }

        public void GotoGame()
        {
            //Debug.Log("GotoGame");
            StopScroll();
            direction = -1;
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update()
        {
            shopScrollRect.horizontalNormalizedPosition += direction * Time.deltaTime / time;
            if (shopScrollRect.GetComponent<ShopScroll>().isScrolling)
            {
                return;
            }
            if (direction == -1 && shopScrollRect.horizontalNormalizedPosition < 0.01f)
            {
                shopScrollRect.StopMovement();
                shopScrollRect.horizontalNormalizedPosition = 0.0f;
                shopScrollRect.enabled = true;
                direction = 0;
            }
            else if (direction == 1 && 0.99f < shopScrollRect.horizontalNormalizedPosition)
            {
                shopScrollRect.StopMovement();
                shopScrollRect.horizontalNormalizedPosition = 1.0f;
                shopScrollRect.enabled = true;
                direction = 0;
            }
            else if (direction == 0 && MathF.Abs(shopScrollRect.velocity.x) < 100.0f && 0.01f < shopScrollRect.horizontalNormalizedPosition && shopScrollRect.horizontalNormalizedPosition < 0.99f)
            {
                if (shopScrollRect.horizontalNormalizedPosition < 0.5f)
                {
                    GotoGame();
                }
                else
                {
                    GotoShop();
                }
            };
        }
    }
}
