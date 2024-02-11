using CountJest.Tower.Cards;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using static CountJest.Wizbo.Bolt;

namespace CountJest.Wizbo.Cards;

internal sealed class CardShazammy : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Shazammy", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Shazammy", "name"]).Localize
        });

    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None ? 0 : 1,
            exhaust = upgrade == Upgrade.B ? true : false,
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
                        status = Status.tempShield,
                        statusAmount = 1,
                        targetPlayer = true,
                    },

                    new ASpawn()
                    {
                        thing = new Bolt()
                        {
                            boltType = BType.Magic,
                            targetPlayer = false
                        }
                    }

                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer= true,
                    },

                    new ASpawn()
                    {
                        thing = new Bolt()
                        {
                            boltType = BType.Magic,
                            targetPlayer = false
                        }
                    },
                    new ASpawn()
                    {
                        offset = 1,
                        thing = new Bolt()
                        {
                            boltType = BType.Magic,
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
                        offset = -1,
                        thing = new Bolt()
                        {
                            boltType = BType.Magic,
                            targetPlayer = false
                        }
                    },
                    new ASpawn()
                    {
                        offset = 1,
                        thing = new Bolt()
                        {
                            boltType = BType.Magic,
                            targetPlayer = false
                        }
                    },
                    new AAddCard()
                    {
                        card = new CardShazammy()
                            {
                                upgrade = Upgrade.B,
                                temporaryOverride= true
                            },
                        amount = 1,
                        destination = CardDestination.Deck
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
