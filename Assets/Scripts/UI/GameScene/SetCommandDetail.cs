using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class SetCommandDetail : MonoBehaviour
    {
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
        }

        public TMP_Text _commandName;
        public TMP_Text _commandDescription;
        public Image _attackRange;
        public Sprite[] spriteList;

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
                case 9:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 10:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel switch
                    {
                        1 => AttackArea.Forward2,
                        2 => AttackArea.Malware2,
                        3 => AttackArea.Malware3,
                        var _ => AttackArea.None
                    })];
                    break;
                case 11:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                case 12:
                    _attackRange.sprite = spriteList[(int) AttackArea.Around];
                    break;
                case 13:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel switch
                    {
                        1 => AttackArea.Forward,
                        2 => AttackArea.Forward,
                        3 => AttackArea.Forward3,
                        var _ => AttackArea.None
                    })];
                    break;
                case 14:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel switch
                    {
                        1 => AttackArea.Backward,
                        2 => AttackArea.Backward,
                        3 => AttackArea.Backward3,
                        var _ => AttackArea.None
                    })];
                    break;
                case 15:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel switch
                    {
                        1 => AttackArea.Forward,
                        2 => AttackArea.Forward,
                        3 => AttackArea.Forward3,
                        var _ => AttackArea.None
                    })];
                    break;
                case 16:
                    _attackRange.sprite = spriteList[(int) (enhancedLevel switch
                    {
                        1 => AttackArea.Backward,
                        2 => AttackArea.Backward,
                        3 => AttackArea.Backward3,
                        var _ => AttackArea.None
                    })];
                    break;
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    _attackRange.sprite = spriteList[(int) AttackArea.None];
                    break;
                default:
                    Debug.Log("Wrong Command Detail Id: " + Id);
                    break;
            }
        }
    }
}
