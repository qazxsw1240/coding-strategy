using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelupBit : MonoBehaviour
{
    public TMP_Text levelupBit;

    public void setLevelupBit(int bit)
    {
        levelupBit.SetText(bit.ToString());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
