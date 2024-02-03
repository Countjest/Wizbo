using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CountJest.Wizbo;
public class HexBolt : Missile
{
    public static readonly string MIDROW_OBJECT_NAME = "missileHexBolt";
    public static readonly int BASE_DAMAGE = 1;
    static HexBolt()
    {
        DB.drones[MIDROW_OBJECT_NAME] = (Spr)ModEntry.sprites["midrow/dagger"].Id;
    }
    public HexBolt()
    {
        base.skin = MIDROW_OBJECT_NAME;
    }
}
