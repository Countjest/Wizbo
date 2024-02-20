using Nickel;
using System.Reflection;
using System.Collections.Generic;
using CountJest.Wizbo.CardActions;
using System.Linq;


namespace CountJest.Wizbo.Cards;

internal sealed class CardMagicMove : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Magic Move", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {

                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Magic Move", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None? 1 : 2,
            floppable = true,
            exhaust = upgrade == Upgrade.None ? false : true,
            art = flipped ? ModEntry.Instance.MMArtBot.Sprite : ModEntry.Instance.MMArtTop.Sprite,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        int FFX = 0;
        int FSX = 0;
        if (s.route is Combat)
        {
            for (var item = 0; item < c.stuff.Values.Count; item++)
                FFX = item;
            for (var item2 = 0; item2 < c.stuff.Values.Count; item2++)
                FSX = item2;
        }
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        disabled = flipped
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 1,
                        targetPlayer = true,
                        disabled = flipped
                    },
                    new ADummyAction() 
                    {
                    },
                    new AFireStorm()
                    {
                        disabled = !flipped,
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        disabled = !flipped,
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        disabled = flipped,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = FFX,
                        iconName = "Fire Field",
                        disabled = flipped,
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = FFX,
                        targetPlayer = true,
                        disabled = flipped,
                        xHint = 1,
                    },
                    new ADummyAction()
                    {
                    },
                    new AFireStorm()
                    {
                        FSBonus = FSX,
                        disabled = !flipped,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = FSX,
                        iconName = "Fire Storm",
                        disabled = !flipped,
                    },
                    new AAttack()
                    {
                        damage = GetDmg( s,  FSX),
                        xHint = 1,
                        disabled = !flipped,
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        disabled = flipped,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = FFX,
                        iconName = "Fire Field",
                        disabled = flipped,
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = FFX,
                        targetPlayer = true,
                        disabled = flipped,
                        xHint = 1,
                    },
                    new ADummyAction()
                    {
                    },
                    new AFireStorm()
                    {
                        FSBonus = FSX,
                        disabled = !flipped,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = FSX,
                        iconName = "Fire Storm",
                        disabled = !flipped,
                    },
                    new AAttack()
                    {
                        damage = GetDmg( s,  0),
                        status = ModEntry.Instance.OxidationStatus.Status,
                        statusAmount = FSX,
                        xHint = 0,
                        disabled = !flipped,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
