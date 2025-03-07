using System;

using CodingStrategy.UI.Shop;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class InGameUI : MonoBehaviour
    {
        public static readonly float Time = 0.2f;

        public GameTurn gameturn;
        public PlayerStatusUI[] playerStatusUI;
        public GameObject[] characterList, playerUIList, cameraPosList;
        public GameObject mainCamera, statusUI;
        public ShopUi shopUi;
        public ScrollRect shopScrollRect;
        public UnityEvent<string> OnBadSectorClickEvent;
        private int direction;

        // Start is called before the first frame update
        private void Start() {}

        // Update is called once per frame
        private void Update()
        {
            shopScrollRect.horizontalNormalizedPosition += direction * UnityEngine.Time.deltaTime / Time;
            if (shopScrollRect.GetComponent<ShopScroll>().Scrolling)
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
            else if (direction == 0
                  && MathF.Abs(shopScrollRect.velocity.x) < 100.0f
                  && 0.01f < shopScrollRect.horizontalNormalizedPosition
                  && shopScrollRect.horizontalNormalizedPosition < 0.99f)
            {
                if (shopScrollRect.horizontalNormalizedPosition < 0.5f)
                {
                    GotoGame();
                }
                else
                {
                    GotoShop();
                }
            }
            ;
        }

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
    }
}
