using System;

using UnityEngine;
using UnityEngine.Rendering;

namespace CodingStrategy.Sound
{
    [DisallowMultipleComponent]
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        private static volatile bool _initialized;
        private static volatile SoundManager _instance;

        [SerializeField]
        private SerializedDictionary<SoundType, AudioSource> _audioSources;

        public static ISoundManager Instance
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            InternalInitialize();
        }

        public void Play(
            AudioClip audioClip,
            SoundType type = SoundType.Effect,
            float pitch = 1.0f,
            float volume = 1.0f)
        {
            if (audioClip is null)
            {
                Debug.LogWarning("AudioClip is null");
                return;
            }

            AudioSource audioSource = _audioSources[type];
            if (type == SoundType.Bgm)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.volume = volume; // 볼륨 조절
                audioSource.Play();
            }
            else
            {
                audioSource.pitch = pitch;
                audioSource.volume = volume; // 볼륨 조절
                audioSource.PlayOneShot(audioClip);
            }
        }

        public AudioSource GetBgmSource()
        {
            return _audioSources[(int) SoundType.Bgm];
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            GameObject gameObject = new GameObject("SoundManager", typeof(SoundManager));
            DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<SoundManager>();
            _initialized = true;
        }

        public void OnStartButtonClickSounds()
        {
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Experience_Up");
            Play(effectClip);
            Debug.Log("Start button sound is coming out!");
        }

        public void OnNicknameChangedSounds(string newNickname)
        {
            AudioClip typingSoundClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
            Play(typingSoundClip, SoundType.Effect, 3.0f, 0.6f);
        }

        public void LobbyRoomButtonSound()
        {
            return;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameLobby_UI_ClickSound");
            Play(effectClip);

            Debug.Log(effectClip);

            if (effectClip == null)
            {
                Debug.Log("Effect clip is null");
            }
        }

        private void InternalInitialize()
        {
            if (_audioSources == null)
            {
                _audioSources = new SerializedDictionary<SoundType, AudioSource>();
            }
            SoundType[] types = (SoundType[]) Enum.GetValues(typeof(SoundType));
            foreach (SoundType type in types)
            {
                GameObject track = new GameObject(type.ToString(), typeof(AudioSource));
                AudioSource audioSource = track.GetComponent<AudioSource>();
                if (type == SoundType.Bgm)
                {
                    audioSource.loop = true;
                }

                _audioSources[type] = audioSource;
                track.transform.parent = transform;
            }
        }
    }

    public interface ISoundManager
    {
        public abstract void Play(
            AudioClip audioClip,
            SoundType type = SoundType.Effect,
            float pitch = 1.0f,
            float volume = 1.0f);

        public AudioSource GetBgmSource();
    }
}
