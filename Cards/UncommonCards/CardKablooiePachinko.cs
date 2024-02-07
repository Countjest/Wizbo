using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardKablooiePachinko : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("KablooiePachinko", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "KablooiePachinko", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None ? 1 : 2,
            description = ModEntry.Instance.Localizations.Localize(["card", "KablooiePachinko", "description", upgrade.ToString()])
            /* In a similar manner to how we localized card names, we'll localize their descriptions
             * For example, if Sheep Dream is upgraded to B, this description would try getting information from card > SheepDream > Description > B in the locale file */
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        int hbonusB = 0;
        int bonush = 0;
        if (c.isPlayerTurn == true && s.route is Combat)
            if (c.otherShip.statusEffects.Values.Count > 0)
                bonush = c.otherShip.Get(Status.heat);
            else
            {
            }

        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, bonush)
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 3+bonush)
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AFireField()
                    {

                    },
                    new AFireStorm()
                    {
                        HBonus = hbonusB

                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, bonush + hbonusB)
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}

