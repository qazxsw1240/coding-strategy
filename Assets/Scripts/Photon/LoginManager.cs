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

    private SoundManager soundManager;
    public SceneChanger sceneChanger;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // Bgm을 불러오고 재생합니다.
        AudioClip BgmClip = Resources.Load<AudioClip>("Sound/Game_Play_Ost");
        soundManager.Play(BgmClip, Sound.Bgm, 1.0f, 0.1f);
        Debug.Log("Bgm is comming out!");

        // startbutton이 클릭되면 이벤트 실행하게 설정해줍니다.
        startButton.onClick.AddListener(OnStartButtonClick);

        // 닉네임 입력 필드의 이벤트에 리스너 추가
        nicknameInputField.onValueChanged.AddListener(OnNicknameChanged);

    }

    void Update()
    {
        //서버의 상태를 좌측 상단 구석에 있는 text로 실시간 갱신되어서 표현합니다.
        connectionStatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void OnStartButtonClick()
    {
        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Experience_Up");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Start button sound is comming out!");

        if (string.IsNullOrEmpty(nicknameInputField.text))
        {
            // 입력 필드가 비어있다면 경고 메시지를 표시하고 함수를 종료합니다.
            warningText.gameObject.SetActive(true);
            return;
        }

        //닉네임 설정합니다.
        PhotonNetwork.LocalPlayer.NickName = nicknameInputField.text;
        
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
        //sceneChanger.ChangeScene("GamemLobby", "Sound/GameLobby_Sleepy Sunshine");
        Debug.Log(PhotonNetwork.NickName + "님 환영합니다.");
    }

    // 닉네임이 변경될 때마다 호출되는 콜백 함수
    private void OnNicknameChanged(string newNickname)
    {
        // 닉네임이 변경될 때마다 효과음 재생
        AudioClip typingSoundClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
        soundManager.Play(typingSoundClip, Sound.Effect, 3.0f, 0.6f);
    }
}
