using Nickel;
using OneOf.Types;
using System.Collections.Generic;
using CountJest.Wizbo.Actions;
using System.Linq;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

/* Like other namespaces, this can be named whatever
 * However it's recommended that you follow the structure defined by ModEntry of <AuthorName>.<ModName> or <AuthorName>.<ModName>.Cards*/
namespace CountJest.Wizbo.Cards.UncommonCards;

/* The Card's class name IS IMPORTANT, however. This is what the game will ask for when trying to get a card.
 * If your card's class shares the same name as a vanilla card, or shares it with a modded card, the game can't keep both, and will only use one
 * For this reason, we recommend to give a unique name that is unlikely to be repeated by others, such as incorporating AuthorName or ModName to it */
internal sealed class CardKablooiePachinko : Card, IDemoCard
{
    /* For a bit more info on the Register Method, look at InternalInterfaces.cs and 1. CARDS section in ModEntry */
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Kablooie Pachinko", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                /* We don't assign cards to characters, but rather to decks! It's important to keep that in mind */
                deck = ModEntry.Instance.Wizbo_Deck.Deck,

                /* The vanilla rarities are Rarity.common, Rarity.uncommon, Rarity.rare */
                rarity = Rarity.uncommon,

                /* Some vanilla cards don't upgrade, some only upgrade to A, but most upgrade to either A or B */
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Kablooie Pachinko", "name"]).Localize
            /* AnyLocalizations.Bind().Localize will find the 'name' of 'Foxtale' in the locale file and feed it here. The output for english in-game from this is 'Fox Tale' */
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade switch
            {
                Upgrade.A => 2,
                Upgrade.B => 2,
                _ => 1
            },
            exhaust = false,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        int FSX = 0;
        if (s.route is Combat)
        {
            for (var item = 0; item < c.stuff.Values.Count; item++)
                FSX = item;
        }
            List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AFireStorm()
                    {
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 1)
                    },
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AFireStorm()
                    {
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = FSX,
                        iconName = "Fire Storm"
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, FSX),
                        xHint = 1,
                    },
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AFireStorm()
                    {
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = (FSX/2),
                        iconName = "Fire Storm"
                    },
                    new AStatus()
                    {
                        status = Status.boost,
                        statusAmount = (FSX/2),
                        targetPlayer = true,
                        xHint = 1,
                    },
                    new AStatus()
                    {
                        status = Status.boost,
                        statusAmount = (FSX/2),
                        targetPlayer = false,
                        xHint = 1,
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
