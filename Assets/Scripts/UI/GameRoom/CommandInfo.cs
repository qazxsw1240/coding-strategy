using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CommandInfo : MonoBehaviour
{
    public GameObject commandInfo;
    public Transform command;
    public GameObject commandParent;

    public TMP_Text commandName;
    public TMP_Text commandDetail;
    //public GameObject attackRange;

    public void setCommandInfo()
    {
        Vector3 commandPos = command.position;
        commandInfo.SetActive(true);
        commandInfo.transform.position = new Vector3(commandPos.x + 200f, commandPos.y - 60f, commandPos.z);
    }

    /*public void setCommandInfo()
    {
        Vector3 commandPos;
        for (int i = 0; i<8; i++)
        {
            if(commandParent.transform.GetChild(i) == command)
            {
                commandPos = commandParent.transform.GetChild(i).transform.position;
                commandInfo.SetActive(true);
                commandInfo.transform.position = new Vector3(commandPos.x + 200f, commandPos.y - 60f, commandPos.z);
                break;
            }
        }
    }*/

    public void setCommandName(string commandname)
    {
        commandName.SetText(commandname);
    }

    public void setCommandDetail(string detail)
    {
        commandDetail.SetText(detail);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
