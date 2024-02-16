using CountJest.Wizbo.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CountJest.Wizbo.Artifacts.Duo;

internal sealed class WizboRiggsArtifact : Artifact, IDemoArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("WizboRiggsArtifact", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/containerofnourishment.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Container Of Nourishment", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Container Of Nourishment", "description"]).Localize
        });
        ModEntry.Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(WizboRiggsArtifact), new[] { ModEntry.Instance.Wizbo_Deck.Deck, Deck.riggs });
    }
}
