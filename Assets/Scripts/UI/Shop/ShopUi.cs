using UnityEngine;
using CodingStrategy.Entities;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;


namespace CodingStrategy.UI.Shop
{
    public class ShopUi : MonoBehaviour
    {
        public TMP_Text ShopLevel;
        public TMP_Text Bit;
        public TMP_Text LevelUpCost, RerollCost;
        public TMP_Text Exp;
        public Image ExpImage;

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

		public void DestroyChildren(Transform transform)
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		public void ClearShopCommandList()
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
                GameObject _object = Instantiate(iconList[int.Parse(command.Id)], shopCommandList);
				Image image;
				switch (command.Info.EnhancedLevel)
				{
					case 2:
						image = _object.transform.GetChild(0).GetComponent<Image>();
						image.sprite = Resources.Load<Sprite>("Image/Frame");
						image.color = new Vector4(0, 0, 0, 200) / 255;
						break;
					case 3:
						image = _object.transform.GetChild(0).GetComponent<Image>();
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
				GameObject _object = Instantiate(iconList[int.Parse(command.Id)], myCommandList);
				Image image;
				switch (command.Info.EnhancedLevel)
				{
					case 2:
						image = _object.transform.GetChild(0).GetComponent<Image>();
						image.sprite = Resources.Load<Sprite>("Image/Frame");
						image.color = new Vector4(0, 0, 0, 200) / 255;
						break;
					case 3:
						image = _object.transform.GetChild(0).GetComponent<Image>();
						image.sprite = Resources.Load<Sprite>("Image/Frame2");
						image.color = new Vector4(144, 36, 33, 200) / 255;
						break;
				}
			}
		}

        public void SetShopLevel(int level)
        {
            ShopLevel.text = level.ToString() + "레벨 상점";
        }

        public void SetBit(int bit)
        {
            Debug.Log("SetBit: " + bit);
            if (bit < 0)
            {
                Bit.color = Color.red;
            }
            else
            {
                Bit.color = Color.white;
            }

            Bit.SetText(bit.ToString() + " B");
        }

        public void SetExp(int currentExp, int fullExp)
        {
            Exp.text = currentExp.ToString() + "/" + fullExp.ToString();
            //SetLevelUpCost(fullExp - currentExp);
            ExpImage.fillAmount = (float)currentExp / (float)fullExp;
		}

        public void SetLevelUpCost(int levelUpCost)
        {
            LevelUpCost.text = levelUpCost.ToString() + " Bit";
        }

        public void SetRerollCost(int rerollCost)
        {
            RerollCost.text = rerollCost.ToString() + " Bit";
        }

        // Start is called before the first frame update
        void Start()
        {
            levelUpButton.onClick.AddListener(() => { OnShopLevelUpEvent.Invoke(); });
            rerollButton.onClick.AddListener(() => { OnShopRerollEvent.Invoke(); });
            //SetShopLevel(5);
            //SetBit(1);
            //SetExp(10, 20);
            //SetRerollCost(7);
        }

        // Update is called once per frame
        void Update() {}
    }
}
