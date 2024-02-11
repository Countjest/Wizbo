using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts.Duo;

internal sealed class WizboDizzyArtifact : Artifact, IDemoArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("WizboDizzyArtifact", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AegisGrimoire.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AegisGrimoire", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AegisGrimoire", "description"]).Localize
        });
        ModEntry.Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(WizboDizzyArtifact), new[] { ModEntry.Instance.Wizbo_Deck.Deck, Deck.dizzy });
    }
}
