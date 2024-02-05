
using FSPRO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace CountJest.Wizbo;

internal sealed class Bolt : StuffBase
{
    public class BoltMetadata
    {
        public string? key;

        public Spr icon;

        public int baseDamage;
        
        public Spr skin;

    }
    public BoltType? boltType;
    public string? skin;
    public static readonly Dictionary<BoltType, BoltMetadata> boltData = new Dictionary<BoltType, BoltMetadata>
        {
            {
                BoltType.Hex,
                new BoltMetadata
                {
                    key = "bolt_hex",
                    icon = ModEntry.Instance.HboltIcon.Sprite,
                    baseDamage = 1,
                    skin = ModEntry.Instance.Hbolt0.Sprite,
                }
            },
            {
                BoltType.Chaos,
                new BoltMetadata
                {
                    key = "bolt_chaos",
                    icon = ModEntry.Instance.CboltIcon.Sprite,
                    baseDamage = 1,
                    skin = ModEntry.Instance.Cbolt0.Sprite,
                }
            },
            {
                BoltType.Magic,
                new BoltMetadata
                {
                    key = "bolt_magic",
                    icon = ModEntry.Instance.MboltIcon.Sprite,
                    baseDamage = 1,
                    skin = ModEntry.Instance.Mbolt0.Sprite
                }
            },
    };

    public override double GetWiggleAmount()                                                                                                                                                                                                                                                                             
        => 1.5;
    public override double GetWiggleRate()
        => 3;
    public override void Render(G g, Vec v)
    {
        base.Render(g, v);
        Vec offset = GetOffset(g, doRound: true);
        Vec vec = new Vec(Math.Sin((double)x + g.state.time * 10.0), Math.Cos((double)x + g.state.time * 20.0 + Math.PI / 2.0)).round();
        offset += vec;
        int num = (boltType == BoltType.Magic && g.state.route is Combat c) ? GetMagicDirection(g.state, c) : 0;
        Vec vec2 = v + offset;
        bool flag = targetPlayer;
        bool flag2 = false;
        Spr spr;
        {
            bool flag3 = g.state.time * 2.0 % 1.0 < 0.5;
            flag2 = num > 0;
            spr = ((num != 0) ? (flag3 ? ModEntry.Instance.Mbolt1.Sprite : ModEntry.Instance.Mbolt3.Sprite) : (flag3 ? ModEntry.Instance.Mbolt1.Sprite : ModEntry.Instance.Mbolt3.Sprite));
        }
        Vec vec3 = default(Vec);
        if (num < 0)
        {
            vec3 += new Vec(-6.0, targetPlayer ? 4 : (-4));
        }

        if (num > 0)
        {
            vec3 += new Vec(6.0, targetPlayer ? 4 : (-4));
        }

        if (!targetPlayer)
        {
            vec3 += new Vec(0.0, 21.0);
        }

        Vec vec4 = vec2 + vec3 + new Vec(7.0, 8.0);
        bool flag4;
        if (skin == null)
        {
            double num2 = vec4.x - 5.0;
            double y = vec4.y + (double)((!targetPlayer) ? 14 : 0);
            Vec? originRel = new Vec(0.0, 1.0);
            flag4 = !targetPlayer;
            bool flipX = flag2;
            bool flipY = flag4;
        }

        Spr id2 = spr;
        flag4 = flag;
        DrawWithHilight(g, id2, vec2, flag2, flag4);
        if (skin == null)
        {
            Glow.Draw(vec4 + new Vec(0.5, -2.5), 25.0, new Color(1.0, 0.5, 0.5).gain(0.2 + 0.1 * Math.Sin(g.state.time * 30.0 + (double)x) * 0.5));
        }
    }
    public int GetMagicDirection(State s, Combat c)
    {
        int magicImpact = GetMagicImpact(s, c);
        if (x < magicImpact)
        {
            return -1;
        }

        if (x > magicImpact)
        {
            return 1;
        }

        return 0;
    }

