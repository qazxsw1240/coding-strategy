using System;

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Camera mainCamera;

    public void Awake()
    {
        if (Camera.main is null)
        {
            throw new NullReferenceException("main camera is null.");
        }
        mainCamera = Camera.main;
        canvas = GetComponent<RectTransform>();
    }

    public void Update()
    {
        canvas.LookAt(mainCamera.transform);
    }
}
