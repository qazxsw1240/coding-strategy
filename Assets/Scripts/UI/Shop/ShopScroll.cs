using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
	public class ShopScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
	{
		private ScrollRect scrollRect;
		public bool isScrolling = false;

		public void OnBeginDrag(PointerEventData eventData)
		{
			isScrolling = true;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			isScrolling = false;
		}

		// Start is called before the first frame update
		void Start()
		{
			scrollRect = GetComponent<ScrollRect>();
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}