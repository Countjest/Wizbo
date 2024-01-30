using System.Collections.Generic;
using System.Linq;
using FSPRO;

public class AFireField : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach (StuffBase item in c.stuff.Values.ToList())
        {
            c.stuff.Remove(item.x);
            SpaceMine value = new SpaceMine
            {
                x = item.x,
                xLerped = item.xLerped,
                bubbleShield = item.bubbleShield,
                targetPlayer = item.targetPlayer,
                age = item.age
            };
            c.stuff[item.x] = value;
        }

        Audio.Play(Event.Status_PowerDown);
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

    public override Icon? GetIcon(State s)
    {
        return new Icon(Spr.icons_medusaField, null, Colors.textMain);
    }
}