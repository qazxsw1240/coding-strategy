using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomScene3DUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] lilrobot;
    public TextMeshProUGUI[] nicknames;
    public GameObject CommandView;
    public Button button;

    private string[] lastNicknames;

    private void Start()
    {
        lastNicknames = new string[nicknames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        NickNamechanged();
    }

    public void NickNamechanged()
    {
        for (int i = 0; i < nicknames.Length; i++)
        {
            if (nicknames[i].text != lastNicknames[i])
            {
                Nicknamecheck(i);
                lastNicknames[i] = nicknames[i].text;
            }
        }
    }



    public void Nicknamecheck(int i)
    {
        
        if (nicknames[i].text != "(없음)")
        {
            //lilrobot[i].SetActive(true);
            StartCoroutine(ActivateWithAnimation(lilrobot[i], 10.0f, 1.0f));
        }
        else
        {
            lilrobot[i].SetActive(false);
        }
    }

    public IEnumerator ActivateWithAnimation(GameObject obj, float fallDistance, float fallDuration)
    {
        obj.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        Vector3 endPosition = obj.transform.position;

        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + fallDistance, obj.transform.position.z);
        
        sequence.Insert(0, obj.transform.DOMove(endPosition, fallDuration).SetEase(Ease.OutCubic));
        
        yield return sequence.WaitForCompletion();
    }


    public void OnScrollView()
    {
        CommandView.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OffScrollView()); //인자가 있을 때 람다식 사용
    }

    public void OffScrollView()
    {
        CommandView.gameObject.SetActive(false);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnScrollView()); //인자가 있을 때 람다식 사용
    }
}
