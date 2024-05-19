namespace CodingStrategy.Entities.Animations
{
    using System.Collections;
    using UnityEngine;

    public class TileAnimation : IEnumerator
    {
        private const float DefaultTimeResolution = 0.01f;

        private readonly GameObject _gameObject;
        private readonly float _depth;
        private readonly float _time;
        private readonly int _steps;

        private int _currentStep;

        public TileAnimation(GameObject gameObject, float depth, float time)
        {
            _gameObject = gameObject;
            _depth = depth;
            _time = time;
            _steps = (int) (_time / DefaultTimeResolution);
            _currentStep = 0;
        }

        public void Reset()
        {
            _currentStep = 0;
        }

        public bool MoveNext()
        {
            return ++_currentStep <= _steps;
        }

        public object Current
        {
            get
            {
                float dy = (_depth) / _steps;
                Vector3 position = _gameObject.transform.position;
                if ((_currentStep << 1) <= _steps)
                {
                    position.y -= dy;
                }
                else
                {
                    position.y += dy;
                }
                _gameObject.transform.position = position;
                return new WaitForSeconds(DefaultTimeResolution);
            }
        }
    }
}
