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
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (deck == ModEntry.Instance.Wizbo_Deck.Deck)
        {
            SpeedCounter++;
            Pulse();
        }

        if (SpeedCounter >= 3)
        {
            combat.QueueImmediate(new AStatus
            {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = 1,
                artifactPulse = Key()
            });
            SpeedCounter = 0;
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
