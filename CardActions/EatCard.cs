using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo.CardActions
{
    public class EatCard : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            Card? card = selectedCard;
            if (card != null)
            {
                int cardCost = card.GetCurrentCost(s);
                c.QueueImmediate(
                    new ExhaustCardAction()
                    {
                        cardId = card.uuid,
                    });
                c.QueueImmediate(
                    new AEnergy()
                    {
                        changeAmount = cardCost,

                    });
            }
        }
    }
}
