using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using static CountJest.Wizbo.Bolt;

namespace CountJest.Wizbo.Cards;

internal sealed class CardDeeMekoides : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Dee Mekoides", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
                            
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Dee Mekoides", "name"]).Localize
        });;
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None ? 1 : 2,
            exhaust = upgrade == Upgrade.None ? false : true,

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
                        thing = new Bolt()
                        {
                            boltType = BType.Chaos,
                            targetPlayer = false
                        }
                    },

                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ASpawn()
                    {
                        offset = 1,
                        thing = new Bolt()
                        {
                            boltType = BType.Chaos,
                            targetPlayer = false
                        }
                    },
                    new ASpawn()
                    {
                        offset = 0,
                        thing = new Sphere()
                        {
                            sphereType = SType.Chaos,
                        }
                    },
                    new ASpawn()
                    {
                        offset = -1,
                        thing = new Bolt()
                        {
                            boltType = BType.Chaos,
                            targetPlayer = false
                        }
                    },
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ASpawn()
                    {
                        offset = 1,
                        thing = new Sphere()
                        {
                            sphereType = SType.Chaos,
                        }
                    },
                    new ASpawn()
                    {
                        offset = 0,
                        thing = new Bolt()
                        {
                            boltType = BType.Chaos,
                            targetPlayer = false
                        }
                    },
                    new ASpawn()
                    {
                        offset = -1,
                        thing = new Sphere()
                        {
                            sphereType = SType.Chaos,
                        }
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}

