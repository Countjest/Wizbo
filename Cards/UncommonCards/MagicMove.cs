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
            cost = upgrade == Upgrade.B? 2 : 1,
            floppable = true,
            exhaust = false,
            art = flipped ? ModEntry.Instance.MMArtBot.Sprite : ModEntry.Instance.MMArtTop.Sprite,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        int FFX = 0;
        if (s.route is Combat)
        {
            for (var item = 0; item < c.stuff.Values.Count; item++)
                FFX = item;
        }
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        sphereType = SType.Fire,
                        disabled = flipped
                    },
                    new ADummyAction() {},
                    new AFireField()
                    {
                        sphereType = SType.Toxic,
                        disabled = !flipped
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        sphereType = SType.Fire,
                        disabled = flipped
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true,
                        disabled = flipped
                    },
                    new ADummyAction() {},
                    new AFireField()
                    {
                        sphereType = SType.Toxic,
                        disabled = !flipped
                    },
                    new AStatus()
                    {
                        status = Status.maxShield,
                        statusAmount = 1,
                        targetPlayer = true,
                        disabled = !flipped
                    },
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AFireField()
                    {
                        sphereType = SType.Chaos,
                        disabled = flipped
                    },
                    new AStatus()
                    {
                        status = Status.boost,
                        statusAmount = 2,
                        targetPlayer = true,
                        disabled = flipped
                    },
                    new AStatus()
                    {
                        status = Status.boost,
                        statusAmount = 2,
                        targetPlayer = true,
                        disabled = flipped
                    },
                    new ADummyAction() {},
                    new AFireField()
                    {
                        sphereType = SType.Chaos,
                        disabled = !flipped
                    },
                    new AHeal()
                    {
                        targetPlayer = true,
                        healAmount = 1,
                        disabled = !flipped
                    },
                    new AHeal()
                    {
                        targetPlayer = false,
                        healAmount = 1,
                        disabled = !flipped
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
