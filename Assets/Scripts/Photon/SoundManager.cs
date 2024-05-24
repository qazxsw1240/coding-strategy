using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];

    // 오디오 클립들을 저장할 딕셔너리
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    private void Start()
    {
        Init();
        DontDestroyOnLoad(this);
    }
    // 초기화
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");

        if (root == null) 
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
            for (int i = 0; i < soundNames.Length - 1; i++)
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
        foreach(AudioSource audioSource in _audioSources)
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
        if(audioClip == null)
        {
            Debug.LogWarning("AudioClip is null");
            return;
        }

        if(type == Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying) // BGM 중첩 방지
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        else
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
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
    AudioClip GetorAddAudioClip(string path, Sound type = Sound.Effect)
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


