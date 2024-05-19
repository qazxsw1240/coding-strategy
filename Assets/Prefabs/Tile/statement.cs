using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statement : MonoBehaviour
{
    public float fadeInDuration = 1000f;
    public float fallDuration = 1f;
    public float fallDistance = 0.5f;
    public float shakeDuration = 0.1f;
    public float shakeStrength = 1f;
    public int shakeVibrato = 10;
    public Camera[] cameras;

    private Renderer itemRenderer;

    private void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        StartCoroutine(AnimateItem());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(AnimateItemReverse());
        }
    }

    IEnumerator AnimateItem()
    {
        // 동시에 시작할 애니메이션들을 담은 Sequence를 생성합니다.
        Sequence sequence = DOTween.Sequence();

        //
        Vector3 endPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + fallDistance, transform.position.z);
        sequence.Insert(0, transform.DOMove(endPosition, fallDuration).SetEase(Ease.InCubic));

        // Fade in 애니메이션
        sequence.Insert(0, itemRenderer.material.DOFade(0.5f, fadeInDuration));

        // Sequence를 시작합니다.
        yield return sequence.WaitForCompletion();

        // 카메라 흔들기 애니메이션
        foreach (Camera camera in cameras)
        {
            camera.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
        }
    }


    IEnumerator AnimateItemReverse()
    {
        Sequence sequence = DOTween.Sequence();

        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y+10, transform.position.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        sequence.Insert(0, transform.DOMove(startPosition, fallDuration).SetEase(Ease.InCubic));

        sequence.Insert(0, itemRenderer.material.DOFade(0, fadeInDuration));

        yield return sequence.WaitForCompletion();
        Destroy(gameObject);
    }
}