    public override Vec GetOffset(G g, bool doRound = true)
    {
        Vec offset = base.GetOffset(g, doRound: true);
        if (boltType == BoltType.Magic)
        {
            double num = Math.Pow((yAnimation - 1.0) / 2.5, 3.0);
            if (g.state.route is Combat c)
            {
                double num2 = (double)((GetMagicImpact(g.state, c) - x) * 16) * num;
                offset.x += num2;
            }
        }
        else
        {

        }
        return offset;
    }
    public int GetMagicImpact(State s, Combat c)
    {
        int num = x;
        Ship ship = (targetPlayer ? s.ship : c.otherShip);
        int num2 = 99;
        int num3 = 0;
        for (int i = 0; i < ship.parts.Count; i++)
        {
            if (ship.parts[i].type != PType.empty)
            {
                num3 = i;
                if (i < num2)
                {
                    num2 = i;
                }
            }
        }

        if (x < ship.x + num2)
        {
            num = ship.x + num2;
        }

        if (x > ship.x + num3)
        {
            num = ship.x + num3;
        }

        if (x == num)
        {
            Part? partAtWorldX = ship.GetPartAtWorldX(num);
            if (partAtWorldX != null && partAtWorldX.type == PType.empty)
            {
                int num4 = num - ship.x;
                int num5 = 99;
                int num6 = 0;
                for (int j = 0; j < ship.parts.Count - 1; j++)
                {
                    if (ship.parts[j].type != PType.empty && Math.Abs(num4 - j) < num5)
                    {
                        num6 = j;
                        num5 = Math.Abs(num4 - j);
                    }
                }

                num = ship.x + num6;
            }
        }

        return num;
    }
    public List<Status> chaosstatuslist = new List<Status>()
           {
            Status.heat,
            Status.maxShield,
            Status.corrode,
            Status.backwardsMissiles,
            Status.boost,
           };
    public override List<CardAction>? GetActions(State s, Combat c)
    {
        Status status = chaosstatuslist[s.rngActions.NextInt() % chaosstatuslist.Count];
        return boltType switch
        {
            BoltType.Hex => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[BoltType.Hex].baseDamage,
                    targetPlayer = targetPlayer
                }
            },
            BoltType.Chaos => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[BoltType.Chaos].baseDamage,
                    targetPlayer = targetPlayer,
                    status = status,
                    statusAmount = 1
                }
            },
            BoltType.Magic => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[BoltType.Magic].baseDamage,
                    targetPlayer = targetPlayer
                }
            },
            _ => throw new NotImplementedException(),
        };
    }

    public class ABoltHit : CardAction
    {
        public bool targetPlayer;

        public int outgoingDamage;

        public int worldX;

        public Status? status;

        public int statusAmount;

        public bool weaken;

        public override bool CanSkipTimerIfLastEvent()
        {
            return false;
        }

        public override void Update(G g, State s, Combat c)
        {
            c.stuff.TryGetValue(worldX, out StuffBase? value);
            if (!(value is Bolt bolt))
            {
                timer -= g.dt;
                return;
            }

            Ship ship = (targetPlayer ? s.ship : c.otherShip);
            if (ship == null)
            {
                return;
            }

            RaycastResult raycastResult = CombatUtils.RaycastGlobal(c, ship, fromDrone: true, worldX);
            bool flag = false;
            if (raycastResult.hitShip)
            {
                Part? partAtWorldX = ship.GetPartAtWorldX(raycastResult.worldX);
                if (partAtWorldX == null || partAtWorldX.type != PType.empty)
                {
                    flag = true;
                }
            }

            if (bolt.boltType == BoltType.Magic)
            {
                flag = true;
                raycastResult.worldX = bolt.GetMagicImpact(s, c);
            }

            if (!bolt.isHitting)
            {
                Audio.Play(flag ? Event.Drones_MissileIncoming : Event.Drones_MissileMiss);
                bolt.isHitting = true;
            }

            if (!(bolt.yAnimation >= 3.5))
            {
                return;
            }

            if (flag)
            {
                int num = outgoingDamage;
                if (num < 0)
                {
                    num = 0;
                }

                DamageDone dmg = ship.NormalDamage(s, c, num, raycastResult.worldX);
                EffectSpawner.NonCannonHit(g, targetPlayer, raycastResult, dmg);
                Part? partAtWorldX2 = ship.GetPartAtWorldX(raycastResult.worldX);
                if (partAtWorldX2 != null && partAtWorldX2.stunModifier == PStunMod.stunnable)
                {
                    c.QueueImmediate(new AStunPart
                    {
                        worldX = raycastResult.worldX
                    });
                }

                if (status.HasValue && flag)
                {
                    c.QueueImmediate(new AStatus
                    {
                        status = status.Value,
                        statusAmount = statusAmount,
                        targetPlayer = targetPlayer
                    });
                }

                if (weaken && flag)
                {
                    c.QueueImmediate(new AWeaken
                    {
                        worldX = worldX,
                        targetPlayer = targetPlayer
                    });
                }

                if (ship.Get(Status.payback) > 0 || ship.Get(Status.tempPayback) > 0)
                {
                    c.QueueImmediate(new AAttack
                    {
                        damage = Card.GetActualDamage(s, ship.Get(Status.payback) + ship.Get(Status.tempPayback), !targetPlayer),
                        targetPlayer = !targetPlayer,
                        fast = true
                    });
                }
            }

            c.stuff.Remove(worldX);
            if (!(raycastResult.hitDrone || flag))
            {
                c.stuffOutro.Add(bolt);
            }
        }
    }
}