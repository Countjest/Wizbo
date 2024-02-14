using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardSpillYourDrink : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Spill Your Drink", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Spill Your Drink", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A ? 1 : 1,
            exhaust = upgrade == Upgrade.A ? false : false,
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
                    new AMove()
                    {
                        dir = 3,
                        isTeleport = true,
                        isRandom = true,
                        targetPlayer = true
                    },
                    new AAddCard
                    {
                        card = new CardMiazbo
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Deck,
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AMove()
                    {
                        dir = 4,
                        isTeleport = true,
                        isRandom = true,
                        targetPlayer = true
                    },
                    new AAddCard
                    {
                        card = new CardMiazbo
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Hand,
                    },
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AMove()
                    {
                        dir = 3,
                        isTeleport = true,
                        isRandom = true,
                        targetPlayer = true
                    },
                    new ADrawCard()
                    {
                        count = 2,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}