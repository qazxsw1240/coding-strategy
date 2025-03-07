using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class PlayerStatusUI : MonoBehaviour
    {
        public static Color Red = new Vector4(224, 0, 4, 204) / 255;
        public static readonly Color Green = new Vector4(83, 219, 57, 255) / 255;
        public static readonly Color Blue = new Vector4(78, 149, 217, 255) / 255;
        public static readonly Color Yellow = new Vector4(245, 184, 0, 255) / 255;

        [SerializeField]
        private TMP_Text Rank;

        [SerializeField]
        private TMP_Text Name;

        [SerializeField]
        private TMP_Text Money;

        [SerializeField]
        private Image image;

        [SerializeField]
        private GameObject robotLife;

        [SerializeField]
        private GameObject playerLife;

        [SerializeField]
        private string userID;

        [SerializeField]
        public Button button, bigButton;

        [SerializeField]
        public GameObject status;

        [SerializeField]
        public UnityEvent OnPlayerUIClickEvent;

        private void Start()
        {
            button = transform.GetChild(transform.childCount - 1).GetComponent<Button>();
            button.onClick.AddListener(
                () =>
                {
                    status.SetActive(true);
                    bigButton.gameObject.SetActive(true);
                    OnPlayerUIClickEvent.Invoke();
                });
            bigButton.onClick.AddListener(
                () =>
                {
                    status.SetActive(false);
                    bigButton.gameObject.SetActive(false);
                });
        }

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

        public string GetRank()
        {
            return Rank.text;
        }

        public void SetRank(int rank)
        {
            Rank.text = rank switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                4 => "4th",
                var _ => Rank.text
            };
        }

        public string GetName()
        {
            return Name.text;
        }

        public void SetName(string name)
        {
            Name.text = name;
        }

        public void SetMoney(int money)
        {
            Money.text = money + " B";
            if (money < 0)
            {
                Money.color = Color.red;
            }
            else
            {
                Money.color = Color.white;
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

        public void SetVisible(bool visibility)
        {
            gameObject.SetActive(visibility);
        }
    }
}
