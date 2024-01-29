using CountJest.Wizbo.Artifacts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPCoreExhaust
    {
        public HPCoreExhaust()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToExhaust)),
            postfix: new HarmonyMethod(GetType(), nameof(HPCoreExhaust.SendCardToExhaust1)));
        }
        public static void SendCardToExhaust1(Combat __instance, State s, Card card)
        {
            var artifact1 = s.EnumerateAllArtifacts().OfType<FramjificentCore>().FirstOrDefault();
            if (artifact1 is null)
                return;
            __instance.Queue(new ADrawCard()
            {
                artifactPulse = artifact1.Key(),
                count = 1,
            });
        }
    }
}