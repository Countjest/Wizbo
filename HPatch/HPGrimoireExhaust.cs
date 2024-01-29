using CountJest.Wizbo.Artifacts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPGrimoireExhaust
    {
        public HPGrimoireExhaust()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToExhaust)),
            postfix: new HarmonyMethod(GetType(), nameof(HPGrimoireExhaust.SendCardToExhaust2)));
        }
        public static void SendCardToExhaust2(Combat __instance, State s, Card card)
        {
            var artifact2 = s.EnumerateAllArtifacts().OfType<EtherealGrimoire>().FirstOrDefault();
            if (artifact2 is null)
                return;
            __instance.Queue(new AStatus()
            {
                artifactPulse = artifact2.Key(),
                status = Status.overdrive,
                statusAmount = 1,
            });

        }
    }
}