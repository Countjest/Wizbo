using Nickel;
using System.Reflection;
using System.Collections.Generic;
using CountJest.Wizbo.CardActions;


namespace CountJest.Wizbo.Cards;

internal sealed class CardMagicMove : Card, IDemoCard
{
    private int ffX;
    private int fsX;

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
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AFireField()
                    {
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
                        damage = GetDmg(s, 2)
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        FFBonus = ffX
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = ffX,
                        iconName = "Fire Field",
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = ffX,
                        targetPlayer = true,
                        disabled = flipped,
                        xHint = 1,
                    },
                    new ADummyAction()
                    {
                    },
                    new AFireStorm()
                    {
                        FSBonus = fsX,
                        disabled = !flipped,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = fsX,
                        iconName = "Fire Storm",
                    },
                    new AAttack()
                    {
                        damage = GetDmg( s,  fsX),
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
                        FFBonus = ffX,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = ffX,
                        iconName = "Fire Field",
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = ffX,
                        targetPlayer = true,
                        disabled = flipped,
                        xHint = 1,
                    },
                    new ADummyAction()
                    {
                    },
                    new AFireStorm()
                    {
                        FSBonus = fsX,
                        disabled = !flipped,
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = fsX,
                        iconName = "Fire Storm",
                    },
                    new AAttack()
                    {
                        damage = GetDmg( s,  fsX),
                        xHint = 1,
                        disabled = !flipped,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
