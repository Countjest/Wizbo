using clay.PhilipTheMechanic.Actions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardGreaterLesserbeam : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Greater Lesserbeam", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Greater Lesserbeam", "name"]).Localize
        });
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
        int boostMod = 0;
        int boostMod2 = 0;
        int bonush = 0;
        int bonush2 = 0;
        if (c.isPlayerTurn == true && s.route is Combat)
        {
            if (c.otherShip.statusEffects.Values.Count > 0)
            {
                bonush = c.otherShip.Get(Status.heat);
                boostMod = c.otherShip.Get(Status.boost);
            }
            if (s.ship.statusEffects.Values.Count > 0)
            {
                bonush2 = s.ship.Get(Status.heat);
                boostMod2 = s.ship.Get(Status.boost);
            }
        }
        else
        {
        }
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, bonush),
                        iconName = "Enemy Heat",

                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, bonush),
                        xHint = 1,
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
                        statusAmount = 3,
                        targetPlayer = false,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = GetDmg(s, 3+boostMod+bonush),
                        iconName = "Enemy Heat",
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 3+boostMod+bonush),
                        xHint = 1,
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ATooltipDummy()
                    {
                        renderVarAssignment = true,
                        icons = new()
                        {
                            new Icon(Enum.Parse<Spr>("icons_heat"), null, Colors.textMain),
                            new Icon(Enum.Parse<Spr>("icons_plus"), null, Colors.textMain),
                            new Icon(Enum.Parse<Spr>("icons_heat"), null, Colors.textMain),
                            new Icon(Enum.Parse<Spr>("icons_outgoing"), null, Colors.textMain),
                        }
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, bonush + bonush2 + boostMod2 + boostMod),
                        xHint = 1,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}

