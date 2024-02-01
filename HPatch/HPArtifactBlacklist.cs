using CountJest.Wizbo.Artifacts;
using FMOD.Studio;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPArtifactBlacklist
    {
        public HPArtifactBlacklist()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ArtifactReward), "GetBlockedArtifacts"),
            postfix: new HarmonyMethod(GetType(), nameof(HPArtifactBlacklist.postfix_GetBlockedArtifacts)));
        }
        public static void postfix_GetBlockedArtifacts(State s, HashSet<Type> __result)
        {
            bool v = s.ship.key == ModEntry.Instance.MagicTower_Ship.UniqueName;
            var ShipKey2 = v;
            HashSet<Type> hashSet = __result;
            if (ShipKey2 != false)
            {
                hashSet.Add(typeof(ArmoredBay));
            }
        }
    }
}
