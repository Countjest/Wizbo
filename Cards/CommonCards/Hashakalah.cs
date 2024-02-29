using CountJest.Wizbo.Actions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Net.Mail;
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
            cost = upgrade == Upgrade.B ? 2 : 1,
            exhaust = upgrade == Upgrade.B ? true : false,
            description = upgrade == Upgrade.B ? ModEntry.Instance.Localizations.Localize(["card", "Hashakalah", "descriptionB"]) : null,
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
                    new ASpawn()
                    {
                        timer = 0,
                        offset = -1,
                        thing = new Bolt()
                        {
                            boltType = BType.Fire,
                            targetPlayer = false
                        }
                    },
                    new ASpawn()
                    {
                        offset = 1,
                        thing = new Bolt()
                        {
                            boltType = BType.Fire,
                            targetPlayer = false
                        }
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
                    new AAddCard
                    {
                        card = new CardMiazbo
                        {
                            temporaryOverride= true
                        },
                        amount = 1,
                        destination = CardDestination.Hand,
                    },
                    new ASpawn()
                    {
                        timer = 0,
                        offset = -1,
                        thing = new Bolt()
                        {
                            boltType = BType.Fire,
                            targetPlayer = false
                        }
                    },
                    new ASpawn()
                    {
                        timer = 0,
                        offset = 1,
                        thing = new Bolt()
                        {
                            boltType = BType.Fire,
                            targetPlayer = false
                        }
                    },
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new BoltSwarm()
                    {
                    },
                    new ASpawn()
                    {
                        offset = (0),
                        thing = new Bolt()
                        {
                            boltType= BType.Fire,
                            targetPlayer = false,
                        },
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}