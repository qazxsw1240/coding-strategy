using System.Runtime.CompilerServices;

using ExitGames.Client.Photon;

using Photon.Realtime;

namespace CodingStrategy.Utility
{
    public static class PhotonUtils
    {
        public static void SetCustomProperties(this Player player, params (object, object)[] properties)
        {
            player.SetCustomProperties(properties.ToHashtable());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Hashtable ToHashtable(this (object, object)[] properties)
        {
            Hashtable hashtable = new Hashtable();
            foreach ((object key, object value) in properties)
            {
                hashtable.Add(key, value);
            }
            return hashtable;
        }
    }
}
