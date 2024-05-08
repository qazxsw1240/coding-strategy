#nullable enable

using System.Collections;
using CodingStrategy.Entities.CodingTime;

namespace CodingStrategy
{
    using Entities.Board;
    using Entities.Player;
    using Entities.Runtime;
    using Entities;
    using Unity.VisualScripting;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public int boardWidth = 9;
        public int boardHeight = 9;

        private IBoardDelegate _boardDelegate = null!;
        private IPlayerPool _playerPool = null!;

        private AnimationCoroutineManager _animationCoroutineManager = null!;

        public void Awake()
        {
            _boardDelegate = new BoardDelegateImpl(boardWidth, boardHeight);
            _playerPool = new PlayerPoolImpl();
            Debug.Log("AnimationCoroutineManager created.");
            _animationCoroutineManager = gameObject.GetOrAddComponent<AnimationCoroutineManager>();
        }

        public void Start()
        {
            Debug.Log("GameManager instance started.");
            StartCoroutine(StartGameManagerCoroutine());
        }

        private IEnumerator StartGameManagerCoroutine()
        {
            Debug.Log("GameManager instance started.");

            CodingTimeExecutor codingTimeExecutor =
                gameObject.GetOrAddComponent<CodingTimeExecutor>();

            yield return null; // Start() 호출 대기

            yield return codingTimeExecutor.Coroutine;

            Destroy(codingTimeExecutor);

            RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

            runtimeExecutor.BoardDelegate = _boardDelegate;
            runtimeExecutor.PlayerPool = _playerPool;

            yield return null; // Start() 호출 대기

            yield return runtimeExecutor.Coroutine;

            Destroy(runtimeExecutor);
        }
    }
}
