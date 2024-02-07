using System;
using System.Collections.Generic;

namespace CountJest.Wizbo;

public class ABoltHit : CardAction
{
    public bool targetPlayer;

    public int outgoingDamage;

    public int worldX;

    public Status? status;

    public int statusAmount;

    public bool done = false;
    public bool will_hit = false;
    public int world_pos_x;

    private Bolts? bolt;
    private FlightFX? missile_fx;
    private int target_pos;

    public override bool CanSkipTimerIfLastEvent()
    {
        return false;
    }

    public override void Begin(G g, State s, Combat c)
    {
        if (!c.stuff.TryGetValue(world_pos_x, out var stuff) || stuff is not Bolts msl)
            return;
        this.bolt = msl;
        var target = bolt.targetPlayer ? s.ship : c.otherShip;

        target_pos = world_pos_x;
        if (bolt.boltType == Bolts.BType.magic || bolt.boltType == Bolts.BType.chaos || bolt.boltType == Bolts.BType.hex)
        {
            will_hit = true;
            //find closest part to target
            if (!target.HasNonEmptyPartAtWorldX(world_pos_x))
            {
                target_pos = GetSeekerImpact(s, c);
            }
        }
        else
        {
            //check if can hit
            will_hit = target.HasNonEmptyPartAtWorldX(world_pos_x);
        }
        //create fx

        missile_fx = new FlightFX();

        missile_fx.miss = !will_hit;

        missile_fx.texture = SpriteLoader.Get(bolt.GetIcon() ?? Spr.icons_recycle);

        missile_fx.start_x = world_pos_x;

        missile_fx.target_x = target_pos;

        missile_fx.start_y = FlightFX.YRow.midrow;

        missile_fx.target_y = bolt.targetPlayer ? FlightFX.YRow.player : FlightFX.YRow.enemy;
        //slgithy longer flight time for miss to not have rockets zoom.
        missile_fx.target_flight_time = will_hit ? 0.4 : 0.6;
        this.timer = missile_fx.target_flight_time + 0.2;
        //add to fx of combat.
        c.fx.Add(missile_fx);
        Audio.Play(FSPRO.Event.Drones_MissileIncoming);
    }
    public override void Update(G g, State s, Combat c)
    {
        timer -= g.dt;
        if (timer < 0.2 && !done)
        {
            if (will_hit)
            {
                OnHit(g, s, c);
            }
            else
            {
                Audio.Play(FSPRO.Event.Drones_MissileMiss);
            }
            done = true;
        }
    }

    int GetSeekerImpact(State s, Combat c)
    {
        if (bolt == null)
            return -1;
        int worldX = world_pos_x;

        Ship ship = this.bolt.targetPlayer ? s.ship : c.otherShip;
        int num1 = 99;
        int num2 = 0;
        for (int index = 0; index < ship.parts.Count; ++index)
        {
            if (ship.parts[index].type != PType.empty)
            {
                num2 = index;
                if (index < num1)
                    num1 = index;
            }
        }
        if (world_pos_x < ship.x + num1)
            worldX = ship.x + num1;
        if (world_pos_x > ship.x + num2)
            worldX = ship.x + num2;
        if (world_pos_x == worldX)
        {
            Part? partAtWorldX = ship.GetPartAtWorldX(worldX);
            if (partAtWorldX != null && partAtWorldX.type == PType.empty)
            {
                int num3 = worldX - ship.x;
                int num4 = 99;
                int num5 = 0;
                for (int index = 0; index < ship.parts.Count - 1; ++index)
                {
                    if (ship.parts[index].type != PType.empty && Math.Abs(num3 - index) < num4)
                    {
                        num5 = index;
                        num4 = Math.Abs(num3 - index);
                    }
                }
                worldX = ship.x + num5;
            }
        }
        return worldX;
    }
    void OnHit(G g, State s, Combat c)
    {
        if (bolt == null)
        {
            if (!c.stuff.TryGetValue(world_pos_x, out var stuff) || stuff is not Bolts msl)
            {
                return;
            }
            bolt = msl;
        }
        int incomingDamage = bolt.RawDamage();
        foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts())
            incomingDamage += enumerateAllArtifact.ModifyBaseMissileDamage(s, s.route as Combat, this.bolt.targetPlayer);

        if (incomingDamage < 0)
            incomingDamage = 0;
        var target = bolt.targetPlayer ? s.ship : c.otherShip;
        DamageDone dmg = target.NormalDamage(s, c, incomingDamage, target_pos);

        EffectSpawner.NonCannonHit(g, bolt.targetPlayer, new RaycastResult() { fromDrone = true, hitDrone = false, hitShip = true, worldX = target_pos }, dmg);

        Part? partAtWorldX = target.GetPartAtWorldX(target_pos);

        if ((partAtWorldX != null ? (partAtWorldX.stunModifier == PStunMod.stunnable ? 1 : 0) : 0) != 0)
            c.QueueImmediate((CardAction)new AStunPart()
            {
                worldX = target_pos
            });

        if (target.Get(Status.payback) > 0 || target.Get(Status.tempPayback) > 0)
        {
            c.QueueImmediate((CardAction)new AAttack()
            {
                damage = Card.GetActualDamage(s, target.Get(Status.payback) + target.Get(Status.tempPayback), !bolt.targetPlayer),
                targetPlayer = !bolt.targetPlayer,
                fast = true,
            });
        }
    }
}