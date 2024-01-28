using Nickel;
using System.Reflection;

namespace CountJest.Wizbo.Artifacts;

internal sealed class FriendlyHearth : Artifact, IDemoArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("FriendlyHearth", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/counting.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FriendlyHearth", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FriendlyHearth", "description"]).Localize
        });
    }
    public override string Description()
    {
        return "Your ship <c=downside>overheats</c> at 4 instead of 3.";
    }

    public override void OnReceiveArtifact(State state)
    {
        state.ship.heatTrigger++;
    }

    public override void OnRemoveArtifact(State state)
    {
        state.ship.heatTrigger--;
    }
}
