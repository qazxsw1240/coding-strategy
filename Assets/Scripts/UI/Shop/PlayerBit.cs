using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerBit : MonoBehaviour
{
    public TMP_Text Bit;
    public TMP_Text BitEngtxt;

    public void setBit(int bit)
    {
        if (bit < 0)
        {
            Bit.color = Color.red;
            BitEngtxt.color = Color.red;
        }
        else
        {
            Bit.color = Color.black;
            BitEngtxt.color = Color.black;
        }
        Bit.SetText(bit.ToString());
    }
    void Start()
    {
             
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
