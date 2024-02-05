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
using System.Runtime.Versioning;

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
    internal ISpriteEntry FireMinespr { get; }
    internal ISpriteEntry Mbolt0 { get; }
    internal ISpriteEntry Mbolt1 { get; }
    internal ISpriteEntry Mbolt2 { get; }
    internal ISpriteEntry Mbolt3 { get; }
    internal ISpriteEntry Mbolt4 { get; }
    internal ISpriteEntry Hbolt0 { get; }
    internal ISpriteEntry Hbolt1 { get; }
    internal ISpriteEntry Hbolt2 { get; }
    internal ISpriteEntry Hbolt3 { get; }
    internal ISpriteEntry Hbolt4 { get; }
    internal ISpriteEntry Cbolt0 { get; }
    internal ISpriteEntry Cbolt1 { get; }
    internal ISpriteEntry Cbolt2 { get; }
    internal ISpriteEntry Cbolt3 { get; }
    internal ISpriteEntry Cbolt4 { get; }
    
    /*Icons*/
    internal ISpriteEntry MboltIcon { get; }
    internal ISpriteEntry HboltIcon { get; }
    internal ISpriteEntry CboltIcon { get; }
    internal ISpriteEntry HeatCostUnsatisfied { get; }
    internal ISpriteEntry HeatCostSatisfied { get; }
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
        typeof(CardShazammy)
    ];
    internal static IReadOnlyList<Type> Wizbo_UncommonCard_Types { get; } = [
        typeof(CardAbraKadoozle),
        typeof(CardGreaterLesserBeam),
        typeof(CardKablooiePachinko),
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
        typeof(AegisGrimoire),
        typeof(GrimoireOfSpeed),
        typeof(EtherealGrimoire),/*Boss*/
        typeof(ParadoxGrimoire)/*Boss*/
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
        Harmony = new(package.Manifest.UniqueName);
        _ = new HPCoreExhaust();
        _ = new HPGrimoireExhaust();
        _ = new HPAGrimoireExhaust();
        _ = new HPPGrimoireExhaust();
        _ = new HPArtifactBlacklist();
        _ = new HPShipAnim();

        /* These localizations lists help us organize our mod's text and messages by language.
         * For general use, prefer AnyLocalizations, as that will provide an easier time to potential localization submods that are made for your mod 
         * IMPORTANT: These localizations are found in the i18n folder (short for internationalization). The Demo Mod comes with a barebones en.json localization file that you might want to check out before continuing 
         * Whenever you add a card, artifact, character, ship, pretty much whatever, you will want to update your locale file in i18n with the necessary information */
        this.AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
        );


        /* Assigning our ISpriteEntry objects manually. This is the easiest way to do it when starting out!
         * Of note: GetRelativeFile is case sensitive. Double check you've written the file names correctly */
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
        TowerDoor = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/door.png"));
        SEmpty = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/none.png"));
        //stuffbase
        FireMinespr = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/FireMine.png"));
        Mbolt0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Mbolt0.png"));
        Mbolt1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Mbolt1.png"));
        Mbolt2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Mbolt2.png"));
        Mbolt3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Mbolt3.png"));
        Mbolt4 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Mbolt4.png"));
        Hbolt0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Hbolt0.png"));
        Hbolt1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Hbolt1.png"));
        Hbolt2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Hbolt2.png"));
        Hbolt3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Hbolt3.png"));
        Hbolt4 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Hbolt4.png"));
        Cbolt0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Cbolt0.png"));
        Cbolt1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Cbolt1.png"));
        Cbolt2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Cbolt2.png"));
        Cbolt3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Cbolt3.png"));
        Cbolt4 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/Cbolt4.png"));
        //Icons
        MboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_mbolt.png"));
        HboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_hbolt.png"));
        CboltIcon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/icon_cbolt.png"));
        HeatCostUnsatisfied = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/mezz_heatCostOff.png"));
        HeatCostSatisfied = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/mezz_heatCost.png"));

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

            /* Since this deck will be used by our Demo Character, we'll use their name. */
            Name = this.AnyLocalizations.Bind(["character", "Wizbo", "name"]).Localize,
        });

        /*Of Note: You may notice we aren't assigning these ICharacterEntry and ICharacterAnimationEntry to any object, unlike stuff above,
        * It's totally fine to assign them, if you'd like, but we don't have a reason to in this mod */
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

        /* The basics for a Character mod are done!
         * But you may still have mechanics you want to tackle, such as,
         * 1. How to make cards
         * 2. How to make artifacts
         * 3. How to make ships */

        /* 1. CARDS
         * Wizbo comes with a neat folder called Cards where all the .cs files for our cards are stored. Take a look.
         * You can decide to not use the folder, or to add more folders to further organize your cards. That is up to you.
         * We do recommend keeping files organized, however. It's way easier to traverse a project when the paths are clear and meaningful */

        /* Here we register our cards so we can find them in game.
         * Notice the IDemoCard interface, you can find it in InternalInterfaces.cs
         * Each card in the IEnumerable 'Wizbo_AllCard_Types' will be asked to run their 'Register' method. Open a card's .cs file, and see what it does */
        foreach (var cardType in Wizbo_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(IDemoCard.Register))?.Invoke(null, [helper]);

        /* 2. ARTIFACTS
         * Creating artifacts is pretty similar to creating Cards
         * Take a look at the Artifacts folder for demo artifacts!
         * You may also notice we're using the other interface from InternalInterfaces.cs, IDemoArtifact, to help us out */
        foreach (var artifactType in Wizbo_AllArtifact_Types)
            AccessTools.DeclaredMethod(artifactType, nameof(IDemoArtifact.Register))?.Invoke(null, [helper]);

        /* 3. SHIPS
         * Creating a ship is much like creating a character
         * You will need some assets for the ship parts
         * You can add ship-exclusive cards and artifacts too */

        /* Let's start with registering the ship parts, so we don't have to do it while making the ship proper
         * You may notice these assets are copies of the vanilla parts. Don't worry, you can get wild with your own designs! */
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
    }
}
/* Dialog ideas :
 Wizbo + Max, discuss magic V.S. non magic when Vanish reappears your tridim cockpit*/

