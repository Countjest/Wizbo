using Nickel;
using System.Collections.Generic;
using System.Reflection;
using CountJest.Wizbo;
using CountJest.Wizbo.Actions;
using CountJest.Wizbo.Cards;

namespace CountJest.Wizbo;

internal sealed class CardBidiBodiBoo : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Bidi Bodi Boo", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Bidi Bodi Boo", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None ? 1 : 2,
            exhaust = true,
            singleUse = upgrade == Upgrade.A ? true : false,
            description = ModEntry.Instance.Localizations.Localize(["card", "Bidi Bodi Boo", "description", upgrade.ToString()])
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
                    new ACardSelect
                    {
                        browseAction = new CloudSavePickCard(),
                        browseSource = CardBrowse.Source.ExhaustPile,
                    }

                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ACardSelect()
                    {
                        browseAction = new ChooseCardToPutInHand(),
                        browseSource = CardBrowse.Source.Codex,
                    },
                    new ACardSelect()
                    {
                        browseAction = new ChooseCardToPutInHand(),
                        browseSource = CardBrowse.Source.Codex,
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new UnExhstCards()
                    {
                        CardID = uuid,
                    },
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}

public class UnExhstCards : CardAction
{
    public int CardID;
    public override void Begin(G g, State s, Combat c)
    {
        if (c.exhausted.Count > 0)
        {
            foreach (Card ECard in c.exhausted)
            {
                if (ECard != null && ECard.uuid != CardID)
                {
                    c.QueueImmediate(
                        new SpitCard()
                        {
                            Ecard = ECard,
                        });
                }
            }
        }
    }
}
public class SpitCard : CardAction
{
    public Card? Ecard;
    public Card? Ecard2;
    public override void Begin(G g, State s, Combat c)
    {
        if (Ecard != null)
            Ecard2 = Ecard.CopyWithNewId();
            Ecard2!.temporaryOverride = true;
            c.SendCardToDiscard(s, Ecard2);
    }
}
public class SpitCard2 : CardAction
{
    public Card? Ecard;
    public Card? Ecard2;
    public override void Begin(G g, State s, Combat c)
    {
        if (Ecard != null)
        {
            Ecard2 = Ecard.CopyWithNewId();
            Ecard2!.temporaryOverride = true;
            c.QueueImmediate(
            new AAddCard()
            {
                card = Ecard2,
                amount =1,
                destination = CardDestination.Deck,
            });
        }
    }
}
