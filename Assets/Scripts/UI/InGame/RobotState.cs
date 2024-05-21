using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RobotState : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Statement;
    public GameObject closeBtn;
    public TMP_Text userName;
    public TMP_Text robotState;
    public TMP_Text robotInfo;
    public void setStatement()
    {
        if (Statement != null)
        {
            Statement.SetActive(!Statement.activeSelf);
        }
    }

    public void CloseStatement()
    {
        Statement.SetActive(false);
    }

    public void setUserName(string name)
    {
        userName.SetText(name);
    }

    public void setRobotState(string state)
    {
        robotState.SetText(state);
    }

    public void setRobotInfo(string state)
    {
        robotInfo.SetText(state);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
