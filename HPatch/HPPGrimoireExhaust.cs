using CountJest.Wizbo.Artifacts;
using FMOD;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPPGrimoireExhaust
    {
        public HPPGrimoireExhaust()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToExhaust)),
            postfix: new HarmonyMethod(GetType(), nameof(HPPGrimoireExhaust.SendCardToExhaust4)));
        }
        public static void SendCardToExhaust4(Combat __instance, State s, Card card)
        {
            var artifact4 = s.EnumerateAllArtifacts().OfType<ParadoxGrimoire>().FirstOrDefault();
            if (artifact4 is null)
                return;
            __instance.Queue(new ExhaustCardAction2()
            {
                cardId2 = card.uuid,
                artifactPulse = artifact4.Key(),
            }) ;
            __instance.Queue(new ParadoxCardAction()
            {
                cardId3 = card.uuid,
                artifactPulse = artifact4.Key(),
            });          
        }
    }
}