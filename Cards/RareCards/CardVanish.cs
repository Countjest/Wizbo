using System;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Nanoray.PluginManager;
using System.Linq.Expressions;

namespace CountJest.Wizbo.Cards;

internal static class VExt
{
    public static PType? GettypeBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "typeBeforeVanish");

    public static void SettypeBeforeVanish(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "typeBeforeVanish", value);

    public static PType? GettypeOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "typeOverride");

    public static void SettypeOverride(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "typeOverride", value);
    public static PDamMod? GetDamageModifier(this Part self)
    => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "DamageModifier");

    public static void SetDamageModifier(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "DamageModifier", value);

    public static PDamMod? GetdamageModifierOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "damageModifierOverride");

    public static void SetdamageModifierOverride(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "damageModifierOverride", value);
    public static String? GetSkinBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<String>(self, "SkinBeforeVanish");

    public static void SetSkinBeforeVanish(this Part self, String? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "SkinBeforeVanish", value);

    public static String? GetSkinOverride(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<String>(self, "SkinOverride");

    public static void SetSkinOverride(this Part self, String? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "SkinOverride", value);
}

internal sealed class CardVanish : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Vanish", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true,
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Vanish", "name"]).Localize
        });
    }
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) =>
        {
            List<Ship> ships = [state.ship, combat.otherShip];
            foreach (var ship in ships)
            {
                foreach (var part in ship.parts)
                {
                    if (part.type != PType.cockpit && part.GettypeBeforeVanish() is { } typeBeforeVanish)
                    {
                        part.type = typeBeforeVanish;
                        part.SettypeBeforeVanish(null);
                    }
                    if (part.typeOverride != PType.empty && part.GettypeOverride() is { } typeOverride)
                    {
                        part.typeOverride = typeOverride;
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
                    if (String?.skin != null && String.GetSkinBeforeVanish() is { } SkinBeforeVanish)
                    {
                        string.SkinBeforeVanish = SkinBeforeVanish;
                        String?.SetSkinBeforeVanish(null);
                    }
                    if (String?.SkinOverride == "parts/empty.png" && String.GetSkinOverride() is { } SkinOverride)
                    {
                        string.SkinOverride = SkinOverride;
                        String?.SetSkinOverride(null);
                    }
                }

            }
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 3,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "Vanish", "description", upgrade.ToString()])
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<Ship> ships = [s.ship];
        if (upgrade == Upgrade.B)
            ships.Add(c.otherShip);

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
            {
                foreach (var partIndex in ship.parts)
                {
                    if (part.type != PType.cockpit)
                    {
                        part.type = part.typeBeforeVanish;
                        part.type = PType.empty;
                    }
                    else return;
                    if (part.skin != "parts/empty.png")
                    {
                        part.skin = SetSkinBeforeVanish;
                        part.skin = "parts/empty.png";
                    }
                }
            }
        }
    }
}

