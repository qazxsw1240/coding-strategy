using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingStrategy.UI.InGame
{
    public class InvokeBadSectorClickEvent : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
			
		}

        // Update is called once per frame
        void Update()
        {
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					GameObject ActiveChild = null;
					foreach (Transform child in transform)
					{
						if (child.gameObject.activeSelf)
						{
							ActiveChild = child.gameObject;
							break;
						}
					}
					BadSectorClickEvent badSectorClickEvent = GameObject.Find("AlwaysOnTop").GetComponent<BadSectorClickEvent>();
					badSectorClickEvent.OnBadSectorClickEvent.Invoke(ActiveChild.name);
					badSectorClickEvent.setBadSectorDetail = GetComponent<SetBadSectorDetail>();
				}
			}
        }
    }
}