using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts.Duo;

internal sealed class WizboPeriArtifact : Artifact, IDemoArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("WizboPeriArtifact", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/parallelstabilizer.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Parallel Stabilizer", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Parallel Stabilizer", "description"]).Localize
        });
        ModEntry.Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(WizboPeriArtifact), new[] { ModEntry.Instance.Wizbo_Deck.Deck, Deck.peri });
    }
    public override void OnTurnStart(State s, Combat c)
    {
        if (c.turn == 1)
        {
            c.QueueImmediate(new AStatus()
            {
                status = Status.tableFlip,
                statusAmount = 1,
                targetPlayer = true
            });
            this.Pulse();
        }
    }
}
