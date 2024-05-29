using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneSound : MonoBehaviour
{

    SceneChanger sceneChanger;

    private void Awake()
    {
        sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();
        //soundmanager.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sceneChanger.SoundsVolumesUp("Sound/GameScene_Bgm"));
    }
}
