using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    [HarmonyPatch(typeof(Card))]
    public class ATooltipDummy : ADummyAction
    {
        public List<Tooltip>? tooltips;
        public List<Icon>? icons;
        public bool renderVarAssignment;
        public override List<Tooltip> GetTooltips(State s)
        {
            return tooltips ?? new();
        }

        public override Icon? GetIcon(State s)
        {
            return null;
        }

        public virtual List<Icon> GetIcons(State s) { return icons ?? new(); }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(Card.RenderAction))]
        public static bool HarmonyPrefix_Card_RenderAction(ref int __result, G g, State state, CardAction action, bool dontDraw = false, int shardAvailable = 0, int stunChargeAvailable = 0, int bubbleJuiceAvailable = 0)
        {
            if (action is ATooltipDummy aTooltipDummy)
            {
                var icons = aTooltipDummy.GetIcons(state);
                if (icons == null)
                {
                    return true;
                }

                if (dontDraw)
                {
                    return false;
                }

                Color spriteColor = (action.disabled ? Colors.disabledIconTint : new Color("ffffff"));
                int w = 0;
                bool isFirst = true;

                if (aTooltipDummy.renderVarAssignment)
                {
                    //VarAssignment(g,  ref w, spriteColor, action.disabled, true);
                    w += 8 + 9; // "X =" is always the same width, no need to calculate it
                }

                foreach (var icon in icons)
                {
                    IconAndOrNumber(icon.path, ref isFirst, ref w, g, action, state, spriteColor, true, amount: icon.number, iconWidth: SpriteLoader.Get(icon.path)?.Width ?? 8);
                }

                w = -w / 2;
                isFirst = true;

                if (aTooltipDummy.renderVarAssignment)
                {
                    VarAssignment(g, ref w, spriteColor, action.disabled, dontDraw);
                }

                foreach (var icon in icons)
                {
                    IconAndOrNumber(icon.path, ref isFirst, ref w, g, action, state, spriteColor, dontDraw, amount: icon.number, iconWidth: SpriteLoader.Get(icon.path)?.Width ?? 8, textColor: icon.color);
                }
            } 
            else
            {
                return true;
            }

            return false;
        }

        private static void VarAssignment(G g, ref int w, Color spriteColor, bool disabled = false, bool dontDraw = false)
        {
            //w--;
            if (dontDraw) return;
            
            Rect? rect = new Rect(w);
            Vec xy = g.Push(null, rect).rect.xy;
            Draw.Sprite
            (
                Enum.Parse<Spr>("icons_x"), 
                xy.x + 1.0,
                xy.y - 1.0, 
                color: spriteColor
            );

            Draw.Text
            (
                "=", 
                xy.x + 12.0,
                xy.y + 2.0, 
                color: (disabled ? Colors.disabledText : Colors.textMain),
                outline: Colors.black,
                dontSubstituteLocFont: true
            );
            g.Pop();
            
            int iconWidth = 8; // the X is a standard icon
            w += iconWidth + 9;
        }

        private static void IconAndOrNumber(Spr icon, ref bool isFirst, ref int w, G g, CardAction action, State state, Color spriteColor, bool dontDraw, int iconIconPadding = -1, int iconNumberPadding = 2, int iconWidth = 8, int numberWidth = 6, int? amount = null, Color? textColor = null, bool flipY = false, int? x = null)
        {
            if (!isFirst)
            {
                w += iconIconPadding;
            }
            if (!dontDraw)
            {
                Rect? rect = new Rect(w);
                Vec xy = g.Push(null, rect).rect.xy;
                Draw.Sprite(icon, xy.x, xy.y, flipX: false, flipY, 0.0, null, null, null, null, spriteColor);
                g.Pop();
            }
            w += iconWidth;
            if (amount.HasValue)
            {
                int valueOrDefault4 = amount.GetValueOrDefault();
                if (!x.HasValue)
                {
                    w += iconNumberPadding;
                    string text = DB.IntStringCache(valueOrDefault4);
                    if (!dontDraw)
                    {
                        Rect? rect = new Rect(w);
                        Vec xy = g.Push(null, rect).rect.xy;
                        BigNumbers.Render(valueOrDefault4, xy.x, xy.y, textColor ?? Colors.textMain);
                        g.Pop();
                    }
                    w += text.Length * numberWidth;
                }
            }
            if (x.HasValue)
            {
                if (x < 0)
                {
                    w += iconNumberPadding;
                    if (!dontDraw)
                    {
                        Rect? rect = new Rect(w - 2);
                        Vec xy = g.Push(null, rect).rect.xy;
                        Color? color14 = (action.disabled ? new Color?(spriteColor) : textColor);
                        Draw.Sprite(Enum.Parse<Spr>("icons_minus"), xy.x, xy.y - 1.0, flipX: false, flipY: false, 0.0, null, null, null, null, color14);
                        g.Pop();
                    }
                    w += 3;
                }
                if (Math.Abs(x.Value) > 1)
                {
                    w += iconNumberPadding + 1;
                    if (!dontDraw)
                    {
                        G g18 = g;
                        Rect? rect12 = new Rect(w);
                        Vec xy16 = g18.Push(null, rect12).rect.xy;
                        BigNumbers.Render(Math.Abs(x.Value), xy16.x, xy16.y, textColor ?? Colors.textMain);
                        g.Pop();
                    }
                    w += 4;
                }
                w += iconNumberPadding;
                if (!dontDraw)
                {
                    G g19 = g;
                    Rect? rect12 = new Rect(w);
                    Vec xy17 = g19.Push(null, rect12).rect.xy;
                    Spr? id13 = Enum.Parse<Spr>("icons_x_white");
                    double x17 = xy17.x;
                    double y16 = xy17.y - 1.0;
                    Color? color14 = action.GetIcon(state)?.color;
                    Draw.Sprite(id13, x17, y16, flipX: false, flipY: false, 0.0, null, null, null, null, color14);
                    g.Pop();
                }
                w += 8;
            }
            isFirst = false;
        }
    }
}
