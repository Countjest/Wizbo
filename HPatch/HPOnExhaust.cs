using CountJest.Wizbo.Artifacts;
using CountJest.Wizbo.Artifacts.Duo;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPOnExhaust
    {
        public HPOnExhaust()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToExhaust)),
            postfix: new HarmonyMethod(GetType(), nameof(HPOnExhaust.SendCardToExhaust)));
        }
        public static void SendCardToExhaust(Combat __instance, State s, Card card)
        {
            var ShipCore = s.EnumerateAllArtifacts().OfType<FramjificentCore>().FirstOrDefault();
            if (ShipCore != null)
            __instance.Queue(new ADrawCard()
            {
                artifactPulse = ShipCore.Key(),
                count = 1,
            });
            var EtherealBook = s.EnumerateAllArtifacts().OfType<EtherealGrimoire>().FirstOrDefault();
            if (EtherealBook != null)
                __instance.Queue(new AStatus()
                {
                    artifactPulse = EtherealBook.Key(),
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                });
            var ShieldBook = s.EnumerateAllArtifacts().OfType<WizboDizzyArtifact>().FirstOrDefault();
            if (ShieldBook != null)
                __instance.Queue(new AStatus()
                {
                    artifactPulse = ShieldBook.Key(),
                    status = Status.shield,
                    statusAmount = 1,
                    targetPlayer = true
                });
            var ParadoxBook = s.EnumerateAllArtifacts().OfType<ParadoxGrimoire>().FirstOrDefault();
            if (ParadoxBook != null)
                __instance.Queue(new ParadoxCardAction()
                {
                    artifactPulse = ParadoxBook.Key(),
                });
        }
    }

}