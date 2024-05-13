#nullable enable


namespace CodingStrategy
{
    using System.Collections;
    using Entities.CodingTime;
    using Entities.Robot;
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
        private IRobotDelegatePool _robotDelegatePool = null!;
        private IPlayerPool _playerPool = null!;

        private AnimationCoroutineManager _animationCoroutineManager = null!;

        public void Awake()
        {
            _boardDelegate = new BoardDelegateImpl(boardWidth, boardHeight);
            _robotDelegatePool = new RobotDelegatePoolImpl();
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
            CodingTimeExecutor codingTimeExecutor = gameObject.GetOrAddComponent<CodingTimeExecutor>();

            yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(codingTimeExecutor);

            RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

            runtimeExecutor.BoardDelegate = _boardDelegate;
            runtimeExecutor.RobotDelegatePool = _robotDelegatePool;
            runtimeExecutor.PlayerPool = _playerPool;
            runtimeExecutor.AnimationCoroutineManager = _animationCoroutineManager;

            yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(runtimeExecutor);
        }
    }
}
