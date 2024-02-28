using FSPRO;
using System;

namespace CountJest.Wizbo
{
    public class ABoltHit : CardAction
    {
        public bool targetPlayer;
        public int outgoingDamage;
        public int worldX;
        public Status? status;
        public int statusAmount;
        public bool weaken;
        public int FDmg { get; private set; }

        public override bool CanSkipTimerIfLastEvent()
        {
            return false;
        }

        public override void Update(G g, State s, Combat c)
        {
            c.stuff.TryGetValue(worldX, out var value);
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

            if (bolt.boltType == BType.Magic || bolt.boltType == BType.Hex || bolt.boltType == BType.Chaos || bolt.boltType == BType.Fire)
            {
                flag = true;
                raycastResult.worldX = bolt.GetBoltImpact(s, c);
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
                foreach (Artifact item in s.EnumerateAllArtifacts())
                {
                    num += item.ModifyBaseMissileDamage(s, s.route as Combat, targetPlayer);
                }

                if (num < 0)
                {
                    num = 0;
                }


                FDmg = ship.Get(Status.heat) + ship.Get(Status.boost);

                if (bolt.boltType is BType.Fire)
                {
                    int Fnum = num + FDmg;
                    DamageDone dmg = ship.NormalDamage(s, c, Fnum, raycastResult.worldX);
                    EffectSpawner.NonCannonHit(g, targetPlayer, raycastResult, dmg);
                }
                else
                {
                    DamageDone dmg = ship.NormalDamage(s, c, num, raycastResult.worldX);
                    EffectSpawner.NonCannonHit(g, targetPlayer, raycastResult, dmg);
                };
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
                        worldX = raycastResult.worldX,
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