using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MoveAndRotate : MonoBehaviour
{
    public float rotationSpeed = 50f; // 얼만큼 회전
    public float moveSpeed = 0.6f; // 얼만큼 빠르게
    public float moveRange = 0.3f; // 얼만큼 이동

    public float fallDuration = 1f;
    public float fallDistance = 10f;
    public bool checkanimate = false;
    private Vector3 startPosition; // 오브젝트 위치 지점 저장
    [SerializeField] AudioSource musicsource;

    private void Start()
    {
        StartCoroutine(AnimateItem());
    }

//비트를 생성했을 때 회전 밑 아이템 이동이 
    private void Update()
    {

        if (checkanimate == true) {
            // Rotate around the z-axis at the specified speed
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Move up and down within the specified range at the specified speed
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveRange;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

// 비트를 먹게 될 경우 실행시키면 되는 함수입니다.
    public void GetBit()
    {
        rotationSpeed = 1000f;
        moveSpeed = 10f;
        moveRange = 2f;

        musicsource.Play();
        Destroy(gameObject, 0.47f);
    }

    IEnumerator AnimateItem()
    {
        startPosition = transform.position; // 생성된 위치 저장
        musicsource.time = 0.3f; // 음악의 0.3초부터 시작s

        // 동시에 시작할 애니메이션들을 담은 Sequence를 생성합니다.
        Sequence sequence = DOTween.Sequence();

        //
        Vector3 endPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        sequence.Insert(0, transform.DOMove(endPosition, fallDuration).SetEase(Ease.OutCubic));

        // Sequence를 시작합니다.
        yield return sequence.WaitForCompletion();
        checkanimate = true;
    }
}