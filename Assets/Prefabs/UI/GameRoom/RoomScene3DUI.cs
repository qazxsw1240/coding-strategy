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

    // Update is called once per frame
    void Update()
    {
        Nicknamecheck();
    }

    public void Nicknamecheck()
    {
        for(int i = 0; i < nicknames.Length; i++)
        {
            if (nicknames[i].text != "(없음)")
            {
                lilrobot[i].SetActive(true);
            }
            else
            {
                lilrobot[i].SetActive(false);
            }
        }
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
