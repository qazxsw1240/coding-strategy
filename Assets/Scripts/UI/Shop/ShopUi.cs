using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CodingStrategy.Entities;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using System;
using System.Xml.Linq;
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

        //ReroadBit
        public TMP_Text reroadBit;

        public GameObject plusIcon;

        public GameObject shopItemList;
        public GameObject myItemList;

        public Sprite Empty, Plus, Lock, UpArrow, LeftArrow, RightArrow, LeftUpArrow, RightUpArrow, LeftRotation, RightRotation, MineCoin, Botnet, Malware, Worm;


        public void ClearShop()
        {
            foreach (Transform child in shopItemList.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        private Sprite GetCommandSprite(ICommand command)
        {
            return command.Info.Name == "UpArrow" ? UpArrow
                    : command.Info.Name == "LeftArrow" ? LeftArrow
                    : command.Info.Name == "RightArrow" ? RightArrow
                    : command.Info.Name == "LeftUpArrow" ? LeftUpArrow
                    : command.Info.Name == "RightUpArrow" ? RightUpArrow
                    : command.Info.Name == "LeftRotation" ? LeftRotation
                    : command.Info.Name == "RightRotation" ? RightRotation
                    : command.Info.Name == "MineCoin" ? MineCoin
                    : command.Info.Name == "Botnet" ? Botnet
                    : command.Info.Name == "Malware" ? Malware
                    : command.Info.Name == "Worm" ? Worm
                    : Plus;
        }

        public void SetShopCommand(int index, Sprite sprite)
        {
            shopItemList.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = sprite;
        }

        public void SetShopCommandList(ICommand[] commandList)
        {
            for (int i = 0; i < 5 && i < commandList.Length; i++)
            {
                SetShopCommand(i, GetCommandSprite(commandList[i]));
            }
        }

        public void AddMyEmptyCommand(int index)
        {
            Transform _parent = gameObject.transform.Find("SelectedCommandList");
            GameObject _object = Instantiate(plusIcon, _parent);
            _object.transform.SetSiblingIndex(index);
        }

        public void SetMyCommand(int index, Sprite sprite)
        {
            Transform _object = myItemList.transform.GetChild(index);
			_object.GetChild(0).GetComponent<Image>().sprite = sprite;
			_object.name = "SelectedItem";
            _object.AddComponent<Drag>();
		}

        public void SetMyCommandList(ICommand[] commandList)
        {
			for (int i = 0; i < commandList.Length; i++)
			{
				SetMyCommand(i, GetCommandSprite(commandList[i]));
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
        void Start() {
            SetShopCommand(0, MineCoin);
            SetShopCommand(1, Botnet);
            SetShopCommand(2, LeftUpArrow);
            SetShopCommand(3, LeftRotation);
            SetShopCommand(4, Malware);

            AddMyEmptyCommand(0);
            AddMyEmptyCommand(0);
            AddMyEmptyCommand(0);
        }

        // Update is called once per frame
        void Update() {}
    }
}
