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
        public string Description { get; private set; }
    }
}
