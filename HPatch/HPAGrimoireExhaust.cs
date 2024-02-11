using CountJest.Wizbo.Artifacts.Duo;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo;

internal sealed class HPAGrimoireExhaust
    {
        public HPAGrimoireExhaust()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToExhaust)),
            postfix: new HarmonyMethod(GetType(), nameof(HPAGrimoireExhaust.SendCardToExhaust3)));
        }
        public static void SendCardToExhaust3(Combat __instance, State s, Card card)
        {
            var artifact3 = s.EnumerateAllArtifacts().OfType<WizboDizzyArtifact>().FirstOrDefault();
            if (artifact3 is null)
                return;
            __instance.Queue(new AStatus()
            {
                artifactPulse = artifact3.Key(),
                status = Status.shield,
                statusAmount = 1,
                targetPlayer = true
            });

        }
    }
