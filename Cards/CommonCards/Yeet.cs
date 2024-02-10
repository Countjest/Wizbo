using Nickel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardYeet : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Yeet", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Yeet", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 1,
            exhaust = true,
            retain = upgrade == Upgrade.B ? true : false,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var Epile = 0;
        if (s.route is Combat)
        {
            foreach (Card card in c.exhausted)
            {
                Epile++;
            }
        }
        else Epile = 0;

        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>() 
                {
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, Epile),
                        iconName = "Exhausted Cards"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, Epile),
                        xHint = 1,
                    },
                    new AAddCard
                    {
                        card = new CardMiasma
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
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, Epile),
                        iconName = "Exhausted Cards"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, Epile),
                        xHint = 1,
                    },
                    new AAddCard
                    {
                        card = new CardYeet
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Hand
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, Epile),
                        iconName = "Exhausted Cards"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, Epile),
                        xHint = 1,
                    },
                    new AAddCard
                    {
                        card = new CardMiasma
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Hand
                    },
                    new AAddCard
                    {
                        card = new CardMiasma
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Discard
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}