using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public TextMeshProUGUI connectionStatusText;
    public InputField nicknameInputField;

    public TMP_Text warningText; // 경고 메시지를 표시할 TextMeshPro 컴포넌트
    public TMP_Text loadingText; // 로딩 메시지를 표시할 TextMeshPro 컴포넌트

    void Start()
    {
        // startbutton이 클릭되면 이벤트 실행하게 설정해줍니다.
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void Update()
    {
        //서버의 상태를 좌측 상단 구석에 있는 text로 실시간 갱신되어서 표현합니다.
        connectionStatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void OnStartButtonClick()
    {
        if (string.IsNullOrEmpty(nicknameInputField.text))
        {
            // 입력 필드가 비어있다면 경고 메시지를 표시하고 함수를 종료합니다.
            warningText.gameObject.SetActive(true);
            return;
        }

        //닉네임 설정합니다.
        PhotonNetwork.NickName = nicknameInputField.text;
        
        //방을 연결합니다.
        PhotonNetwork.ConnectUsingSettings();

        //start button과 nicknameInputfield를 비활성화합니다.
        //그 다음 Loading text를 띄울겁니다. 이로써 유저들은 버튼이 눌린게 직접적으로 볼 수 있겠군요
        
        startButton.gameObject.SetActive(false);
        nicknameInputField.gameObject.SetActive(false);

        // 로딩 텍스트를 활성화합니다.
        loadingText.gameObject.SetActive(true);


        //이 Login Manager는 다른 씬에서도 남아있도록 합니다.
        DontDestroyOnLoad(gameObject);
    }

    //이 함수는 "연결되었을 때" 실행되는 함수를 오버라이드 한것입니다.
    public override void OnConnectedToMaster()
    {
        //로비 씬으로 이동합니다.
        SceneManager.LoadScene("GameLobby");
        Debug.Log(PhotonNetwork.NickName + "님 환영합니다.");
    }
}
