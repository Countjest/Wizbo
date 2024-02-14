using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardKoolahLimpoo : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Koolah Limpoo", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Koolah Limpoo", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            flippable = true,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.hermes,
                        statusAmount = 1,
                        targetPlayer = true,
                    },
                    new AMove()
                    {
                        targetPlayer = true,
                        dir = 1,
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.hermes,
                        statusAmount = 1,
                        targetPlayer = true,
                    },
                    new AMove()
                    {
                        targetPlayer = true,
                        dir = 1,
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.hermes,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new AMove()
                    {
                        targetPlayer = true,
                        dir = 1,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}