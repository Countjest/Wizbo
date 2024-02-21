using CountJest.Wizbo.Artifacts;
using System;
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
internal class DunamisCardAction : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        var artifact2 = s.EnumerateAllArtifacts().OfType<DunamisGrimoire>().FirstOrDefault();

        if (artifact2 != null)
        {
            artifact2.DunamisCounter++;
            if (artifact2.DunamisCounter == 10)
            {
                c.QueueImmediate(new AStatus()
                {
                    status = Status.powerdrive,
                    statusAmount = 1,
                    targetPlayer = true,
                });
                artifact2.DunamisCounter = 0;
            }
        }
    }
}
internal class ParadoxCardAction : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        var artifact = s.EnumerateAllArtifacts().OfType<ParadoxGrimoire>().FirstOrDefault();

        if (artifact != null)
        {
            artifact.ParadoxCounter++;
            if (artifact.ParadoxCounter == 8)
            {
                c.QueueImmediate(new ADrawCard()
                {
                    count = 1
                });
                c.QueueImmediate(new AEnergy()
                {
                    changeAmount = 1
                });
                artifact.ParadoxCounter = 0;
            }
        }
    }
}

