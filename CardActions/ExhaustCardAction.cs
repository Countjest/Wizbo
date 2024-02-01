﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo;
internal class ExhaustCardAction : CardAction // We made a custom card action that uses attributes and parameters from its parent class CardAction
{
    public int cardId; // We declare a number we will use as a card uuid, so any method that calls this card action will want to specify a card uuid, be it the card itself's, or another card's
    public override void Begin(G g, State s, Combat c) // We inherited Begin method from parent CardAction, we will overwrite it with our own method
    {
        var card = s.FindCard(cardId); // We want to make sure the uuid is valid
        if (card != null) // We only run the code below if it's not null, aka it's valid
        {
            card.ExhaustFX(); // This is a method for the exhaust sound effect and poof visual effect
            s.RemoveCardFromWhereverItIs(cardId);
            c.SendCardToExhaust(s, card);
        }
    }
}
internal class ExhaustCardAction2 : CardAction // We made a custom card action that uses attributes and parameters from its parent class CardAction
{
    public int cardId2; // We declare a number we will use as a card uuid, so any method that calls this card action will want to specify a card uuid, be it the card itself's, or another card's
    public override void Begin(G g, State s, Combat c) // We inherited Begin method from parent CardAction, we will overwrite it with our own method
    {
        var card2 = s.FindCard(cardId2); // We want to make sure the uuid is valid
        if (card2 != null) // We only run the code below if it's not null, aka it's valid
        {
            card2.ExhaustFX(); // This is a method for the exhaust sound effect and poof visual effect
            s.RemoveCardFromWhereverItIs(cardId2);
            c.SendCardToExhaust(s, card2);
        }
    }
}
internal class ParadoxCardAction : CardAction
{
    public int cardId3;
    public override void Begin(G g, State s, Combat c)
    {
        var card3 = s.FindCard(cardId3);
        if (card3 != null)
        {
            var CardCost = card3.GetCurrentCost(s);
            if (CardCost > 0)
            {
                Card card4 = card3.CopyWithNewId();
                Card card5 = card3.CopyWithNewId();
                card4.temporaryOverride = true;
                card5.temporaryOverride = true;
                c.QueueImmediate(new AAddCard
                {
                    card = card4,
                    destination = CardDestination.Deck,
                    amount = 1
                });
                c.QueueImmediate(new AAddCard
                {
                    card = card5,
                    destination = CardDestination.Discard,
                    amount = 1
                });
            }
        }
    }
}

