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
		private SoundManager soundManager; // soundManager

		public void OnBeginDrag(PointerEventData eventData)
		{
			// Begin drag sound
			soundManager = FindObjectOfType<SoundManager>();
			soundManager.Init();
			AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_DragBtnClick_Sound");
			soundManager.Play(effectClip, Sound.Effect, 1.0f);
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
