using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopLevel : MonoBehaviour
{
    public TMP_Text Level1;
    public TMP_Text Level2;

    public void setLevel(int level)
    {
        Level1.SetText(level.ToString());
        Level2.SetText(level.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
