using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.InGame
{
	public class OnClickCommandDetail : MonoBehaviour
	{
		public GameObject detailPrefab;

		// Start is called before the first frame update
		void Start()
		{
			GetComponent<Button>().onClick.AddListener(() =>
			{
				GameObject alwaysOnTop = GameObject.Find("AlwaysOnTop");
				foreach (Transform child in alwaysOnTop.transform)
				{
					if (child.name.Contains("CommandDetail"))
					{
						Destroy(child.gameObject);
					}
				}
				Instantiate(detailPrefab, alwaysOnTop.transform).transform.position = Input.mousePosition;
			});
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}