using Nickel;
using System.Reflection;

namespace CountJest.Wizbo.Artifacts;

internal sealed class FramjificentCore : Artifact, IDemoArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("FramjificentCore", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/counting.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FramjificentCore", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FramjificentCore", "description"]).Localize
        });
    }
}
