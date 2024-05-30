using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CodingStrategy.UI.InGame
{
    public class SetBadSectorDetail : MonoBehaviour
    {
        public TMP_Text Name;
        public TMP_Text Description;

        public void SetName(string BadSectorName)
        {
            Name.text = BadSectorName;
        }
        public void SetDescription(string BadSectorDescription)
        {
            Description.text = BadSectorDescription;
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