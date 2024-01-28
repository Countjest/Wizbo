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
                rarity = Rarity.common,
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
            description = ModEntry.Instance.Localizations.Localize(["card", "Kachow", "description", upgrade.ToString()])
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var max = s.ship.statusEffects.Values.Max();
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AAttack()
                    {
                    damage = GetDmg(s, max)
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AAttack()
                    {
                    damage = GetDmg(s, max)
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AAttack()
                    {
                    damage = GetDmg(s, max)
                    },
                    new AAttack()
                    {
                    damage = GetDmg(s, max)
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
