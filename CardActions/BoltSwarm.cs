using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo;

public class BoltSwarm : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        if (c.isPlayerTurn == true)
        {
            int LR = s.ship.Get(Status.heat) + 4;
            int L = LR * (-1);
            int R = LR * (1);
            for (int i = LR; i > 0 ; i--)
            {
                L++;
                c.QueueImmediate(
                new ASpawn()
                {
                    timer = 0,
                    offset = L,
                    thing = new Bolt()
                    {
                        boltType = BType.Fire,
                        targetPlayer = false,
                    },
                    omitFromTooltips = true,
                });
                R--;
                c.QueueImmediate(
                new ASpawn()
                {
                    offset = R,
                    thing = new Bolt()
                    {
                        boltType = BType.Fire,
                        targetPlayer = false,
                    },
                    omitFromTooltips = true,
                });
            }
            if (LR == 0)
            {
                c.Queue(
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 0,
                        targetPlayer = true,
                        mode = AStatusMode.Set,
                    });
            }
        }
    }
}
