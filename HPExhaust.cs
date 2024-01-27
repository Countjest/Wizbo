using CountJest.Wizbo.Artifacts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPExhaust
    {
        public HPExhaust()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToExhaust)),
            postfix: new HarmonyMethod(GetType(), nameof(HPExhaust.SendCardToExhaust)));
        }
        public static void SendCardToExhaust(Combat __instance, State s, Card card)
        {
            var artifact = s.EnumerateAllArtifacts().OfType<FramjificentCore>().FirstOrDefault();
            if (artifact is null)
                return;
            __instance.Queue(new ADrawCard()
            {
                artifactPulse = artifact.Key(),
                count = 1,
            });

        }
    }
}