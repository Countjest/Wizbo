using Nickel;
using System.Reflection;

namespace CountJest.Wizbo.Artifacts;

internal sealed class ReinforcedGate : Artifact, IDemoArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ReinforcedGate", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/ReinforcedGate.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ReinforcedGate", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ReinforcedGate", "description"]).Localize
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        foreach (Part part in state.ship.parts)
        {
            if (part.type == PType.missiles)
            {
                part.damageModifier = PDamMod.armor;
            }
        }
    }
}

