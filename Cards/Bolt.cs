﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using FSPRO;
using static StoryVars;
using FMOD;

namespace CountJest.Wizbo;

public class Bolt : StuffBase
{
    public static readonly int SprWidth = 17;
    public static readonly int SprHeight = 33;
    public static readonly int SprFrames = 8;
    public static readonly double SprFPS = 12;
    public string? skin;
    private class BoltData
    {
        public string? Key;
        public Color boltColor;

        public Spr icon;

        public int baseDamage;
    }

    public BType boltType;
    public enum BType
    {
        witch = 0,
        chaos = 1,
        magic = 2,
        hex = 3,
    }
    private static readonly Dictionary<BType, BoltData> boltData = new Dictionary<BType, BoltData>
    {
        {
            BType.witch,
            new BoltData
            {
                Key = "Witch",
                boltColor  = new Color("C3FFFB"),
                icon = ModEntry.Instance.WboltIcon.Sprite,
                baseDamage = 0
            }
        },
        {
            BType.chaos,
            new BoltData
            {
                Key = "Chaos",
                boltColor  = new Color("faffea"),
                icon = ModEntry.Instance.CboltIcon.Sprite,
                baseDamage = 0
            }
        },
        {
            BType.magic,
            new BoltData
            {
                Key = "Magic",
                boltColor  = new Color("ffffff"),
                icon = ModEntry.Instance.MboltIcon.Sprite,
                baseDamage = 1
            }
        },
        {
            BType.hex,
            new BoltData
            {
                Key = "Hex",
                boltColor  = new Color("ceb9ff"),
                icon = ModEntry.Instance.HboltIcon.Sprite,
                baseDamage = 0
            }
        }
    };
    public override bool IsHostile()
    {
        return targetPlayer;
    }
    public override string GetDialogueTag()
    {
        return boltData[boltType].Key!;
    }

    public override bool GetIsDoneAnimatingAsExtraStuff()
    {
        return yAnimation > 14.0;
    }

