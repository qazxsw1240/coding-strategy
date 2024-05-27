using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.InGame
{
    public class SetIconColor : MonoBehaviour
    {
        private static Color[] colorList = new Color[] { 
            new Vector4(239, 239, 239, 255) / 255, 
            new Vector4(182, 215, 168, 255) / 255, 
            new Vector4(111, 168, 220, 255) / 255, 
            new Vector4(103, 78, 167, 255) / 255, 
            new Vector4(249, 203, 106, 255) / 255}; 

        // 다섯 단계의 등급에 따라 0~4의 index를 입력으로 받음.
        public void SetColor(int index)
        {
            GetComponent<Image>().color = colorList[index];
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
}