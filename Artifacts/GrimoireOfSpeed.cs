using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts;

internal sealed class GrimoireOfSpeed : Artifact, IDemoArtifact
{
    public int SpeedCounter = 0;
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("GrimoireOfSpeed", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Wizbo_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/GrimoireOfSpeed.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GrimoireOfSpeed", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GrimoireOfSpeed", "description"]).Localize
        });
    }
    public override void OnTurnStart(State s, Combat c)
    {
        if (!c.isPlayerTurn)
            return;
        this.SpeedCounter += 1;
        if (this.SpeedCounter == 4)
        {
            c.QueueImmediate(new ADrawCard()
            {
                count = 1,
            });
            this.SpeedCounter = 0;
            this.Pulse();
        }
    }

    public override void OnReceiveArtifact(State state)
    {
        this.SpeedCounter = 0;
    }

    public override int? GetDisplayNumber(State s)
    {
        if (this.SpeedCounter != 0)
            return this.SpeedCounter;
        return null;
    }
}
