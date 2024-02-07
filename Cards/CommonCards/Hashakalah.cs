using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardHashakalah : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Hashakalah", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Hashakalah", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            exhaust = upgrade == Upgrade.A ? true : false,
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
                        status = Status.drawNextTurn,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new AAddCard
                    {
                        card = new CardMiasmaW
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Deck
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.drawNextTurn,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new AAddCard
                    {
                        card = new CardMiasmaW
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
                    new AStatus()
                    {
                        status = Status.drawNextTurn,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new AAddCard
                    {
                        card = new CardMiasmaW
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Hand,
                    },
                    new AAddCard
                    {
                        card = new CardMiasmaW
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Deck,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}