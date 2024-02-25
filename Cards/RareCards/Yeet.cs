using Nickel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace CountJest.Wizbo.Cards.RareCards;

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
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Yeet", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade switch
            {
                Upgrade.A => 0,
                Upgrade.B => 2,
                _ => 1
            },
            exhaust = true,
            retain = upgrade == Upgrade.B ? true : false,
            description = upgrade == Upgrade.B ? ModEntry.Instance.Localizations.Localize(["card", "Yeet", "descriptionB"]) : null,
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
                        card = new CardMiazbo
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
                        card = new CardMiazbo
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
                    new AAddCard
                    {
                        card = new CardMiazbo
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Hand
                    },
                };
                for (int i = 0; i < ((2*Epile)/3); i++)
                {
                    cardActionList3.Add(new AAttack
                    {
                        damage = GetDmg(s, 1),
                        omitFromTooltips = true
                    });
                }
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}