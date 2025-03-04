using System.Collections;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class GameResult : MonoBehaviour
    {
        public GameObject Gameresult;
        public GameObject EndCard;
        public TextMeshProUGUI Rank;
        public TextMeshProUGUI EndingMessage;

        public Button QuitRoomBtn;

        public void PlayerLeave()
        {
            SceneManager.LoadScene("GameLobby");
        }

        //화면 위에서 아래로 떨어지듯이 나오는 모습 연출
        //일단 Rank의 텍스트 지정한 후에
        //코루틴 실행시키시면 됩니다.
        public IEnumerator ResultUIAnimation(int rank)
        {
            yield return new WaitForSeconds(0.2f);

            Gameresult.SetActive(true);
            QuitRoomBtn = GameObject.Find("Quit Room Button").GetComponent<Button>();
            QuitRoomBtn.onClick.AddListener(PlayerLeave);

            switch (rank)
            {
                case 1:
                    Rank.text = "1st";
                    EndingMessage.text = "축하드립니다! 1등입니다!";
                    break;
                case 2:
                    Rank.text = "2nd";
                    EndingMessage.text = "2등 축하드립니다!";
                    break;
                case 3:
                    Rank.text = "3rd";
                    EndingMessage.text = "다음번엔 잘할 수 있을 겁니다.";
                    break;
                case 4:
                    Rank.text = "4th";
                    EndingMessage.text = "다음번엔 잘할 수 있을 겁니다.";
                    break;
                default:
                    Debug.Log("Wrong GameResult rank");
                    break;
            }

            Sequence sequence = DOTween.Sequence();

            Vector3 endPosition = EndCard.transform.position;

            EndCard.transform.position += new Vector3(0, 1000, 0);

            sequence.Insert(0, EndCard.transform.DOMove(endPosition, 3).SetEase(Ease.OutCubic));

            yield return sequence.WaitForCompletion();
        }
    }
}
