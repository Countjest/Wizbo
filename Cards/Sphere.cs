using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using FSPRO;
using FMOD;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Linq;

namespace CountJest.Wizbo;
[JsonConverter(typeof(StringEnumConverter))]
public enum SType
{
    Toxic,
    Chaos,
    Fire,
    Pholder
}
public class Sphere : StuffBase
{
    public static readonly int SprWidth = 17;
    public static readonly int SprHeight = 33;
    public static readonly int SprFrames = 8;
    public static readonly double SprFPS = 16;
    public string? skin;
    public class SphereData
    {
        public string? Key;
        public Color sphereColor;

        public Spr icon;

        public int baseDamage;

        public Status status;

        public int statusAmount;
    }

    public SType sphereType;
    public static readonly Dictionary<SType, SphereData> sphereData = new Dictionary<SType, SphereData>
    {
        {
            SType.Toxic,
            new SphereData
            {
                Key = "SToxic",
                sphereColor  = new Color("C3FFFB"),
                icon = ModEntry.Instance.TsphereIcon.Sprite,
                baseDamage = 0,
                status = ModEntry.Instance.OxidationStatus.Status,
                statusAmount = 0,
            }
        },
        {
            SType.Chaos,
            new SphereData
            {
                Key = "SChaos",
                sphereColor  = new Color("faffea"),
                icon = ModEntry.Instance.CsphereIcon.Sprite,
                baseDamage = 0
            }
        },
        {
            SType.Fire,
            new SphereData
            {
                Key = "SFire",
                sphereColor  = new Color("fff63f"),
                icon = ModEntry.Instance.FsphereIcon.Sprite,
                baseDamage = 1,
                status = Status.heat,
                statusAmount = 2,
            }
        },
        {
            SType.Pholder,
            new SphereData
            {
                Key = "SPholder",
                sphereColor  = new Color("ceb9ff"),
                icon = ModEntry.Instance.FsphereIcon.Sprite,
                baseDamage = 0
            }
        }
    };

    public override string GetDialogueTag()
    {
        return sphereData[sphereType].Key!;
    }

    public override double GetWiggleAmount()
    {
        return 2.0;
    }

