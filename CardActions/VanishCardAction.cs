using Nanoray.PluginManager;
using Newtonsoft.Json;
using Nickel;
using System;
using System.Collections.Generic;


namespace CountJest.Wizbo;

internal static class VExt
{
    public static PType? GettypeBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "TypeBeforeVanish");

    public static void SettypeBeforeVanish(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "TypeBeforeVanish", value);

    public static PType? GettypeOverrideWhileActiveBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "TypeOverrideWhileActiveBeforeVanish");

    public static void SettypeOverrideWhileActiveBeforeVanish(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "TypeOverrideWhileActiveBeforeVanish", value);
    public static PDamMod? GetDamageModifierBeforeVanish(this Part self)
    => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "DamageModifierBeforeVanish");

    public static void SetDamageModifierBeforeVanish(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "DamageModifierBeforeVanish", value);

    public static PDamMod? GetDamageModifierOverrideWhileActiveBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "DamageModifierOverrideWhileActiveBeforeVanish");

    public static void SetDamageModifierOverrideWhileActiveBeforeVanish(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "DamageModifierOverrideWhileActiveBeforeVanish", value);
    public static string? GetSkin(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<String>(self, "SkinBeforeVanish");

    public static void SetSkinBeforeVanish(this Part self, String? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "SkinBeforeVanish", value);

    public static string? GetSkinOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<String>(self, "SkinOverrideWhileActiveBeforeVanish");

    public static void SetSkinOverrideWhileActiveBeforeVanish(this Part self, String value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "SkinOverrideWhileActiveBeforeVanish", value);
}
internal class VanishCardAction : CardAction // We made a custom card action that uses attributes and parameters from its parent class CardAction
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) =>
        {
            if (!combat.isPlayerTurn)
                return;

            List<Ship> ships = [state.ship, combat.otherShip];
            foreach (var ship in ships)
            {
                foreach (var part in ship.parts)
                {
                    if (part.type != PType.cockpit && part.GettypeBeforeVanish() is { } TypeBeforeVanish)
                    {
                        part.type = TypeBeforeVanish;
                        part.SettypeBeforeVanish(null);
                    }
                }
                foreach (var part in ship.parts)
                {
                    if (part.damageModifier != PDamMod.none && part.GetDamageModifierBeforeVanish() is { } DamageModifierBeforeVanish)
                    {
                        part.damageModifier = DamageModifierBeforeVanish;
                        part.SetDamageModifierBeforeVanish(null);
                    }
                }
                foreach (var part in ship.parts)
                {
                    if (part.skin != "none" && part.GetSkin() is { } SkinBeforeVanish)
                    {
                        part.skin = SkinBeforeVanish;
                        part.SetSkinBeforeVanish(null);
                    }
                }
            }
        }
        

    public override void Begin(G g, State s, Combat c) // We inherited Begin method from parent CardAction, we will overwrite it with our own method
    {
        foreach (Part part in s.ship.parts)
        {
            if (part.type != PType.cockpit)
            {
                part.type = PType.empty;
                part.skin = "parts/empty.png";
            }
        }
        if (!PlayerTurn)
            {

        }
        base.Begin(g, s, c);
        timer = 0;


    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<Ship> ships = [s.ship];
        List<CardAction> actions = [];
        foreach (var ship in ships)
            for (var partIndex = 0; partIndex < ship.parts.Count; partIndex++)
                if (ship.parts[partIndex].type != PType.empty)
                    actions.Add(new AVanishPart
                    {
                        TargetPlayer = ship.isPlayerShip,
                        WorldX = ship.x + partIndex,
                        omitFromTooltips = true,
                    });
        return actions;
    }
        if (part.typeOverrideWhileActive is not null && part.typeOverrideWhileActive != PType.cockpit)
        {
            part.SettypeOverrideWhileActiveBeforeVanish(part.typeOverrideWhileActive);
            c.QueueImmediate(new AArmor
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX,
                justTheActiveOverride = true
            });
        }
    }
}
public sealed class AVanishPart : CardAction
{
    [JsonProperty]
    public required bool TargetPlayer;

    [JsonProperty]
    public required int WorldX;

    public override void Begin(G g, State s, Combat c)
    {
        base.Begin(g, s, c);
        timer = 0;

        var ship = TargetPlayer ? s.ship : c.otherShip;
        if (ship.GetPartAtWorldX(WorldX) is not { } part)
            return;

        if (part.type != PType.cockpit)
        {
            part.SettypeBeforeVanish(part.type);
            c.QueueImmediate(new AArmor
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX
            });
        }
        if (part.typeOverrideWhileActive is not null && part.typeOverrideWhileActive != PType.cockpit)
        {
            part.SettypeOverrideWhileActiveBeforeVanish(part.typeOverrideWhileActive);
            c.QueueImmediate(new AArmor
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX,
                justTheActiveOverride = true
            });
        }
    }
}
