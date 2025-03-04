using CodingStrategy.UI.InGame;

using UnityEngine;
using UnityEngine.Events;

namespace CodingStrategy.UI.GameScene
{
    public class BadSectorClickEvent : MonoBehaviour
    {
        public UnityEvent<string> OnBadSectorClickEvent;
        public SetBadSectorDetail setBadSectorDetail;
    }
}
