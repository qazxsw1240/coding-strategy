using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.InGame
{
    public class SetCommandDetail : MonoBehaviour
    {
        public TMP_Text _commandName;
        public TMP_Text _commandDescription;
        public Image _attackRange;

        void SetCommandName(string commandName)
        {
            _commandName.text = commandName;
        }

        void SetCommandDescription(string commandDescription)
        {
            _commandDescription.text = commandDescription;
        }

        void SetCommandAttackRange(Sprite sprite)
        {
            _attackRange.sprite = sprite;
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