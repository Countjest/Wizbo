using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo;
internal class VanishCardAction : CardAction // We made a custom card action that uses attributes and parameters from its parent class CardAction
{
    public override void Begin(G g, State s, Combat c) // We inherited Begin method from parent CardAction, we will overwrite it with our own method
    {
        foreach (Part part in s.ship.parts)
        {
            if (part.type != PType.cockpit)
            {
                part.type = PType.empty;
                part.skin = "parts/empty.png";
            }
        }
    }
}
