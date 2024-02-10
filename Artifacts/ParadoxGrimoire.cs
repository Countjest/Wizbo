using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts;

internal sealed class ParadoxGrimoire : Artifact, IDemoArtifact
{
    public int ParadoxCounter = 0;
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ParadoxGrimoire", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Wizbo_Deck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/ParadoxGrimoire.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ParadoxGrimoire", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ParadoxGrimoire", "description"]).Localize
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        this.ParadoxCounter = 0;
    }

    public override int? GetDisplayNumber(State s)
    {
        if (this.ParadoxCounter != 0)
            return this.ParadoxCounter;
        return null;
    }
}
