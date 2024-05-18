using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUi : MonoBehaviour
{
    public GameObject plusIcon;

    public void AddPlusIcon()
    {
        Transform _parent = gameObject.transform.Find("SelectedCommandList");
		GameObject _object = Instantiate(plusIcon, _parent);
        _object.transform.SetSiblingIndex(_parent.childCount - 2);
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
