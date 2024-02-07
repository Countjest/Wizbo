using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using static CountJest.Wizbo.Bolts;

namespace CountJest.Wizbo.Cards;

internal sealed class CardTregunaMekoides : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("TregunaMekoides", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
                            
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TregunaMekoides", "name"]).Localize
        });;
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None ? 1 : 2,

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
                    new ASpawn
                    {
                        thing = new Bolts
                        {
                        boltType = BType.magic,
                        targetPlayer = false
                        }
                    },

                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ASpawn
                    {
                        thing = new Bolts
                        {
                        boltType = BType.magic,
                        targetPlayer = false
                        }
                    },
                    new AMove()
                    {
                        dir = -2,
                        targetPlayer = false
                    },
                    new ASpawn
                    {
                        thing = new Bolts
                        {
                        boltType = BType.magic,
                        targetPlayer = false
                        }
                    },

                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ASpawn
                    {
                        thing = new Bolts
                        {
                        boltType = BType.magic,
                        targetPlayer = false
                        }
                    },
                    new AMove()
                    {
                        targetPlayer = false,
                        dir = 2
                    },
                    new ASpawn
                    {
                        thing = new Bolts
                        {
                        boltType = BType.magic,
                        targetPlayer = false
                        }
                    },

                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}

