using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomManagerInfo : MonoBehaviour
{
    public TMP_Text Name;
    // is called before the first frame update

    public void setName(string name)
    {
        Name.SetText(name);
    }

    void Start()
    {
        setName("ABC");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
