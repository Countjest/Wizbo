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
    internal sealed class HPAMove
    {
        public HPAMove()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(CardAction), nameof(AMove)),
            postfix: new HarmonyMethod(GetType(), nameof(HPAMove.AMove)));
        }
        void AMove(G g, State s, Combat __instance)
        {
            var BobaContainer = s.EnumerateAllArtifacts().OfType<WizboRiggsArtifact>().FirstOrDefault();
            if (BobaContainer != null)
            {
                if (s.ship.Get(Status.strafe) > 0)
                {
                    __instance.Queue(new ASpawn()
                    {
                        thing = new Bolt()
                        {
                            boltType = BType.Magic,
                            targetPlayer = false
                        },
                        artifactPulse = BobaContainer.Key(),
                    });
                }
            }
        }
    }
}
