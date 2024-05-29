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
        public string Id;

        void SetCommandName(string name)
        {
            _commandName.text = name;
        }

        void SetCommandDescription(string description)
        {
            _commandDescription.text = description;
        }

        void SetCommandAttackRange(int enhancedLevel)
        {
            switch (int.Parse(Id))
            {
                case 1:
                    //_attackRange.sprite = enhancedLevel == 1 ? sprite
                    //    : enhancedLevel == 2 ? sprite
                    //    : enhancedLevel == 3 ? sprite;
					break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    break;
                case 19:
                    break;
                case 20:
                    break;
                case 21:
                    break;
                case 22:
                    break;
                case 23:
                    break;
                case 24:
                    break;
                case 25:
                    break;
                default:
                    Debug.Log("Wrong Command Detail Id: " + Id);
                    break;
            }
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