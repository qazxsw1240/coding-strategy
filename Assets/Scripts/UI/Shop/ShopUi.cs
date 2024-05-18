using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CodingStrategy.Entities;

public class ShopUi : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
