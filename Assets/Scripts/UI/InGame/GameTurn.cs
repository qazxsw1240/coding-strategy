using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTurn : MonoBehaviour
{
    public TMP_Text gameTurn;

    public void SetTurn(int turn)
    {
        gameTurn.text = turn.ToString();
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
