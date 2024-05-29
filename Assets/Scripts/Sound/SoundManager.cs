using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Sound
{
    Bgm, // 배경음악
    Effect, // 효과음
    MaxCount, // Sound enum의 개수 세기 위해 존재
}

// SoundManager 클래스 
public class SoundManager : MonoBehaviour
{
    // 오디오 소스들을 저장할 리스트
    [SerializeField]
    private List<AudioSource> _audioSources = new List<AudioSource>;

    // 오디오 클립들을 저장할 딕셔너리
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public SceneChanger sceneChanger;
    public InputField nicknameInputField;
    public GameObject RoomEnterBtn;

    public static int initcheck=0;

    private void Awake()
    {
        if (initcheck == 0)
        { 
            Init();
            initcheck += 1;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        // 씬 이름이 GameStartScene이라면
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameStartScene")
        {
            // Bgm을 불러오고 재생합니다.
            AudioClip BgmClip = Resources.Load<AudioClip>("Sound/Game_Play_Ost");
            Play(BgmClip, Sound.Bgm, 1.0f, 0.1f);

            // 닉네임 입력 필드의 이벤트에 리스너 추가
            nicknameInputField.onValueChanged.AddListener(OnNicknameChanged);
        }
    }

    public void OnStartButtonClick()
    {
        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Experience_Up");
        Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Start button sound is comming out!");
    }

    private void OnNicknameChanged(string newNickname)
    {
        // 닉네임이 변경될 때마다 효과음 재생
        AudioClip typingSoundClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
        Play(typingSoundClip, Sound.Effect, 3.0f, 0.6f);
    }

    public void LobbyRoomButtonSound()
    {
        // 추가적으로 실행하고자 하는 코드
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameLobby_UI_ClickSound");
        Debug.Log(Sound.Effect);
        Debug.Log(effectClip);
        Play(effectClip, Sound.Effect, 1.0f);
    }

    // 초기화
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");

        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            //Object.DontDestroyOnLoad(root); // Bgm을 씬마다 다르게 할거라면 주석처리 해야함.

            string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect", "maxCount"
            for (int i = 0; i < soundNames.Length; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
        }
    }

    // 모든 사운드 초기화
    public void Clear()
    {
        // 모든 오디오 소스 정지 및 클립 제거
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }

    // 오디오 클립 재생
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip is null");
            return;
        }

        if (type == Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying) // BGM 중첩 방지
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            //audioSource.volume = 0.5f; // 볼륨 조절
            audioSource.Play();
        }

        else
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            Debug.Log(audioSource);
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    // 볼륨 조절 가능한 오디오 클립 재생
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f, float volumn = 1.0f)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip is null");
            return;
        }

        if (type == Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying) // BGM 중첩 방지
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.volume = volumn; // 볼륨 조절
            audioSource.Play();
        }

        else
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.volume = volumn; // 볼륨 조절
            audioSource.PlayOneShot(audioClip);
        }
    }


    // 경로를 통해 오디오 클립 재생
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetorAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }


    // 오디오 클립 로드 또는 딕셔너리에서 검색
    public AudioClip GetorAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if (!path.Contains("Sound/"))
            path = $"Sounds/{path}"; // Sound 폴더 안에 저장될 수 있도록

        AudioClip audioClip = null;

        if (type == Sound.Bgm) // BGM 배경음악 클립 로드
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public AudioSource GetBgmSource()
    {
        return _audioSources[(int)Sound.Bgm];
    }
}


// Manager 클래스
public class Manager : MonoBehaviour
{
    private static Manager s_instance;

    public static Manager Instance
    {
        get
        {
            if (s_instance == null)
                Init();
            return s_instance;
        }
    }

    private SoundManager _soundManager;
    public static SoundManager Sound
    {
        get { return Instance._soundManager; }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Manager>();
                DontDestroyOnLoad(go);
            }

            s_instance = go.GetComponent<Manager>();
            s_instance._soundManager = go.AddComponent<SoundManager>();
            s_instance._soundManager.Init();
        }
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            _soundManager = gameObject.AddComponent<SoundManager>();
            _soundManager.Init();
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }
}


