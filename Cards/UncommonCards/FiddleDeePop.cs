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
internal sealed class CardFiddleDeeDoop : Card, IDemoCard
{
    /* For a bit more info on the Register Method, look at InternalInterfaces.cs and 1. CARDS section in ModEntry */
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("FiddleDeeDoop", new()
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FiddleDeeDoop", "name"]).Localize
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
            exhaust = true,
            /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
            /* if we don't set a card specific 'art' here, the game will give it the deck's 'DefaultCardArt' */
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();
        Card? card = null;
        int cardCost = 0;
        if (card != null && s.route is Combat)
        {
            cardCost = card.GetCurrentCost(s);
        }
        switch (upgrade)
        { 
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new ADelay
                    {
                        time = -0.5
                    },
                    new ACardSelect
                    {
                        browseAction = new AExhaustOtherCard()
                        {
                            selectedCard = card,
                        },
                        browseSource = CardBrowse.Source.Hand
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = cardCost,
                        iconName = "Card Cost"
                    },
                    new AEnergy()
                    {
                        changeAmount = cardCost,
                        xHint = 1,
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ADelay
                    {
                        time = -0.5
                    },
                    new ACardSelect
                    {
                        browseAction = new AExhaustOtherCard()
                        {
                            selectedCard = card,
                        },
                        browseSource = CardBrowse.Source.Hand
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = cardCost,
                        iconName = "Card Cost"
                    },
                    new AEnergy()
                    {
                        changeAmount = cardCost,
                        xHint = 1,
                    },
                    new AStatus()
                    {
                        status = Status.tempShield, 
                        statusAmount = cardCost,
                        xHint = 2,
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ADelay
                    {
                        time = -0.5
                    },
                    new ACardSelect
                    {
                        browseAction = new AExhaustOtherCard()
                        {
                            selectedCard = card,
                        },
                        browseSource = CardBrowse.Source.DiscardPile
                    },
                    new AVariableHintFake()
                    {
                        displayAmount = cardCost,
                        iconName = "Card Cost"
                    },
                    new AEnergy()
                    {
                        changeAmount = cardCost,
                        xHint = 1,
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
