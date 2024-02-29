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
            int LR = s.ship.Get(Status.heat) + 2;
            int L = (LR * (-1))/2;
            int R = (LR * (1))/2;
            for (int i = LR; i > 0 ; i-=2)
            {
                L++;
                if (L < 0)
                {
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
                }
                R--;
                if (R > 0)
                {
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
            }
            c.QueueImmediate(
                new AStatus()
                {
                    status = Status.heat,
                    statusAmount = 0,
                    targetPlayer = true,
                    mode = AStatusMode.Set
                });
        }
    }
}
