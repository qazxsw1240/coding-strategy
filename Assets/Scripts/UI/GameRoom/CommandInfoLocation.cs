using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommandInfoLocation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform command;
    public GameObject commandInfo;

    /*public void OnButtonClick()
    {
        Vector3 commandPos = command.position;
        commandInfo.SetActive(true);
        commandInfo.transform.position = new Vector3(commandPos.x + 200f, commandPos.y - 100f, commandPos.z);
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 commandPos = command.position;
        commandInfo.SetActive(true);
        commandInfo.transform.position = new Vector3(commandPos.x + 200f, commandPos.y - 100f, commandPos.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        commandInfo.SetActive(false);
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
