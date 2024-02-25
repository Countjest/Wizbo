using Nickel;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace CountJest.Wizbo.Cards;
internal sealed class CardSquaDaLa : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Squa Da La", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {

                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Squa Da La", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 1,
            flippable = true,
            exhaust = false,
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
                        thing = new Sphere()
                        {
                            sphereType = SType.Fire,
                        }
                    },
                    new AMove()
                    {
                        dir = 1,
                        isTeleport = true,
                        targetPlayer = true,
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ASpawn()
                    {
                        thing = new Sphere()
                        {
                            sphereType = SType.Fire,
                        }
                    },
                    new AMove()
                    {
                        dir = 2,
                        isTeleport = true,
                        targetPlayer = true,
                    },
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ASpawn()
                    {
                        thing = new Sphere()
                        {
                            sphereType = SType.Fire,
                        }
                    },
                    new AMove()
                    {
                        dir = 1,
                        isTeleport = true,
                        targetPlayer = true,
                    },
                    new ASpawn()
                    {
                        thing = new Sphere()
                        {
                            sphereType = SType.Fire,
                        }
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
