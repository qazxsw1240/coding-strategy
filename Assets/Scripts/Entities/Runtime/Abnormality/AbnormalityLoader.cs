using System;
using System.Collections.Generic;

using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public class AbnormalityLoader : MonoBehaviour
    {
        private static readonly IDictionary<string, AbnormalityProfile> Abnormalities =
            new Dictionary<string, AbnormalityProfile>();

        public static AbnormalityProfile Load(string name)
        {
            if (Abnormalities.TryGetValue(name, out AbnormalityProfile abnormalityProfile))
            {
                return abnormalityProfile;
            }
            string path = $"Abnormalities/{name}";
            AbnormalityProfile profile = Resources.Load<AbnormalityProfile>(path);
            if (!profile)
            {
                throw new NullReferenceException($"Failed to load abnormality profile: {path}");
            }
            Abnormalities.Add(name, profile);
            return profile;
        }
    }
}
