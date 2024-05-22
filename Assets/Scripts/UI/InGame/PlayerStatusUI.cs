using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CodingStrategy.UI.InGame
{
    public class PlayerStatusUI : MonoBehaviour
    {
        public static readonly Color Red = new Vector4(230, 190, 166, 255) / 255;
        public static readonly Color Green = new Vector4(173, 220, 156, 255) / 255;
        public static readonly Color Blue = new Vector4(78, 149, 217, 255) / 255;
        public static readonly Color Yellow = new Vector4(245, 184, 0, 255) / 255;

        private TMP_Text Rank;
        private TMP_Text Name;
        private TMP_Text Money;
        private Image image;
        private GameObject robotLife;
        private GameObject playerLife;
        private string userID;

        public string GetUserID()
        {
            return userID;
        }

        public void SetUserID(string userID)
        {
            this.userID = userID;
        }

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
            Money.text = money.ToString() + " Bit";
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
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
