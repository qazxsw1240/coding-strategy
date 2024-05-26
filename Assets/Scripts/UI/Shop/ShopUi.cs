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
        // PlayerBit
        public TMP_Text Bit;
        public TMP_Text BitEngtxt;

        // ShopLevel
        public TMP_Text Level1;
        public TMP_Text Level2;

        //LevelupBit
        public TMP_Text levelupBit;
        public Button levelUpButton;

        //ReroadBit
        public TMP_Text reroadBit;
        public Button rerollButton;

        public Transform shopCommandList;
        public Transform myCommandList;

        public GameObject[] iconList;

        public UnityEvent<int, int> OnBuyCommandEvent;
        public UnityEvent<int> OnSellCommandEvent;
        public UnityEvent<int, int> OnChangeCommandEvent;
        public UnityEvent OnShopLevelUpEvent;
        public UnityEvent OnShopRerollEvent;

		public void DestoryChildren(Transform transform)
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		public void ClearShopCommandList()
        {
            DestoryChildren(shopCommandList);
        }

		public void ClearMyCommandList()
		{
            DestoryChildren(myCommandList);
		}

		public void SetShopCommandList(ICommand[] commandList)
        {
            ClearShopCommandList();
            foreach (ICommand command in commandList)
            {
                GameObject _object = Instantiate(iconList[int.Parse(command.Id)], shopCommandList);
                Drop drop = _object.GetComponent<Drop>();
                if (drop != null)
                {
                    drop.slotName = "ShopCommand";
                }
            }
        }

		public void SetMyCommandList(ICommand[] commandList)
        {
            ClearMyCommandList();
			foreach (ICommand command in commandList)
			{
				GameObject _object = Instantiate(iconList[int.Parse(command.Id)], myCommandList);
				Drop drop = _object.GetComponent<Drop>();
				if (drop != null)
				{
					drop.slotName = "MyCommand";
				}
			}
		}

        // PlayerBit
        public void setBit(int bit)
        {
            if (bit < 0)
            {
                Bit.color = Color.red;
                BitEngtxt.color = Color.red;
            }
            else
            {
                Bit.color = Color.black;
                BitEngtxt.color = Color.black;
            }

            Bit.SetText(bit.ToString());
        }

        // ShopLevel
        public void setLevel(int level)
        {
            Level1.SetText(level.ToString());
            Level2.SetText(level.ToString());
        }

        //LevelupBit
        public void setLevelupBit(int bit)
        {
            levelupBit.SetText(bit.ToString());
        }

        //ReroadBit
        public void setReroadBit(int bit)
        {
            reroadBit.SetText(bit.ToString());
        }

        // Start is called before the first frame update
        void Start()
        {
            levelUpButton.onClick.AddListener(() => { OnShopLevelUpEvent.Invoke(); });
            rerollButton.onClick.AddListener(() => { OnShopRerollEvent.Invoke(); });
        }

        // Update is called once per frame
        void Update() {}
    }
}
