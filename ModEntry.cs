using CountJest.Tower.Cards;
using CountJest.Wizbo.Artifacts;
using CountJest.Wizbo.Cards;
using CountJest.Wizbo.Cards.UncommonCards;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Runtime.Versioning;
using static HarmonyLib.Code;
using CountJest.Wizbo.Artifacts.Duo;

/* In the Cobalt Core modding community it is common for namespaces to be <Author>.<ModName>
 * This is helpful to know at a glance what mod we're looking at, and who made it */
namespace CountJest.Wizbo;
/* MEGA special thanks to Sorwest for the getting the VanishCard working */
/* Extra special thanks to APurpleApple for the getting the Door animation set up, ship sorcery */
/* Special thanks to Fayti1703 for getting us out of the Paradox loop :D */
/* Special thanks to Mezzelo for the heat cost sprites*/
/*rft50 */
/*clay */
/*shockah */

/* ModEntry is the base for our mod. Others like to name it Manifest, and some like to name it <ModName>
 * Notice the ': SimpleMod'. This means ModEntry is a subclass (child) of the superclass SimpleMod (parent). This is help us use Nickel's functions more easily! */
public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony { get; }
    internal IKokoroApi KokoroApi { get; }
    internal IDuoArtifactsApi? DuoArtifactsApi { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }

    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    internal ISpriteEntry Wizbo_Character_CardBackground { get; }
    internal ISpriteEntry Wizbo_Character_CardFrame { get; }
    internal ISpriteEntry Wizbo_Character_Panel { get; }
    internal ISpriteEntry Wizbo_Character_Neutral_0 { get; }
    internal ISpriteEntry Wizbo_Character_Neutral_1 { get; }
    internal ISpriteEntry Wizbo_Character_Neutral_2 { get; }
    internal ISpriteEntry Wizbo_Character_Neutral_3 { get; }
    internal ISpriteEntry Wizbo_Character_Neutral_4 { get; }
    internal ISpriteEntry Wizbo_Character_Mini_0 { get; }
    internal ISpriteEntry Wizbo_Character_Squint_0 { get; }
    internal ISpriteEntry Wizbo_Character_Squint_1 { get; }
    internal ISpriteEntry Wizbo_Character_Squint_2 { get; }
    internal ISpriteEntry Wizbo_Character_Squint_3 { get; }
    internal ISpriteEntry TowerDoor { get; }
    internal ISpriteEntry SEmpty { get; }
    internal IDeckEntry Wizbo_Deck { get; }
    internal IShipEntry MagicTower_Ship { get; }

    /*midrow*/
    internal ISpriteEntry FireMinespr { get; }
    internal ISpriteEntry Bolt { get; }
    /*Icons*/
    internal ISpriteEntry FmineIcon { get; }
    internal ISpriteEntry WboltIcon { get; }
    internal ISpriteEntry MboltIcon { get; }
    internal ISpriteEntry HboltIcon { get; }
    internal ISpriteEntry CboltIcon { get; }
    internal ISpriteEntry HeatCostUnsatisfied { get; }
    internal ISpriteEntry HeatCostSatisfied { get; }
    internal ISpriteEntry HStat { get; }
    internal ISpriteEntry SumHStat { get; }
    internal ISpriteEntry ExhstCards { get; }
    internal ISpriteEntry EHeat { get; }






    internal static IReadOnlyList<Type> Wizbo_StarterCard_Types { get; } = [
        /* Add more starter cards here if you'd like. */
        typeof(CardPocusCrocus),
        typeof(CardCrocusPocus)
    ];

    /* You can create many IReadOnlyList<Type> as a way to organize your content.
     * We recommend having a Starter Cards list, a Common Cards list, an Uncommon Cards list, and a Rare Cards list
     * However you can be more detailed, or you can be more loose, if that's your style */
    internal static IReadOnlyList<Type> Wizbo_CommonCard_Types { get; } = [
        typeof(CardMiasma),
        typeof(CardToxic),
        typeof(CardKoolahLimpoo),
        typeof(CardHashakalah),
        typeof(CardSpillYourDrink),
        typeof(CardYeet),
        typeof(CardShazammy),
        typeof(CardDeeMekoides)
    ];
    internal static IReadOnlyList<Type> Wizbo_UncommonCard_Types { get; } = [
        typeof(CardAbraKadoozle),
        typeof(CardGreaterLesserBeam),
        typeof(CardKablooiePachinko),
        typeof(CardSkiddleDeePop),
        typeof(CardFiddleDeeDoop),
    ];
    internal static IReadOnlyList<Type> Wizbo_RareCard_Types { get; } = [
        typeof(CardKachow),
        typeof(CardVanish)
    ];
    /* We can use an IEnumerable to combine the lists we made above, and modify it if needed
     * Maybe you created a new list for Uncommon cards, and want to add it.
     * If so, you can .Concat(TheUncommonListYouMade) */
    internal static IReadOnlyList<Type> Wizbo_TowerCard_Types { get; } = [
        typeof(CardPonder)
    ];

    internal static IEnumerable<Type> Wizbo_AllCard_Types
        => Wizbo_StarterCard_Types
        .Concat(Wizbo_CommonCard_Types)
        .Concat(Wizbo_UncommonCard_Types)
        .Concat(Wizbo_RareCard_Types)
        .Concat(Wizbo_TowerCard_Types);

    /* We'll organize our artifacts the same way: making lists and then feed those to an IEnumerable */
    internal static IReadOnlyList<Type> Wizbo_CommonArtifact_Types { get; } = [
        typeof(GrimoireOfPower),
        typeof(GrimoireOfSpeed),//meh
        typeof(EtherealGrimoire),/*Boss*/
        typeof(ParadoxGrimoire)/*Boss meh */
    ];
    /*Duo Artifacts*/
    internal static IReadOnlyList<Type> DuoArtifactTypes { get; } = [
        typeof(WizboDizzyArtifact),
        /*typeof(WizboRiggsArtifact),
        typeof(WizboPeriArtifact),
        typeof(WizboIsaacArtifact),
        typeof(WizboDrakeArtifact),
        typeof(WizboMaxArtifact),
        typeof(WizboBooksArtifact),
        typeof(WizboCatArtifact)*/
    ];
    /*Ship starting artifacts*/
    internal static IReadOnlyList<Type> TowerShip_Artifact_Types { get; } = [
        typeof(FramjificentCore),
        typeof(FriendlyHearth),
        typeof(ReinforcedGate)
    ];
    internal static IEnumerable<Type> Wizbo_AllArtifact_Types
        => Wizbo_CommonArtifact_Types
        .Concat(TowerShip_Artifact_Types);


    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        DuoArtifactsApi = helper.ModRegistry.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts")!;
        Harmony = new(package.Manifest.UniqueName);
        _ = new HPCoreExhaust();
        _ = new HPGrimoireExhaust();
        _ = new HPAGrimoireExhaust();
        _ = new HPPGrimoireExhaust();
        _ = new HPArtifactBlacklist();
        _ = new HPShipAnim();


        CustomTTGlossary.ApplyPatches(Harmony);
        this.AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
        );
        //Char
        Wizbo_Character_CardBackground = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_char_cardbackground.png"));
        Wizbo_Character_CardFrame = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_char_cardframe.png"));
        Wizbo_Character_Panel = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_char_panel.png"));
        Wizbo_Character_Neutral_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_neutral_0.png"));
        Wizbo_Character_Neutral_1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_neutral_1.png"));
        Wizbo_Character_Neutral_2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_neutral_2.png"));
        Wizbo_Character_Neutral_3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_neutral_3.png"));
        Wizbo_Character_Neutral_4 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_neutral_4.png"));
        Wizbo_Character_Mini_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_mini_0.png"));
        Wizbo_Character_Squint_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_squint_0.png"));
        Wizbo_Character_Squint_1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_squint_1.png"));
        Wizbo_Character_Squint_2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_squint_2.png"));
        Wizbo_Character_Squint_3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wizard_squint_3.png"));
        //ship
        TowerDoor = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/door.png"));
        SEmpty = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/none.png"));
        //stuffbase
        FireMinespr = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/FireMine.png"));
        Bolt = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Bolt.png"));
        //Icons
        FmineIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/fmine.png"));
        WboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_wbolt.png"));
        MboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_mbolt.png"));
        HboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_hbolt.png"));
        CboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_cbolt.png"));
        HeatCostUnsatisfied = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/mezz_heatCostOff.png"));
        HeatCostSatisfied = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/mezz_heatCost.png"));
        HStat = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/hstat.png"));
        SumHStat = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/sumhstat.png"));
        ExhstCards = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/exhstcards.png"));
        EHeat = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/eheat.png"));

        Wizbo_Deck = Helper.Content.Decks.RegisterDeck("WizboDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                /* This color is used in various situations. 
                 * It is used as the deck's rarity 'shine'
                 * If a playable character uses this deck, the character Name will use this color
                 * If a playable character uses this deck, the character mini panel will use this color */
                color = new Color("7d5bff"),

                /* This color is for the card name in-game
                 * Make sure it has a good contrast against the CardFrame, and take rarity 'shine' into account as well */
                titleColor = new Color("d4b55b")
            },
            DefaultCardArt = Wizbo_Character_CardBackground.Sprite,
            BorderSprite = Wizbo_Character_CardFrame.Sprite,
            Name = this.AnyLocalizations.Bind(["character", "Wizbo", "name"]).Localize,
        });
        Helper.Content.Characters.RegisterCharacter("Wizbo", new CharacterConfiguration()
        {
            Deck = Wizbo_Deck.Deck,
            StarterCardTypes = Wizbo_StarterCard_Types,
            Description = this.AnyLocalizations.Bind(["character", "Wizbo", "description"]).Localize,
            BorderSprite = Wizbo_Character_Panel.Sprite,
            NeutralAnimation = new()
            {
                Deck = Wizbo_Deck.Deck,
                LoopTag = "neutral",
                Frames = new[]
                {
                    Wizbo_Character_Neutral_0.Sprite,
                    Wizbo_Character_Neutral_1.Sprite,
                    Wizbo_Character_Neutral_2.Sprite,
                    Wizbo_Character_Neutral_3.Sprite,
                    Wizbo_Character_Neutral_4.Sprite
                }
            },
            MiniAnimation = new()
            {
                Deck = Wizbo_Deck.Deck,
                LoopTag = "mini",
                Frames = new[]
                {
                    Wizbo_Character_Mini_0.Sprite
                }
            }
        });
        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Wizbo_Deck.Deck,
            LoopTag = "squint",
            Frames = new[]
            {
                Wizbo_Character_Squint_0.Sprite,
                Wizbo_Character_Squint_1.Sprite,
                Wizbo_Character_Squint_2.Sprite,
                Wizbo_Character_Squint_3.Sprite,
            }
        });
        foreach (var cardType in Wizbo_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(IDemoCard.Register))?.Invoke(null, [helper]);

        foreach (var artifactType in Wizbo_AllArtifact_Types)
            AccessTools.DeclaredMethod(artifactType, nameof(IDemoArtifact.Register))?.Invoke(null, [helper]);

        var TowerPartWing = Helper.Content.Ships.RegisterPart("TowerPart.Wing", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/towerwing.png")).Sprite
        });
        var TowerPartCannon = Helper.Content.Ships.RegisterPart("TowerPart.Cannon", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/towercannon.png")).Sprite
        });
        var TowerPartScaffold = Helper.Content.Ships.RegisterPart("TowerPart.Scaffold", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/towerscaffolding.png")).Sprite
        });
        var TowerPartMissiles = Helper.Content.Ships.RegisterPart("TowerPart.Missiles", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/towermissilesOpen.png")).Sprite
        });
        var TowerPartCockpit = Helper.Content.Ships.RegisterPart("TowerPart.Cockpit", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/towercockpit.png")).Sprite
        });
        var TowerSpriteChassis = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/towerchassis.png")).Sprite;

        /* With the parts and sprites done, we can now create our Ship a bit more easily */
        MagicTower_Ship = Helper.Content.Ships.RegisterShip("Tower", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    /* This is how much hull the ship will start a run with. We recommend matching hullMax */
                    hull = 10,
                    hullMax = 10,
                    shieldMaxBase = 5,
                    evadeMax = 4,
                    parts =
                    {
                        /* This is the order in which the ship parts will be arranged in-game, from left to right. Part1 -> Part2 -> Part3 */
                        new Part
                        {
                            type = PType.wing,
                            skin = TowerPartWing.UniqueName,
                            damageModifier = PDamMod.none
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = TowerPartCannon.UniqueName,
                            damageModifier = PDamMod.none
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = TowerPartMissiles.UniqueName,
                            damageModifier = PDamMod.none,
                            key = "Gate"
                        },
                        new Part
                        {
                            type = PType.empty,
                            skin = TowerPartScaffold.UniqueName,
                            damageModifier = PDamMod.none
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = TowerPartCockpit.UniqueName
                        },
                    }
                },

                /* These are cards and artifacts the ship will start a run with. The recommended card amount is 4, and the recommended artifact amount is 2 to 3 */
                cards =
                {
                    new CannonColorless(),
                    new CardPonder(),
                    new DodgeColorless(),
                    new BasicShieldColorless(),
                },
                artifacts =
                {
                    new ReinforcedGate(),
                    new FriendlyHearth(),
                    new FramjificentCore()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                /* If you make some artifacts that you want only this ship to encounter in a run, here is where you place them */
                typeof(FramjificentCore),
                typeof(FriendlyHearth),
                typeof(ReinforcedGate)
            },

            UnderChassisSprite = TowerSpriteChassis,
            Name = this.AnyLocalizations.Bind(["ship", "Tower", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["ship", "Tower", "description"]).Localize,

        });
        if (DuoArtifactsApi is not null)
        {
            foreach (var artifactType in DuoArtifactTypes)
                AccessTools.DeclaredMethod(artifactType, nameof(IDemoArtifact.Register))?.Invoke(null, [helper]);
        }
    }
}
/* Dialog ideas :
 Wizbo + Max, discuss magic AfsFlse.S. non magic when Vanish reappears your tridim cockpit*/

