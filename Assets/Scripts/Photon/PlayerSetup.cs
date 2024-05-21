using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviour
{
    public string nickname;

    [PunRPC]
    public void SetNickname(string _name) 
    {
        nickname = _name;
    }
}
