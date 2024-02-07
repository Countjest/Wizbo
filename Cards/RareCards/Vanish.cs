using Newtonsoft.Json;
using Nickel;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;

namespace CountJest.Wizbo.Cards;
internal static class VanishExt
{
    // We save up our part damage modifiers
    public static PDamMod? GetPDamModBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "PDamModBeforeVanish");
    public static void SetPDamModBeforeVanish(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "PDamModBeforeVanish", value);

    public static PDamMod? GetOverrideWhileActivePDamModBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PDamMod>(self, "OverrideWhileActivePDamModBeforeVanish");
    public static void SetOverrideWhileActivePDamModBeforeVanish(this Part self, PDamMod? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "OverrideWhileActivePDamModBeforeVanish", value);

    // We save up our part types
    public static PType? GetPTypeBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<PType>(self, "PTypeBeforeVanish");
    public static void SetPTypeBeforeVanish(this Part self, PType? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "PTypeBeforeVanish", value);

    // We save up our part skins
    public static string? GetSkinBeforeVanish(this Part self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<string>(self, "SkinBeforeVanish");
    public static void SetSkinBeforeVanish(this Part self, string? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "SkinBeforeVanish", value);
    public static string? GetChassisOverBeforeVanish(this Ship self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<string>(self, "ChassisOverBeforeVanish");
    public static void SetChassisOverBeforeVanish(this Ship self, string? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "ChassisOverBeforeVanish", value);
    public static string? GetChassisUnderBeforeVanish(this Ship self)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<string>(self, "ChassisUnderBeforeVanish");
    public static void SetChassisUnderBeforeVanish(this Ship self, string? value)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData(self, "ChassisUnderBeforeVanish", value);
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
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Vanish", "name"]).Localize
        });
        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) =>
        {
            if (!combat.isPlayerTurn)
                return;

            List<Ship> ships = [state.ship, combat.otherShip];
            foreach (var ship in ships)
            {
                foreach (var part in ship.parts)
                {
                    if (part.GetPDamModBeforeVanish() is { } PDamModBeforeVanish)
                    {
                        part.damageModifier = PDamModBeforeVanish;
                        part.SetPDamModBeforeVanish(null);
                    }
                    if (part.GetOverrideWhileActivePDamModBeforeVanish() is { } overrideWhileActivePDamModBeforeVanish)
                    {
                        part.damageModifierOverrideWhileActive = overrideWhileActivePDamModBeforeVanish;
                        part.SetOverrideWhileActivePDamModBeforeVanish(null);
                    }
                    if (part.GetPTypeBeforeVanish() is { } PTypeBeforeVanish)
                    {
                        part.type = PTypeBeforeVanish;
                        part.SetPTypeBeforeVanish(null);
                    }
                    if (part.GetSkinBeforeVanish() is { } SkinBeforeVanish)
                    {
                        part.skin = SkinBeforeVanish;
                        part.SetSkinBeforeVanish(null);
                    }
                }
                if (ship.GetChassisOverBeforeVanish() is { } ChassisOverBeforeVanish)
                {
                    ship.chassisOver = ChassisOverBeforeVanish;
                    ship.SetChassisOverBeforeVanish(null);
                }
                if (ship.GetChassisUnderBeforeVanish() is { } ChassisUnderBeforeVanish)
                {
                    ship.chassisUnder = ChassisUnderBeforeVanish;
                    ship.SetChassisUnderBeforeVanish(null);
                }
            }
        }, 0);
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 3,
            exhaust = true,
            retain = upgrade == Upgrade.A? true : false,
            description = ModEntry.Instance.Localizations.Localize(["card", "Vanish", "description", upgrade.ToString()])
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var ship = upgrade == Upgrade.B ? c.otherShip : s.ship;
        List<CardAction> actions = new();
        for (var partIndex = 0; partIndex < ship.parts.Count; partIndex++)
        {
            if (ship.parts[partIndex].type != PType.cockpit)
                actions.Add(new AVanishPart
                {
                    TargetPlayer = ship.isPlayerShip,
                    WorldX = ship.x + partIndex,
                    omitFromTooltips = true,
                });
        }
        actions.Add(new AVanishChassis
        {
            TargetPlayer = ship.isPlayerShip
        });
        if (upgrade == Upgrade.B)
        {
            actions.Add(new AStunShip
            {
                targetPlayer = ship.isPlayerShip
            });
        }
        return actions;
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

        if (part.damageModifier != PDamMod.none)
        {
            part.SetPDamModBeforeVanish(part.damageModifier);
            c.QueueImmediate(new ANoneMod
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX
            });
        }
        if (part.damageModifierOverrideWhileActive is not null && part.damageModifierOverrideWhileActive != PDamMod.none)
        {
            part.SetOverrideWhileActivePDamModBeforeVanish(part.damageModifierOverrideWhileActive);
            c.QueueImmediate(new ANoneMod
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX
            });
        }
        if (part.type != PType.empty)
        {
            part.SetPTypeBeforeVanish(part.type);
            part.SetSkinBeforeVanish(part.skin);
            c.QueueImmediate(new AEmptyType
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX
            });
        }
        if (part.skin != ModEntry.Instance.SEmpty.UniqueName
            && part.skin != "scaffolding_tridim_pink"
                && part.skin != "scaffolding_tridim_red"
                    && part.skin != "scaffolding_tridim_tiderunner"
                        && part.skin != "scaffolding_tridim_jupiter"
                            && part.skin != "scaffolding_tridim")
        {
            part.SetSkinBeforeVanish(part.skin);
            c.QueueImmediate(new AEmptySkin
            {
                targetPlayer = TargetPlayer,
                worldX = WorldX
            });
        }
        if (part.skin == "scaffolding_tridim_pink") part.type = PType.cockpit;
        else if (part.skin != "scaffolding_tridim_red") part.type = PType.cockpit;
        else if (part.skin != "scaffolding_tridim_tiderunner") part.type = PType.cockpit;
        else if (part.skin != "scaffolding_tridim_jupiter") part.type = PType.cockpit;
        else if (part.skin != "scaffolding_tridim") part.type = PType.cockpit;
    }
}

public class ANoneMod : CardAction
{
    public int worldX;

    public bool targetPlayer;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        Part? partAtWorldX = (targetPlayer ? s.ship : c.otherShip).GetPartAtWorldX(worldX);
        if (partAtWorldX != null)
        {
            partAtWorldX.damageModifier = PDamMod.none;
        }
    }
}
public class AEmptyType : CardAction
{
    public int worldX;

    public bool targetPlayer;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        Part? partAtWorldX = (targetPlayer ? s.ship : c.otherShip).GetPartAtWorldX(worldX);
        if (partAtWorldX != null)
        {
            partAtWorldX.type = PType.empty;
        }
    }
}
public class AEmptySkin : CardAction
{
    public int worldX;

    public bool targetPlayer;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        Part? partAtWorldX = (targetPlayer ? s.ship : c.otherShip).GetPartAtWorldX(worldX);
        if (partAtWorldX != null)
        {
            partAtWorldX.skin = ModEntry.Instance.SEmpty.UniqueName;
        }
    }
}
public class AVanishChassis : CardAction
{
    public bool TargetPlayer;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        var ship = TargetPlayer ? s.ship : c.otherShip;
        if (ship.chassisOver != null)
        {
            ship.SetChassisOverBeforeVanish(ship.chassisOver);
            ship.chassisOver = null;
        }
        if (ship.chassisUnder != null)
        {
            ship.SetChassisUnderBeforeVanish(ship.chassisUnder);
            ship.chassisUnder = "empty";
        }
    }
}