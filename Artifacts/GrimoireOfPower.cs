using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts;

internal sealed class GrimoireOfPower : Artifact, IDemoArtifact
{
    public int BoostCounter = 0;
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("GrimoireOfPower", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Wizbo_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/GrimoireOfPower.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GrimoireOfPower", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GrimoireOfPower", "description"]).Localize
        });
    }
    public override void OnTurnStart(State s, Combat c)
    {
        if (!c.isPlayerTurn)
            return;
        this.BoostCounter += 1;
        if (this.BoostCounter == 3)
        {
            c.QueueImmediate(new AStatus()
            {
                statusAmount = 1,
                targetPlayer = true
            });
            this.BoostCounter = 0;
            this.Pulse();
        }
    }

    public override void OnReceiveArtifact(State state)
    {
        this.BoostCounter = 0;
    }

    public override int? GetDisplayNumber(State s)
    {
        if (this.BoostCounter != 0)
            return this.BoostCounter;
        return null;
    }
}
