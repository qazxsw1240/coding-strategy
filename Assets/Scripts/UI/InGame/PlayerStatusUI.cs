using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatusUI : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Money;
    public GameObject robotLife;
    public GameObject playerLife;
    public void setName(string name)
    {
        Name.text = name; 
    }

    public void setMoney(int money)
    {
        Money.text = money.ToString() + " Bit";
        if(money<0)
        {
            Money.color = Color.red;
        }
    }

    public void setPlayerHP(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            playerLife.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = hp; i < 3; i++)
        {
            playerLife.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void setRobotHP(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            robotLife.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = hp; i < 5; i++)
        {
            robotLife.transform.GetChild(i).gameObject.SetActive(false);
        }
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
