using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace CodingStrategy.UI.InGame
{
    public class PlayerStatusUI : MonoBehaviour
    {
        public static readonly Color Red = new Vector4(230, 190, 166, 255) / 255;
        public static readonly Color Green = new Vector4(173, 220, 156, 255) / 255;
        public static readonly Color Blue = new Vector4(78, 149, 217, 255) / 255;
        public static readonly Color Yellow = new Vector4(245, 184, 0, 255) / 255;

        public TMP_Text Rank;
        public TMP_Text Name;
        public TMP_Text Money;
        public Image image;
        public GameObject robotLife;
        public GameObject playerLife;
        public Button button, bigButton;
        public GameObject status;

		public void SetColor(Color color)
        {
            image.color = color;
        }

        public void SetRank(int rank)
        {
            switch (rank)
            {
                case 1:
                    Rank.text = "1st";
                    break;
                case 2:
                    Rank.text = "2nd";
                    break;
                case 3:
                    Rank.text = "3rd";
                    break;
                case 4:
                    Rank.text = "4th";
                    break;
            }
        }

        public void SetName(string name)
        {
            Name.text = name;
        }

        public void SetMoney(int money)
        {
            Money.text = money.ToString() + " B";
            if (money < 0)
            {
                Money.color = Color.red;
            }
        }

        public void SetPlayerHP(int hp)
        {
            for (int i = 0; i < hp; i++)
            {
                playerLife.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = hp; i < 3; i++)
            {
                playerLife.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void SetRobotHP(int hp)
        {
            for (int i = 0; i < hp; i++)
            {
                robotLife.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = hp; i < 5; i++)
            {
                robotLife.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            button = transform.GetChild(transform.childCount - 1).GetComponent<Button>();
            button.onClick.AddListener(() => { status.SetActive(true); bigButton.gameObject.SetActive(true); });
            bigButton.onClick.AddListener(() => { status.SetActive(false); bigButton.gameObject.SetActive(false); });
		}

        // Update is called once per frame
        void Update() {}
    }
}
