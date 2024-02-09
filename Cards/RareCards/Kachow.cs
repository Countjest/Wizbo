using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace CountJest.Wizbo.Cards;

internal sealed class CardKachow : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Kachow", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Kachow", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = upgrade == Upgrade.A ? false : true,

        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var jazStatus = s.ship.statusEffects.Where(pair =>
        pair.Key != Status.shield &&
        pair.Key != Status.tempShield)
        .ToDictionary(i => i.Key, i => i.Value).Values;
        var ejazStatus = c.otherShip.statusEffects.Where(pair =>
        pair.Key != Status.shield &&
        pair.Key != Status.tempShield)
        .ToDictionary(i => i.Key, i => i.Value).Values;
        int max = 0;
        int max2 = 0;
        if (jazStatus.Count > 0)
        {
            if (s.route is Combat)
            {
                max = jazStatus.Max();
                if (ejazStatus.Count > 0)
                {
                    max2 = ejazStatus.Max();
                }
            }
        };
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, max),
                        iconName = "Highest Status"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, max),
                        xHint = 1,
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, max),
                        iconName = "Highest Status"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, max),
                        xHint = 1 ,
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, max+max2),
                        iconName = "Sum Highest Status"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, max + max2),
                        xHint = 1 ,
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
