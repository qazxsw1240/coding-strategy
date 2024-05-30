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
        public Sprite[] spriteList;

        public enum AttackArea
        {
            None,
            Forward,
            Left,
            Right,
            Backward,
            Forward2,
            Forward3,
            Backward3,
            ForwardLeft,
            ForwardRight,
            Malware2,
            Malware3,
            Around
        };

        public string Id;

        public void SetCommandName(string name)
        {
            _commandName.text = name;
        }

        public void SetCommandDescription(string description)
        {
            _commandDescription.text = description;
        }

        public void SetCommandAttackRange(int enhancedLevel)
        {
            switch (int.Parse(Id))
            {
                case 0:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 1:
                    _attackRange.sprite = spriteList[(int) AttackArea.Forward];
                    break;
                case 2:
                    _attackRange.sprite = spriteList[(int) AttackArea.Left];
                    break;
                case 3:
                    _attackRange.sprite = spriteList[(int) AttackArea.Right];
                    break;
                case 4:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 5:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 6:
                    _attackRange.sprite = spriteList[(int) AttackArea.ForwardLeft];
                    break;
                case 7:
                    _attackRange.sprite = spriteList[(int) AttackArea.ForwardRight];
                    break;
                case 8:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 9:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 10:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel == 1 ? AttackArea.Forward2 :
                        enhancedLevel == 2 ? AttackArea.Malware2 :
                        enhancedLevel == 3 ? AttackArea.Malware3 : AttackArea.None)];
                    break;
                case 11:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 12:
                    _attackRange.sprite = spriteList[(int) AttackArea.Around];
                    break;
                case 13:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel == 1 || enhancedLevel == 2
                        ?
                        AttackArea.Forward
                        :
                        enhancedLevel == 3
                            ? AttackArea.Forward3
                            : AttackArea.None)];
                    break;
                case 14:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel == 1 || enhancedLevel == 2
                        ?
                        AttackArea.Backward
                        :
                        enhancedLevel == 3
                            ? AttackArea.Backward3
                            : AttackArea.None)];
                    break;
                case 15:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel == 1 || enhancedLevel == 2
                        ?
                        AttackArea.Forward
                        :
                        enhancedLevel == 3
                            ? AttackArea.Forward3
                            : AttackArea.None)];
                    break;
                case 16:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel == 1 || enhancedLevel == 2
                        ?
                        AttackArea.Backward
                        :
                        enhancedLevel == 3
                            ? AttackArea.Backward3
                            : AttackArea.None)];
                    break;
                case 17:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 18:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 19:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 20:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 21:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 22:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 23:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 24:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 25:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                default:
                    Debug.Log("Wrong Command Detail Id: " + Id);
                    break;
            }
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
