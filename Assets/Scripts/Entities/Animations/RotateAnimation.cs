using System.Collections;

using UnityEngine;

namespace CodingStrategy.Entities.Animations
{
    public class RotateAnimation : IEnumerator
    {
        private const float DefaultTimeResolution = 0.01f;
        private readonly Quaternion _end;

        private readonly GameObject _gameObject;
        private readonly Quaternion _start;
        private readonly int _steps;
        private readonly float _time;

        private int _currentStep;

        public RotateAnimation(GameObject gameObject, Quaternion start, Quaternion end, float time)
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
                float ratio = (float) _currentStep / _steps;
                _gameObject.transform.rotation = Quaternion.Slerp(_start, _end, ratio);
                return new WaitForSeconds(DefaultTimeResolution);
            }
        }
    }
}
