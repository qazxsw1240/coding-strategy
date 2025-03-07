using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Command
{
    [CreateAssetMenu(
        fileName = "CommandProfile",
        menuName = "CodingStrategy/Command/Create CommandProfile")]
    public class CommandProfile : ScriptableObject
    {
        [field: SerializeField]
        public string ID { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }

        [field: Range(1, 5)]
        [field: SerializeField]
        public int Grade { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public Sprite DetailIcon { get; private set; }
    }
}
