using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardPocusCrocus : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("PocusCrocus", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PocusCrocus", "name"]).Localize,
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A? 0 : 2,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "PocusCrocus", "description", upgrade.ToString()])
            /* In a similar manner to how we localized card names, we'll localize their descriptions
             * For example, if Sheep Dream is upgraded to B, this description would try getting information from card > SheepDream > Description > B in the locale file */
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
                        destination = CardDestination.Deck
                    },
                    new AAddCard
                    {
                        card = new CardToxic
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Discard
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
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
                        destination = CardDestination.Deck
                    },
                    new AAddCard
                    {
                        card = new CardToxic
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Discard
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
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
                        destination = CardDestination.Deck
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
                    new AAddCard
                    {
                        card = new CardToxic
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Discard
                    },
                    new AAddCard
                    {
                        card = new CardToxic
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
