using CodingStrategy.Entities;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
    public class ShopUi : MonoBehaviour
    {
        public TMP_Text ShopLevel;
        public TMP_Text Bit;
        public TMP_Text LevelUpCost, RerollCost;
        public TMP_Text Exp;
        public Image ExpImage;
        public TMP_Text Timer;
        public Image TimerImage;

        public Button levelUpButton;
        public Button rerollButton;

        public Transform shopCommandList;
        public Transform myCommandList;

        public GameObject[] iconList;

        public UnityEvent<int, int> OnBuyCommandEvent;
        public UnityEvent<int> OnSellCommandEvent;
        public UnityEvent<int, int> OnChangeCommandEvent;
        public UnityEvent OnShopLevelUpEvent;
        public UnityEvent OnShopRerollEvent;

        // Start is called before the first frame update
        private void Start()
        {
            levelUpButton.onClick.AddListener(() => { OnShopLevelUpEvent.Invoke(); });
            rerollButton.onClick.AddListener(() => { OnShopRerollEvent.Invoke(); });
        }

        public static void DestroyChildren(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void ClearShopCommandList()
        {
            DestroyChildren(shopCommandList);
        }

        public void ClearMyCommandList()
        {
            DestroyChildren(myCommandList);
        }

        public void SetShopCommandList(ICommand[] commandList)
        {
            ClearShopCommandList();
            foreach (ICommand command in commandList)
            {
                GameObject icon = Instantiate(iconList[int.Parse(command.ID)], shopCommandList);
                Image image;
                switch (command.Info.EnhancedLevel)
                {
                    case 2:
                        image = icon.transform.GetChild(0).GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>("Image/Frame");
                        image.color = new Vector4(0, 0, 0, 200) / 255;
                        break;
                    case 3:
                        image = icon.transform.GetChild(0).GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>("Image/Frame2");
                        image.color = new Vector4(144, 36, 33, 200) / 255;
                        break;
                }
            }
        }

        public void SetMyCommandList(ICommand[] commandList)
        {
            ClearMyCommandList();
            foreach (ICommand command in commandList)
            {
                GameObject o = Instantiate(iconList[int.Parse(command.ID)], myCommandList);
                Image image;
                switch (command.Info.EnhancedLevel)
                {
                    case 2:
                        image = o.transform.GetChild(0).GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>("Image/Frame");
                        image.color = new Vector4(0, 0, 0, 200) / 255;
                        break;
                    case 3:
                        image = o.transform.GetChild(0).GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>("Image/Frame2");
                        image.color = new Vector4(144, 36, 33, 200) / 255;
                        break;
                }
            }
        }

        public void SetShopLevel(int level)
        {
            ShopLevel.text = $"{level}레벨 상점";
        }

        public void SetBit(int bit)
        {
            Bit.color = bit < 0 ? Color.red : Color.white;
            Bit.SetText(bit + " B");
        }

        public void SetExp(int currentExp, int fullExp)
        {
            Exp.text = currentExp + "/" + fullExp;
            ExpImage.fillAmount = currentExp / (float) fullExp;
        }

        public void SetTimer(int remainingTime, int fullTime)
        {
            Timer.text = remainingTime.ToString("00");
            TimerImage.fillAmount = remainingTime / (float) fullTime;
        }

        public void SetLevelUpCost(int levelUpCost)
        {
            LevelUpCost.text = levelUpCost + " Bit";
        }

        public void SetRerollCost(int rerollCost)
        {
            RerollCost.text = rerollCost + " Bit";
        }
    }
}
