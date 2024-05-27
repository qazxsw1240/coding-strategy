using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.InGame
{
    public class ResizeIcon : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta *= transform.parent.GetComponent<GridLayoutGroup>().cellSize / new Vector2(200.0f, 200.0f);
        }

        // Update is called once per frame
        void Update() { }
    }
}