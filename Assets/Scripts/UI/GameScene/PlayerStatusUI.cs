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
        public static Vector2 ButtonSize;

        public TMP_Text Rank;
        public TMP_Text Name;
        public TMP_Text Money;
        public Image image;
        public GameObject robotLife;
        public GameObject playerLife;
        public Button button;
        public GameObject status;

		private UnityAction unityAction;

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
            ButtonSize = status.GetComponent<RectTransform>().sizeDelta;
			unityAction += ToggleActive;
            button.onClick.AddListener(unityAction);
            status.GetComponent<Button>().onClick.AddListener(unityAction);
        }

        
        private void ToggleActive()
        {
			if (status.activeSelf)
            {
                status.SetActive(false);
                button.GetComponent<RectTransform>().sizeDelta = ButtonSize;
            }
            else
            {
                status.SetActive(true);
				button.GetComponent<RectTransform>().sizeDelta = 10 * ButtonSize;
			}
        }

        // Update is called once per frame
        void Update() {}
    }
}
