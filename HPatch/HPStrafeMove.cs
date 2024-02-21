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
    internal sealed class HPStrafeMove
    {
        public HPStrafeMove()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(CardAction), nameof(AMove)),
            postfix: new HarmonyMethod(GetType(), nameof(HPStrafeMove.AMove)));
        }
        public static void AMove(CardAction __instance, State s, Combat c)
        {
            var BobaContainer = s.EnumerateAllArtifacts().OfType<WizboRiggsArtifact>().FirstOrDefault();
            if (BobaContainer != null)
            {
                if (s.ship.Get(Status.strafe) > 0)
                {
                    c.QueueImmediate(new ASpawn()
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
