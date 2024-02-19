using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CountJest.Wizbo;
using FSPRO;
using Nickel;

namespace CountJest.Wizbo;
public class AFireField : CardAction
{
    public int FFBonus;
    public override void Begin(G g, State s, Combat c)
    {
        foreach (StuffBase item in c.stuff.Values.ToList())
        {
            FFBonus++;
            c.stuff.Remove(item.x);
            FireMine value = new FireMine
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
            new TTGlossary("action.FireField")
        };
    }
    public override Icon? GetIcon(State s)
    {
        Icon value = default(Icon);
        value.path = ModEntry.Instance.FFieldIcon.Sprite;
        return value;
    }
}