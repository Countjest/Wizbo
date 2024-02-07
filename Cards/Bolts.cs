using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using FSPRO;

namespace CountJest.Wizbo;
public class Bolts : StuffBase
{
    public BType boltType = BType.hex;
    private const int resolution = 10000;
    public enum BType
    {
        hex = 0,
        chaos = 1,
        magic = 2,
    }
    public Spr? GetActionIcon()
    {
        switch (boltType)
        {
            case BType.hex:
                return (Spr)((Num != 0) ? (Flag3 ? ModEntry.Instance.Hbolt1.Sprite : ModEntry.Instance.Hbolt3.Sprite) : (Flag3 ? ModEntry.Instance.Hbolt1.Sprite : ModEntry.Instance.Hbolt3.Sprite));

            case BType.chaos:
                return (Spr)((Num != 0) ? (Flag3 ? ModEntry.Instance.Cbolt1.Sprite : ModEntry.Instance.Cbolt3.Sprite) : (Flag3 ? ModEntry.Instance.Cbolt1.Sprite : ModEntry.Instance.Cbolt3.Sprite));

            case BType.magic:
                return (Spr)((Num != 0) ? (Flag3 ? ModEntry.Instance.Mbolt1.Sprite : ModEntry.Instance.Mbolt3.Sprite) : (Flag3 ? ModEntry.Instance.Mbolt1.Sprite : ModEntry.Instance.Mbolt3.Sprite));

            default:
                return null;
        }
    }
    public override List<Tooltip> GetTooltips()
    {
        var result = new List<Tooltip>();

        switch (boltType)
        {
            case BType.hex:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["midrow", "Bolts", "Hex", "description"])));
                break;
            case BType.chaos:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["midrow", "Bolts", "Chaos", "description"])));
                break;
            case BType.magic:
                result.Add(new TTText(ModEntry.Instance.Localizations.Localize(["midrow", "Bolts", "Magic", "description"])));
                break;
            default:
                throw new NotImplementedException($"Unkown magic bolt type {boltType}");
        }
        return result;
    }
    public List<Status> chaosstatuslist = new List<Status>()
    {
        Status.heat,
        Status.maxShield,
        Status.corrode,
        Status.backwardsMissiles,
        Status.boost,
    };
    public int Num;
    public bool Flag3;
    public bool Flag2;
    public Spr spr;
    public override List<CardAction>? GetActions(State s, Combat c)
    {
        bool Flag3 = s.time * 2.0 % 1.0 < 0.5;
        int HDmg = 0; 
        if (s.route is Combat && c.otherShip.statusEffects.Values.Count > 0)
            HDmg = c.otherShip.Get(Status.heat);
        Status status = chaosstatuslist[s.rngActions.NextInt() % chaosstatuslist.Count];
        return boltType switch
        {
            BType.hex => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = RawDamage() + HDmg,
                    targetPlayer = targetPlayer,
                    status = Status.heat,
                    statusAmount = 2,

                }
            },
            BType.chaos => new List<CardAction>
            {
                new ABoltHit
                {
                    worldX = x,
                    outgoingDamage = RawDamage(),
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
                    outgoingDamage = RawDamage(),
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

            case BType.chaos:
                return (ModEntry.Instance.CboltIcon.Sprite);

            case BType.magic:
                return (ModEntry.Instance.MboltIcon.Sprite);
            default:
                return null;
        }
    }
    public int RawDamage()
    {
        if (boltType == BType.hex)
            return 0;
        return 1;
    }
}        