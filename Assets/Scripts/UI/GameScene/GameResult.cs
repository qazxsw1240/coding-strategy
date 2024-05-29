using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace CodingStrategy.UI.InGame
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
        public IEnumerator ResultUIAnimation()
        {
            Gameresult.SetActive(true);

            QuitRoomBtn = GameObject.Find("Quit Room Button").GetComponent<Button>();
            QuitRoomBtn.onClick.AddListener(PlayerLeave);

            yield return new WaitForSeconds(0.2f);


            if (Rank.text == "1st")
            {
                EndingMessage.text = "축하드립니다! 1등입니다!";
            }

            else if (Rank.text == "2nd")
            {
                EndingMessage.text = "2등 축하드립니다!";
            }
            else
            {
                EndingMessage.text = "다음번엔 잘할 수 있을겁니다.";
            }

            Sequence sequence = DOTween.Sequence();

            Vector3 endPosition = EndCard.transform.position;

            EndCard.transform.position = new Vector3(EndCard.transform.position.x, EndCard.transform.position.y + 1000, EndCard.transform.position.z);
            
            sequence.Insert(0, EndCard.transform.DOMove(endPosition, 3).SetEase(Ease.OutCubic));


            yield return sequence.WaitForCompletion();
        }
    }
}
