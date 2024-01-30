using CountJest.Wizbo.Artifacts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo
{
    internal sealed class HPShipAnim
    {
        public HPShipAnim()
        {
            ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.DrawTopLayer)),
            postfix: new HarmonyMethod(GetType(), nameof(HPShipAnim.DrawTopLayer)));
        }
        public static void DrawTopLayer(Ship __instance, G g, Vec v, Vec worldPos)
        {
            var ShipKey = __instance.key == ModEntry.Instance.MagicTower_Ship.UniqueName;
            if (ShipKey is false)
                return;
            int i = __instance.parts.FindIndex((x) => x.key == "Gate");
            if (i >= 0)
            {
                Spr? id = ModEntry.Instance.TowerDoor.Sprite;
                Part bay = __instance.parts[i];
                Vec bayPos = v + worldPos + new Vec((bay.xLerped ?? ((double)i)) * 16.0, -32.0 + (__instance.isPlayerShip ? bay.offset.y : (1.0 + (0.0 - bay.offset.y)))) + new Vec(-1.0, -1.0);
                Draw.Sprite(id, (bayPos.x+bay.pulse*7), bayPos.y);
                Draw.Sprite(id, (bayPos.x+bay.pulse*(-7)), bayPos.y, flipX:true);
                
            }

        }
    }
}
