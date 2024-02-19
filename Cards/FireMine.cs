using Nickel;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CountJest.Wizbo;

public class FireMine : StuffBase
{
    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> list = new List<Tooltip>();
        list.Add(new TTGlossary(MKGlossary("FireMine"), "1 damage 2 heat"));
        List<Tooltip> list2 = list;
        if (bubbleShield)
        {
            list2.Add(new TTGlossary("midrow.bubbleShield"));
        }

        return list2;
    }

    public override Spr? GetIcon()
    {
        return ModEntry.Instance.FmineIcon.Sprite;
    }

    public override string GetDialogueTag()
    {
        return "FireMine";
    }

    public override double GetWiggleAmount()
    {
        return 2.0;
    }

    public override double GetWiggleRate()
    {
        return 4.0;
    }

    public override void Render(G g, Vec v)
    {
        DrawWithHilight(g, ModEntry.Instance.FireMinespr.Sprite, v + GetOffset(g), Mutil.Rand((double)x + 0.1) > 0.5, Mutil.Rand((double)x + 0.2) > 0.5);
    }

    public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
    {
        return new List<CardAction>
        {
            new AFireMineAttack
            {
                hurtAmount = 1,
                targetPlayer = wasPlayer,
                worldX = worldX
            },
            new AStatus
            {
                status = Status.heat,
                statusAmount = 2,
                targetPlayer = wasPlayer,

            }
        };
    }
    public override void DoDestroyedEffect(State s, Combat c)
    {
        s.AddShake(2.0);
        c.fx.Add(new DroneExplosion
        {
            pos = new Vec(x * 16, 60.0) + new Vec(7.5, 4.0)
        });
    }
    private class AFireMineAttack : CardAction
    {
        public int hurtAmount { get; set; }
        public bool targetPlayer { get; set; }
        public int worldX { get; set; }
    }
}

