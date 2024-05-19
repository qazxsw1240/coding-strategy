using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tile;

    public IEnumerator BounceEffect(Rigidbody tileRigidbody)
    {
        
        
        Vector3 originalPosition = transform.position;
        Vector3 scale = transform.localScale;


        // 아래로 조금 내려갑니다.
        yield return transform.DOMoveY(transform.position.y - 0.1f, 0.2f).WaitForCompletion();

        // y의 스케일을 0.6 줄여서 다른 오브젝트들과 높이를 맞춥니다.
        scale.y = 0.6f;
        // Y 위치를 0.3으로 설정합니다.
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);

        this.gameObject.transform.localScale = scale;
        
        // 다시 원래 위치로 돌아옵니다.
        yield return transform.DOMoveY(originalPosition.y, 0.2f).WaitForCompletion();

        if (tileRigidbody != null)
        {
            tileRigidbody.isKinematic = true;
        }
    }
}