using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class TileChangeAnim : MonoBehaviour
{
    // 애니메이션을 표현할 Tile입니다.
    public GameObject Tile;

    // 설치될 상태이상들입니다.
    public GameObject stack;
    public GameObject troijan;
    public GameObject warm;
    public GameObject malware;
    public GameObject spyware;
    public GameObject adware;

    public int TileIndex = -1;
    
    public void TileChange(int TileIndex)
    {
        //   아래는 dotween 예시사항입니다. Dotween은 애니메이션을 직접적으로 큐에 추가하여 순서대로 실행합니다.
        //   연속적인 애니메이션을 어떻게 몇초간 실행할 것인지 나열하였습니다.
        //   
        //   finalSequence = DOTween.Sequence().SetAutoKill(false).Pause()
        //   DOTween.Sequence().SetAutoKill(false).Pause()는 반복 여부를 설정합니다.
        //
        //              Prepend(동작) 가장 앞쪽에 추가하고자 할 때
        //              Append(동작) 가장 뒤에 추가하고자 할 때
        //              Join(동작) 앞에 추가된 동작과 동시에 작동할 때
        //              Insert(시간, 동작) 순서와 상관없이 일정 시간이 되면 동작할 때
        //
        //    다음은 예시 사항입니다.
        //
        //   .Append(transform.DOMoveX(targetX, 3).SetEase(ease))
        //   .Append(text.DOFade(targetFadeValue, 3))
        //   .Append(transform.GetComponent<Renderer>().material.DOColor(targetColor, 3))
        //   .Append(transform.DOScale(targetScale, 3).SetEase(ease))
        //   .Append(transform.DOShakeRotation(targetShakeDurtation).SetEase(ease))
        //   .Append(text2.DOText(textString, 3));


        // 제가 의도한 느낌은 다음과 같습니다.

        // 타일맵 변경은 이미지만 뛰우는 것이 아니라,
        // 위로 살짝 띄웠다가
        // 반바퀴 회전하고
        // 타일위에 이미지를 띄우고(disable able 기능)
        // 아래로 다시 가라 앉힐 것입니다.

        Tile.transform.DORotate(new Vector3(0, 0, 180.0f), 1, RotateMode.Fast);
    }
}