    public override List<Tooltip> GetTooltips()
    {
        var result = new List<Tooltip>();

        switch (boltType)
        {
            case BType.hex:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["Midrow", "Bolt", "Hex", "name", "description"])));
                break;
            case BType.witch:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["Midrow", "Bolt", "Witch", "name", "description"])));
                break;
            case BType.chaos:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["Midrow", "Bolt", "Chaos", "name", "description"])));
                break;
            case BType.magic:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["Midrow", "Bolt", "Magic", "name", "description"])));
                break;
            default:
                throw new NotImplementedException($"Unkown magic Bolt type {boltType}");
        }
        return result;
    }
    public List<Status> chaosstatuslist = new List<Status>()
    {
        Status.heat,
        Status.boost,
        Status.corrode,
        Status.backwardsMissiles,
        Status.boost,
    };
    public virtual Spr GetSprite()
    {
        return (Spr)ModEntry.Instance.Bolt.Sprite;
    }
    public override void Render(G g, Vec v)
    {
        Vec offset = GetOffset(g, doRound: true);
        Vec vec = new Vec(Math.Sin((double)x + g.state.time * 10.0), Math.Cos((double)x + g.state.time * 20.0 + Math.PI / 2.0)).round();
        offset += vec;
        int num = ((boltType == BType.magic && g.state.route is Combat c) ? GetBoltDirection(g.state, c) : 0);
        Vec vec2 = v + offset;
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
        int frame = (int)Math.Truncate((-this.x + g.state.time * SprFPS) % SprFrames);
        DrawWithHilight(g, GetSprite(), v, pixelRect: new Rect(frame * SprWidth, 0, SprWidth, SprHeight));
        // this is for the missile's exhaust (this render function is based on missile's render function)
        //Glow.Draw(vec4 + new Vec(0.5, -2.5), 25.0, boltColor * new Color(1.0, 0.5, 0.5).gain(0.2 + 0.1 * Math.Sin(g.state.time * 30.0 + (double)x) * 0.5));
        bool flag = targetPlayer;
        bool flag2 = false;
        bool flag3 = g.state.time * 2.0 % 1.0 < 0.5;
            flag2 = num > 0;
        bool flag4;
        flag4 = flag;
    }

    public override Vec GetOffset(G g, bool doRound = true)
    {
        Vec offset = base.GetOffset(g, doRound: true);
        if (boltType == BType.magic)
        {
            double num = Math.Pow((yAnimation - 1.0) / 2.5, 3.0);
            if (g.state.route is Combat c)
            {
                double num2 = (double)((GetBoltImpact(g.state, c) - x) * 16) * num;
                offset.x += num2;
            }
        }

        return offset;
    }
    // stolen right from StuffBase, but I added the pixelRect argument to support spritesheet animations
    public void DrawWithHilight(G g, Spr id, Vec v, bool flipX = false, bool flipY = false, Rect? pixelRect = null)
    {
        if (ShouldDrawHilight(g))
        {
            Texture2D? outlined = SpriteLoader.GetOutlined(id);
            double num = v.x - 2.0;
            double y = v.y - 2.0;
            BlendState screen = BlendMode.Screen;
            Color? color = boltData[boltType].boltColor;
            Draw.Sprite(outlined, num, y, flipX, flipY, 0.0, null, null, null, pixelRect, color, screen);
        }
        Draw.Sprite(id, v.x - 1.0, v.y - 1.0, flipX, flipY, pixelRect: pixelRect);
    }
    private int GetBoltDirection(State s, Combat c)
    {
        int boltImpact = GetBoltImpact(s, c);
        if (x < boltImpact)
        {
            return -1;
        }

        if (x > boltImpact)
        {
            return 1;
        }

        return 0;
    }
    public int GetBoltImpact(State s, Combat c)
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

    public int Num;
    public bool Flag3;
    public bool Flag2;
    public Spr spr;
    public override List<CardAction>? GetActions(State s, Combat c)
    {
        bool Flag3 = s.time * 2.0 % 1.0 < 0.5;
        int WDmg = 0;
        if (s.route is Combat)
            for (var partIndex = 0; partIndex < c.otherShip.parts.Count; partIndex++)
                WDmg = partIndex;
        Status status = chaosstatuslist[s.rngActions.NextInt() % chaosstatuslist.Count];
        return boltType switch
        {
            BType.witch => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[boltType].baseDamage + WDmg,
                    targetPlayer = targetPlayer,
                    status = Status.boost,
                    statusAmount = 1,

                }
            },
            BType.hex => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[boltType].baseDamage,
                    targetPlayer = targetPlayer,
                    status = Status.backwardsMissiles,
                    statusAmount = 2,
                    weaken = true,

                }
            },
            BType.chaos => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[boltType].baseDamage,
                    targetPlayer = targetPlayer,
                    status = status,
                    statusAmount = 1,

                }
            },
            BType.magic => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = boltData[boltType].baseDamage,
                    targetPlayer = targetPlayer,
                }
            },
            _ => throw new NotImplementedException(),
        };
    }
    public override Spr? GetIcon()
    {
        switch (boltType)
        {
            case BType.hex:
                return (ModEntry.Instance.HboltIcon.Sprite);
            case BType.witch:
                return (ModEntry.Instance.WboltIcon.Sprite);
            case BType.chaos:
                return (ModEntry.Instance.CboltIcon.Sprite);
            case BType.magic:
                return (ModEntry.Instance.MboltIcon.Sprite);  
            default:
                return null;
        }
    }

    public override List<CardAction>? GetActionsOnShotWhileInvincible(State s, Combat c, bool wasPlayer, int damage)
    {
        return new List<CardAction>
        {
            new ADroneFlipSingle
            {
                thing = this,
                dialogueSelector = ".flippedMissile"
            }
        };
    }
}