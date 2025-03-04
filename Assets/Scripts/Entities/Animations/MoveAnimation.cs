using System.Collections;

using UnityEngine;

namespace CodingStrategy.Entities.Animations
{
    public class MoveAnimation : IEnumerator
    {
        private const float DefaultTimeResolution = 0.01f;
        private readonly Vector3 _end;

        private readonly GameObject _gameObject;
        private readonly Vector3 _start;
        private readonly int _steps;
        private readonly float _time;

        private int _currentStep;

        public MoveAnimation(GameObject gameObject, Vector3 start, Vector3 end, float time)
        {
            _gameObject = gameObject;
            _start = start;
            _end = end;
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
                Vector3 distance = _end - _start;
                float dx = distance.x / _steps * _currentStep;
                float dz = distance.z / _steps * _currentStep;
                Vector3 position = _start;
                position.x = _start.x + dx;
                position.z = _start.z + dz;
                _gameObject.transform.position = position;
                return new WaitForSeconds(DefaultTimeResolution);
            }
        }
    }
}
