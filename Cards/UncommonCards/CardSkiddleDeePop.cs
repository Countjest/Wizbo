using Nickel;
using OneOf.Types;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

/* Like other namespaces, this can be named whatever
 * However it's recommended that you follow the structure defined by ModEntry of <AuthorName>.<ModName> or <AuthorName>.<ModName>.Cards*/
namespace CountJest.Wizbo.Cards.UncommonCards;

/* The Card's class name IS IMPORTANT, however. This is what the game will ask for when trying to get a card.
 * If your card's class shares the same name as a vanilla card, or shares it with a modded card, the game can't keep both, and will only use one
 * For this reason, we recommend to give a unique name that is unlikely to be repeated by others, such as incorporating AuthorName or ModName to it */
internal sealed class CardSkiddleDeePop : Card, IDemoCard
{
    /* For a bit more info on the Register Method, look at InternalInterfaces.cs and 1. CARDS section in ModEntry */
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Skiddle Dee Pop", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                /* We don't assign cards to characters, but rather to decks! It's important to keep that in mind */
                deck = ModEntry.Instance.Wizbo_Deck.Deck,

                /* The vanilla rarities are Rarity.common, Rarity.uncommon, Rarity.rare */
                rarity = Rarity.uncommon,

                
                upgradesTo = [Upgrade.A, Upgrade.B],
                
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Skiddle Dee Pop", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade switch
            {
                Upgrade.A => 1,
                Upgrade.B => 1,
                _ => 1
            },
            exhaust = upgrade == Upgrade.B ? true : false,
            /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
            /* if we don't set a card specific 'art' here, the game will give it the deck's 'DefaultCardArt' */
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        int boostMod = 0;
        int Cnum = 0;
        if (s.route is Combat)
            boostMod = s.ship.Get(Status.boost);
        if (s.ship.statusEffects.Values.Count > 0)
        {
            if (s.route is Combat && upgrade != Upgrade.B)
                Cnum = s.ship.Get(Status.heat);
            if (s.route is Combat && upgrade == Upgrade.B)
                Cnum = s.ship.statusEffects.Where(pair =>
                    pair.Key != Status.shield &&
                    pair.Key != Status.tempShield)
                    .ToDictionary(i => i.Key, i => i.Value).Values.Max();
        }


        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new AVariableHint()
                    {
                        status = Status.heat,
                    },
                    new ADrawCard()
                    {
                        count = (2+boostMod) + Cnum,
                        xHint = 1,
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 2,
                        targetPlayer = true,
                    },
                    new AVariableHint()
                    {
                        status = Status.heat,
                    },
                    new ADrawCard()
                    {
                        count = (2+boostMod) + Cnum,
                        xHint = 1,
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = -1,
                        targetPlayer = true,
                    },

                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AVariableHintFake()
                    {
                        displayAmount = Cnum,
                        iconName = "Highest Status"
                    },
                    new ADrawCard()
                    {
                        count = Cnum,
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
