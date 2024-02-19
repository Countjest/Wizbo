using System;
using System.Collections.Generic;
using System.Linq;

namespace CountJest.Wizbo;
public class AFireStorm : CardAction
{
    private const bool AfsFlse = false;
    private int x;
    public int FSBonus;
    public override void Begin(G g, State s, Combat c)
    {
        base.Begin(g, s, c);

        new CardAction();
        foreach (var item in c.stuff.Values.ToList())
        {
            x = item.x;
            bool targetPlayer = AfsFlse;
            if (item != null)
            {               
                c.QueueImmediate(item.GetActionsOnDestroyed(s, c, targetPlayer, x));
                c.stuff.Remove(item.x);
                FSBonus++;
                s.AddShake(2.0);
                c.fx.Add(new DroneExplosion
                {
                    pos = new Vec(x * 16, 60.0) + new Vec(7.5, 4.0)
                }); 
            }
        }
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        if (s.route is Combat combat)
        {
            foreach (StuffBase value in combat.stuff.Values)
            {
                value.hilight = 2;
            }
        }

        return new List<Tooltip>
        {
            new TTGlossary("action.medusaField")
        };
    }
    public static Spr FireMinespr { get; }
}