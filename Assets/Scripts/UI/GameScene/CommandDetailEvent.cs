using CodingStrategy.UI.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CodingStrategy.UI.InGame
{
    public class CommandDetailEvent : MonoBehaviour
    {
        public UnityEvent<string> OnCommandClickEvent;
        public SetCommandDetail setCommandDetail;

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