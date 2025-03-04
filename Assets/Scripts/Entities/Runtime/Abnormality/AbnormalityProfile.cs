using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    [CreateAssetMenu(
        fileName = "Abnormality",
        menuName = "CodingStrategy/Abnormality/Create Abnormality")]
    public class AbnormalityProfile : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }
    }
}
