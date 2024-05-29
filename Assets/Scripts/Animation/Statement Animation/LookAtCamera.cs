using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private RectTransform _canvas;

    private void Awake()
    {
        _canvas = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _canvas.LookAt(Camera.main.transform);
    }
}
