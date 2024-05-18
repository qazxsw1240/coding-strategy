using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CodingStrategy.Entities;
using TMPro;

public class ShopUi : MonoBehaviour
{
    // PlayerBit
    public TMP_Text Bit;
    public TMP_Text BitEngtxt;

    // ShopLevel
    public TMP_Text Level1;
    public TMP_Text Level2;

    //LevelupBit
    public TMP_Text levelupBit;

    //ReroadBit
    public TMP_Text reroadBit;

    public GameObject plusIcon;

    public void AddPlusIcon()
    {
        Transform _parent = gameObject.transform.Find("SelectedCommandList");
		GameObject _object = Instantiate(plusIcon, _parent);
        _object.transform.SetSiblingIndex(_parent.childCount - 2);
    }

    public void SetShopCommandList(ICommand[] list)
    {
        foreach (ICommand command in list)
        {
            // command.Id;
            // command.Info;
        }
    }

    // PlayerBit
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

    // ShopLevel
    public void setLevel(int level)
    {
        Level1.SetText(level.ToString());
        Level2.SetText(level.ToString());
    }

    //LevelupBit
    public void setLevelupBit(int bit)
    {
        levelupBit.SetText(bit.ToString());
    }

    //ReroadBit
    public void setReroadBit(int bit)
    {
        reroadBit.SetText(bit.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        setBit(0);
        setLevel(0);
        setLevelupBit(0);
        setReroadBit(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
