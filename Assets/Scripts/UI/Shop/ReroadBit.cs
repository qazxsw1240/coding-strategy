using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CodingStrategy.UI.Shop
{
    public class ReroadBit : MonoBehaviour
    {
        public TMP_Text reroadBit;

        public void setReroadBit(int bit)
        {
            reroadBit.SetText(bit.ToString());
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
