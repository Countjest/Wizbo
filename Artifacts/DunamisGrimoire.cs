using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts;

internal sealed class DunamisGrimoire : Artifact, IDemoArtifact
{
    public int DunamisCounter = 0;
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("DunamisGrimoire", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Wizbo_Deck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/DunamisGrimoire.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DunamisGrimoire", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DunamisGrimoire", "description"]).Localize
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        this.DunamisCounter = 0;
    }

    public override int? GetDisplayNumber(State s)
    {
        if (this.DunamisCounter != 0)
            return this.DunamisCounter;
        return null;
    }

}
