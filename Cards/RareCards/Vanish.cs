using Nanoray.PluginManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nickel;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CountJest.Wizbo.Cards;
internal sealed class CardVanish : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Vanish", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Vanish", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 3,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "Vanish", "description", upgrade.ToString()])
        };
        return data;
    }
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) =>
        {
            List<Ship> ships = [state.ship, combat.otherShip];
            PropertyInfo[] array = typeof(Ship).GetProperties();
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo? Ship = array[i];
                foreach (var parts in typeof(List<Part>).GetProperties())
                {
                    PropertyInfo? PartTypeBeforeVanish = null;
                    PropertyInfo? Part = PartTypeBeforeVanish;
                }
            }
        });
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var TargetPlayer = s.ship;
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new VanishPart()
                    {
                        TargetPlayer = true,
                        WorldX = 0,
                    }

                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new VanishPart()
                    {
                        TargetPlayer = true,
                        WorldX = 0,
                    }

                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new VanishPart()
                    {
                        TargetPlayer = true,
                        WorldX = 0,
                    }

                };
                actions = cardActionList3;
                break;
        }
    }
}
public sealed class VanishPart : CardAction
{
    public readonly PType PartTypeAfterVanish;
    [JsonProperty]
    public readonly PType PartTypeBeforeVanish;
    [JsonProperty]
    public readonly string? PartSkinBeforeVanish;
    [JsonProperty]
    public readonly string? PartSkinAfterVanish;
    [JsonProperty]
    public required bool TargetPlayer;
    [JsonProperty]
    public required int WorldX;
    public override void Begin(G g, State s, Combat c)
    {
        var ship = TargetPlayer ? s.ship : c.otherShip;
        if (ship.GetPartAtWorldX(WorldX) is not { } part)
            return;
        {
            foreach (var partIndex in ship.parts)
            {
                if (part.type != PType.cockpit)
                {
                    part.type = PartTypeBeforeVanish;
                    part.type = PType.empty;
                    part.type = PartTypeAfterVanish;
                    part.skin = PartSkinBeforeVanish;
                    part.skin = partIndex.skin;
                }
                else return;
                if (part.type == PType.empty)
                {
                    part.skin = "parts/empty.png";
                    part.skin = PartSkinAfterVanish;
                }
            }
        }
    }
}