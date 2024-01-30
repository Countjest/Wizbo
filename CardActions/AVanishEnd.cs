using Nanoray.PluginManager;
using Newtonsoft.Json;
using Nickel;
using System;
using System.Collections.Generic;


namespace CountJest.Wizbo;

internal static class VExt
{
    public static PType? Gettype(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "type");

    public static void Settype(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "type", value);

    public static PType? GettypeOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "TypeOverride");

    public static void SettypeOverride(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "TypeOverride", value);
    public static PDamMod? GetDamageModifier(this Part self)
    => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "DamageModifier");

    public static void SetDamageModifier(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "DamageModifier", value);

    public static PDamMod? GetdamageModifierOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "damageModifierOverride");

    public static void SetdamageModifierOverride(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "damageModifierOverride", value);
    public static String? GetSkin(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<String>(self, "Skin");

    public static void SetSkin(this Part self, String? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "Skin", value);

    public static String? GetSkinOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<String>(self, "SkinOverride");

    public static void SetSkinOverride(this Part self, String? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "SkinOverride", value);
}
internal class AVanishEnd : CardAction // We made a custom card action that uses attributes and parameters from its parent class CardAction
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) =>
        {
            List<Ship> ships = [state.ship, combat.otherShip];
            foreach (var ship in ships)
            {
                foreach (var part in ship.parts)
                {
                    if (part.type != PType.cockpit && part.Gettype() is { } type)
                    {
                        part.type = type;
                        part.Settype(null);
                    }
                    if (part.TypeOverride != PType.empty && part.GettypeOverride() is { } TypeOverride)
                    {
                        part.TypeOverride = TypeOverride;
                        part.SettypeOverride(null);
                    }
                }
                foreach (var part in ship.parts)
                {
                    if (part.damageModifier != PDamMod.none && part.GetDamageModifier() is { } DamageModifier)
                    {
                        part.damageModifier = DamageModifier;
                        part.SetDamageModifier(null);
                    }
                    if (part.damageModifierOverride != PDamMod.none && part.GetdamageModifierOverride() is { } damageModifierOverride)
                    {
                        part.damageModifierOverride = damageModifierOverride;
                        part.SetdamageModifierOverride(null);
                    }
                }
                foreach (var String in ship.parts)
                {
                    if (String.skin != null && String.GetSkin() is { } Skin)
                    {
                        String.skin = Skin;
                        String.SetSkin(null);
                    }
                    if (String.SkinOverride == "parts/empty.png" && String.GetSkinOverride() is { } SkinOverride)
                    {
                        String.SkinOverride = SkinOverride;
                        String.SetSkin(null);
                    }
                }

            }
        });
    }
}




