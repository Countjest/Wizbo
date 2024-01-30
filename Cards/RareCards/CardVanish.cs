using System;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

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
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<Ship> ships = [s.ship];
        {
            if (upgrade == Upgrade.B)
            {
                ships.Add(c.otherShip);
            }
        }

        List<CardAction> actions = [];
        foreach (var ship in ships)
            for (var partIndex = 0; partIndex < ship.parts.Count; partIndex++)
                if (ship.parts[partIndex].type != PType.empty)
                    actions.Add(new AVanishPart
                    {
                        TargetPlayer = ship.isPlayerShip,
                        WorldX = ship.x + partIndex,
                        omitFromTooltips = true,
                    });
        return actions;
    }
    public sealed class AVanishPart : CardAction
    {
        [JsonProperty]
        public required bool TargetPlayer;

        [JsonProperty]
        public required int WorldX;
        public override void Begin(G g, State s, Combat c)
        {
            base.Begin(g, s, c);
            timer = 0;

            var ship = TargetPlayer ? s.ship : c.otherShip;
            if (ship.GetPartAtWorldX(WorldX) is not { } part)
                return;
            foreach (var partIndex in ship.parts)
            {
                if (part.type != PType.cockpit)
                {
                    part.type = PType.empty;
                }
                if (part.skin != "parts/empty.png")
                {
                    part.skin = "parts/empty.png";
                }
            }
        }
    }
}