    public override double GetWiggleRate()
    {
        return 3.0;
    }
    public override List<Tooltip> GetTooltips()
    {
        Spr sprite = sphereType switch
        {
            SType.Toxic => ModEntry.Instance.TsphereIcon.Sprite,
            SType.Chaos => ModEntry.Instance.CsphereIcon.Sprite,
            SType.Fire => ModEntry.Instance.FsphereIcon.Sprite,
            SType.Pholder => ModEntry.Instance.FsphereIcon.Sprite,
            _ => ModEntry.Instance.FsphereIcon.Sprite,
        };
        var result = new List<Tooltip>()
        {
            new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.midrow,
                () => (Spr)sprite,
                () => ModEntry.Instance.Localizations.Localize(["Midrow", "Sphere", $"{sphereType}", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["Midrow", "Sphere", $"{sphereType}", "description"]),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::Sphere{sphereType}"
            )
        };
        return result;
    }
    public virtual Spr GetSprite(State s, Combat c)
    {
        Spr SSprite = ModEntry.Instance.FBallspr.Sprite;
        switch (sphereType)
        {
            case SType.Chaos:
                SSprite = ModEntry.Instance.CBallspr.Sprite;
                break;
            case SType.Toxic:
                SSprite = ModEntry.Instance.TBallspr.Sprite;
                break;
            case SType.Fire:
                SSprite = ModEntry.Instance.FBallspr.Sprite;
                break;
            case SType.Pholder:
                SSprite = ModEntry.Instance.FBallspr.Sprite;
                break;
        }
        return SSprite;
    }
    public override Spr? GetIcon()
    {
        Spr Ssprite = sphereType switch
        {
            SType.Toxic => ModEntry.Instance.TsphereIcon.Sprite,
            SType.Chaos => ModEntry.Instance.CsphereIcon.Sprite,
            SType.Fire => ModEntry.Instance.FsphereIcon.Sprite,
            SType.Pholder => ModEntry.Instance.FsphereIcon.Sprite,
            _ => ModEntry.Instance.FsphereIcon.Sprite,
        };
        return Ssprite;
    }
    public override void Render(G g, Vec v)
    {
        int frame = (int)Math.Truncate((-this.x + g.state.time * SprFPS) % SprFrames);
        DrawWithHilight(g, GetSprite(g.state, (g.state.route as Combat)!), v, pixelRect: new Rect(frame * SprWidth, 0, SprWidth, SprHeight));
        //DrawWithHilight(g, ModEntry.Instance.FBallspr.Sprite, v + GetOffset(g), Mutil.Rand((double)x + 0.1) > 0.5, Mutil.Rand((double)x + 0.2) > 0.5);
    }
    public void DrawWithHilight(G g, Spr id, Vec v, bool flipX = false, bool flipY = false, Rect? pixelRect = null)
    {
        if (ShouldDrawHilight(g))
        {
            Texture2D? outlined = SpriteLoader.GetOutlined(id);
            double num = v.x - 2.0;
            double y = v.y - 2.0;
            BlendState screen = BlendMode.Screen;
            Color? color = sphereData[sphereType].sphereColor;
            Draw.Sprite(outlined, num, y, flipX, flipY, 0.0, null, null, null, pixelRect, color, screen);
        }
        Draw.Sprite(id, v.x - 1.0, v.y - 1.0, flipX, flipY, pixelRect: pixelRect);
    }
    public List<Status> Chaosstatuslist = new List<Status>()
    {
        Status.heat,
        Status.boost,
        Status.corrode,
        Status.backwardsMissiles,
        Status.boost,
    };
    public int sCounter;
    private const bool AfsFlse = false;
    public override List<CardAction> GetActions(State s, Combat c)
    {
        StuffBase item = this;
        sCounter++;
        List<CardAction> actions = new();
        {
            List<CardAction> cardActionList1 = new List<CardAction>()
            {

            };
            if (sCounter == 3)
            {
                x = item.x;
                bool targetPlayer = AfsFlse;
                c.QueueImmediate(item.GetActionsOnDestroyed(s, c, targetPlayer, x));
                c.stuff.Remove(item.x);
                s.AddShake(2.0);
                c.fx.Add(new DroneExplosion
                {
                    pos = new Vec(x * 16, 60.0) + new Vec(7.5, 4.0)
                });

            }
            actions = cardActionList1;

        }
        return actions;
    }
    public override List<CardAction> GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
    {
        bool Flag3 = s.time * 2.0 % 1.0 < 0.5;
        Status status = Chaosstatuslist[s.rngActions.NextInt() % Chaosstatuslist.Count];
        return sphereType switch
        {
            SType.Fire => new List<CardAction>
            {
                new ABallAttack
                {
                    hurtAmount = (1 + (sCounter / 2)),
                    targetPlayer = wasPlayer,
                    worldX = worldX,
                },
                new ABallStatus
                {
                    status = Status.heat,
                    statusAmount = (2 + (sCounter/2)),
                    targetPlayer = wasPlayer,
                    worldX = worldX,
                },
            },
            SType.Toxic => new List<CardAction>
            {
                new ABallAttack
                {
                    hurtAmount = 0,
                    targetPlayer = wasPlayer,
                    worldX = worldX
                },
                new ABallStatus
                {
                    status = ModEntry.Instance.OxidationStatus.Status,
                    statusAmount = (2 + sCounter),
                    targetPlayer = wasPlayer,
                    worldX = worldX,
                },

            },
            SType.Chaos => new List<CardAction>
            {
                new ABallAttack
                {
                    hurtAmount = 0,
                    targetPlayer = wasPlayer,
                    worldX = worldX
                },
                new ABallStatus
                {
                    status = status,
                    statusAmount = (1 + sCounter),
                    targetPlayer = wasPlayer,
                    worldX = worldX,
                },

            },
            SType.Pholder => new List<CardAction>
            {

            },
            _ => throw new NotImplementedException(),
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
    public class ABallAttack : CardAction
    {
        public int hurtAmount;

        public bool targetPlayer;

        public int worldX;

        public override void Begin(G g, State s, Combat c)
        {
            foreach (Artifact item in s.EnumerateAllArtifacts())
            {
                hurtAmount += item.ModifySpaceMineDamage(s, s.route as Combat, targetPlayer);
            }

            if (hurtAmount < 0)
            {
                hurtAmount = 0;
            }

            Ship ship = (targetPlayer ? s.ship : c.otherShip);
            RaycastResult raycastResult = CombatUtils.RaycastGlobal(c, ship, fromDrone: true, worldX);
            DamageDone? damageDone = null;
            if (raycastResult.hitShip)
            {
                damageDone = ship.NormalDamage(s, c, hurtAmount, worldX);
                c.QueueImmediate(new ABallStatus()
                {
                    targetPlayer = targetPlayer,
                    worldX = worldX,

                });
            }

            if (damageDone != null)
            {
                EffectSpawner.NonCannonHit(g, targetPlayer, raycastResult, damageDone);
                if (!raycastResult.hitShip)
                {
                    Audio.Play(Event.Hits_HitHurt);
                }
            }
        }
    }
    public class ABallStatus : CardAction
    {
        public Status status;
        public int statusAmount;
        public bool targetPlayer;
        public int worldX;
        public override void Begin(G g, State s, Combat c)
        {
            Ship ship = (targetPlayer ? s.ship : c.otherShip);
            RaycastResult raycastResult = CombatUtils.RaycastGlobal(c, ship, fromDrone: true, worldX);
            if (raycastResult.hitShip)
            {
                c.QueueImmediate(new AStatus()
                {
                    status = status,
                    statusAmount = statusAmount,
                    targetPlayer = targetPlayer,
                });
            }
        }
    }
}

