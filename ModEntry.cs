﻿using Nickel;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using System.Linq;
using AuthorName.DemoMod.Cards;
using AuthorName.DemoMod.Artifacts;

/* In the Cobalt Core modding community it is common for namespaces to be <Author>.<ModName>
 * This is helpful to know at a glance what mod we're looking at, and who made it */
namespace AuthorName.DemoMod;

/* ModEntry is the base for our mod. Others like to name it Manifest, and some like to name it <ModName>
 * Notice the ': SimpleMod'. This means ModEntry is a subclass (child) of the superclass SimpleMod (parent). This is help us use Nickel's functions more easily! */
public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    internal ISpriteEntry DemoMod_Character_CardBackground { get; }
    internal ISpriteEntry DemoMod_Character_CardFrame { get; }
    internal ISpriteEntry DemoMod_Character_Panel { get; }
    internal ISpriteEntry DemoMod_Character_Neutral_0 {  get; }
    internal ISpriteEntry DemoMod_Character_Neutral_1 {  get; }
    internal ISpriteEntry DemoMod_Character_Neutral_2 {  get; }
    internal ISpriteEntry DemoMod_Character_Neutral_3 {  get; }
    internal ISpriteEntry DemoMod_Character_Neutral_4 {  get; }
    internal ISpriteEntry DemoMod_Character_Mini_0 {  get; }
    internal ISpriteEntry DemoMod_Character_Squint_0 { get; }
    internal ISpriteEntry DemoMod_Character_Squint_1 { get; }
    internal ISpriteEntry DemoMod_Character_Squint_2 { get; }
    internal ISpriteEntry DemoMod_Character_Squint_3 { get; }
    internal IDeckEntry DemoMod_Deck { get; }
    internal IShipEntry DemoMod_Ship { get; }
    internal static IReadOnlyList<Type> DemoCharacter_StarterCard_Types { get; } = [
        /* Add more starter cards here if you'd like. */
        typeof(DemoCardFoxTale),
        typeof(DemoCardSheepDream)
    ];

    /* You can create many IReadOnlyList<Type> as a way to organize your content.
     * We recommend having a Starter Cards list, a Common Cards list, an Uncommon Cards list, and a Rare Cards list
     * However you can be more detailed, or you can be more loose, if that's your style */
    internal static IReadOnlyList<Type> DemoCharacter_CommonCard_Types { get; } = [
        
    ];

    /* We can use an IEnumerable to combine the lists we made above, and modify it if needed
     * Maybe you created a new list for Uncommon cards, and want to add it.
     * If so, you can .Concat(TheUncommonListYouMade) */
    internal static IEnumerable<Type> DemoMod_AllCard_Types
        => DemoCharacter_StarterCard_Types
        .Concat(DemoCharacter_CommonCard_Types);

    /* We'll organize our artifacts the same way: making lists and then feed those to an IEnumerable */
    internal static IReadOnlyList<Type> DemoCharacter_CommonArtifact_Types { get; } = [
        typeof(DemoArtifactBookOfTails)
    ];
    internal static IReadOnlyList<Type> DemoShip_Artifact_Types { get; } = [
        typeof(DemoArtifactCounting)
    ];
    internal static IEnumerable<Type> DemoMod_AllArtifact_Types
        => DemoCharacter_CommonArtifact_Types
        .Concat(DemoShip_Artifact_Types);


    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;

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
        DemoMod_Character_CardBackground = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_cardbackground.png"));
        DemoMod_Character_CardFrame = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_cardframe.png"));
        DemoMod_Character_Panel = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_panel.png"));
        DemoMod_Character_Neutral_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_neutral_0.png"));
        DemoMod_Character_Neutral_1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_neutral_1.png"));
        DemoMod_Character_Neutral_2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_neutral_2.png"));
        DemoMod_Character_Neutral_3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_neutral_3.png"));
        DemoMod_Character_Neutral_4 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_neutral_4.png"));
        DemoMod_Character_Mini_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_mini_0.png"));
        DemoMod_Character_Squint_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_squint_0.png"));
        DemoMod_Character_Squint_1 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_squint_1.png"));
        DemoMod_Character_Squint_2 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_squint_2.png"));
        DemoMod_Character_Squint_3 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/demomod_character_squint_3.png"));

        /* Decks are assigned separate of the character. This is because the game has decks like Trash which is not related to a playable character
         * Do note that Color accepts a HEX string format (like Color("a1b2c3")) or a Float RGB format (like Color(0.63, 0.7, 0.76). It does NOT allow a traditional RGB format (Meaning Color(161, 178, 195) will NOT work) */
        DemoMod_Deck = Helper.Content.Decks.RegisterDeck("DemoDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                /* This color is used in various situations. 
                 * It is used as the deck's rarity 'shine'
                 * If a playable character uses this deck, the character Name will use this color
                 * If a playable character uses this deck, the character mini panel will use this color */
                color = new Color("cd6a00"),

                /* This color is for the card name in-game
                 * Make sure it has a good contrast against the CardFrame, and take rarity 'shine' into account as well */
                titleColor = new Color("000000")
            },
            DefaultCardArt = DemoMod_Character_CardBackground.Sprite,
            BorderSprite = DemoMod_Character_CardFrame.Sprite,

            /* Since this deck will be used by our Demo Character, we'll use their name. */
            Name = this.AnyLocalizations.Bind(["character", "DemoCharacter", "name"]).Localize,
        });

        /*Of Note: You may notice we aren't assigning these ICharacterEntry and ICharacterAnimationEntry to any object, unlike stuff above,
        * It's totally fine to assign them, if you'd like, but we don't have a reason to in this mod */
        Helper.Content.Characters.RegisterCharacter("DemoCharacter", new CharacterConfiguration()
        {
            /* What we registered above was an IDeckEntry object, but when you register a character the Helper will ask for you to provide its Deck 'id'
             * This is simple enough, as you can get it from DemoMod_Deck */
            Deck = DemoMod_Deck.Deck,

            /* The Starter Card Types are, as the name implies, the cards you will start a DemoCharacter run with. 
             * You could provide vanilla cards if you want, but it's way more fun to create your own cards! */
            StarterCardTypes = DemoCharacter_StarterCard_Types,

            /* This is the little blurb that appears when you hover over the character in-game.
             * You can make it fluff, use it as a way to tell players about the character's playstyle, or a little bit of both! */
            Description = this.AnyLocalizations.Bind(["character", "DemoCharacter", "description"]).Localize,

            /* This is the fancy panel that encapsulates your character while in active combat.
             * It's recommended that it follows the same color scheme as the character and deck, for cohesion */
            BorderSprite = DemoMod_Character_Panel.Sprite 
        });

        /* Let's create some animations, because if you were to boot up this mod from what you have above,
         * DemoCharacter would be a blank void inside a box, we haven't added their sprites yet! */
        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            /* Characters themselves aren't used that much by the code itself, most of the time we care about having the character's deck at hand */
            Deck = DemoMod_Deck.Deck,

            /* The Looptag is the 'name' of the animation. When making shouts and events, and you want your character to show emotions, the LoopTag is what you want
             * In vanilla Cobalt Core, there are 3 'animations' looptags that any character should have: "neutral", "mini" and "squint", as these are used in: Neutral is used as default, mini is used in character select and out-of-combat UI, and Squink is hardcoded used in certain events */
            LoopTag = "neutral",

            /* The game doesn't use frames properly when there are only 2 or 3 frames. If you want a proper animation, avoid only adding 2 or 3 frames to it */
            Frames = new[]
            {
                DemoMod_Character_Neutral_0.Sprite,
                DemoMod_Character_Neutral_1.Sprite,
                DemoMod_Character_Neutral_2.Sprite,
                DemoMod_Character_Neutral_3.Sprite,
                DemoMod_Character_Neutral_4.Sprite
            }
        });
        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = DemoMod_Deck.Deck,
            LoopTag = "mini",
            Frames = new[]
            {
                /* Mini only needs one sprite. We call it animation just because we add it the same way as other expressions. */
                DemoMod_Character_Mini_0.Sprite
            }
        });
        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = DemoMod_Deck.Deck,
            LoopTag = "squint",
            Frames = new[]
            {
                DemoMod_Character_Squint_0.Sprite,
                DemoMod_Character_Squint_1.Sprite,
                DemoMod_Character_Squint_2.Sprite,
                DemoMod_Character_Squint_3.Sprite,
            }
        });

        /* The basics for a Character mod are done!
         * But you may still have mechanics you want to tackle, such as,
         * 1. How to make cards
         * 2. How to make artifacts
         * 3. How to make ships */

        /* 1. CARDS
         * DemoMod comes with a neat folder called Cards where all the .cs files for our cards are stored. Take a look.
         * You can decide to not use the folder, or to add more folders to further organize your cards. That is up to you.
         * We do recommend keeping files organized, however. It's way easier to traverse a project when the paths are clear and meaningful */

        /* Here we register our cards so we can find them in game.
         * Notice the IDemoCard interface, you can find it in InternalInterfaces.cs
         * Each card in the IEnumerable 'DemoMod_AllCard_Types' will be asked to run their 'Register' method. Open a card's .cs file, and see what it does */
        foreach (var cardType in DemoMod_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(IDemoCard.Register))?.Invoke(null, [helper]);

        /* 2. ARTIFACTS
         * Creating artifacts is pretty similar to creating Cards
         * Take a look at the Artifacts folder for demo artifacts!
         * You may also notice we're using the other interface from InternalInterfaces.cs, IDemoArtifact, to help us out */
        foreach (var artifactType in DemoMod_AllArtifact_Types)
            AccessTools.DeclaredMethod(artifactType, nameof(IDemoArtifact.Register))?.Invoke(null, [helper]);

        /* 3. SHIPS
         * Creating a ship is much like creating a character
         * You will need some assets for the ship parts
         * You can add ship-exclusive cards and artifacts too */

        /* Let's start with registering the ship parts, so we don't have to do it while making the ship proper
         * You may notice these assets are copies of the vanilla parts. Don't worry, you can get wild with your own designs! */
        var demoShipPartWing = Helper.Content.Ships.RegisterPart("DemoPart.Wing", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/demowing.png")).Sprite
        });
        var demoShipPartCannon = Helper.Content.Ships.RegisterPart("DemoPart.Cannon", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/democannon.png")).Sprite
        });
        var demoShipPartMissiles = Helper.Content.Ships.RegisterPart("DemoPart.Missiles", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/demomissiles.png")).Sprite
        });
        var demoShipPartCockpit = Helper.Content.Ships.RegisterPart("DemoPart.Cockpit", new PartConfiguration()
        {
            Sprite = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/democockpit.png")).Sprite
        });
        var demoShipSpriteChassis = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/ships/demochassis.png")).Sprite;

        /* With the parts and sprites done, we can now create our Ship a bit more easily */
        DemoMod_Ship = Helper.Content.Ships.RegisterShip("DemoShip", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    /* This is how much hull the ship will start a run with. We recommend matching hullMax */
                    hull = 12,
                    hullMax = 12,
                    shieldMaxBase = 4,
                    parts =
                    {
                        /* This is the order in which the ship parts will be arranged in-game, from left to right. Part1 -> Part2 -> Part3 */
                        new Part
                        {
                            type = PType.wing,
                            skin = demoShipPartWing.UniqueName,
                            damageModifier = PDamMod.none
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = demoShipPartCannon.UniqueName,
                            damageModifier = PDamMod.armor
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = demoShipPartMissiles.UniqueName,
                            damageModifier = PDamMod.weak
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = demoShipPartCockpit.UniqueName
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = demoShipPartWing.UniqueName,
                            flip = true
                        }
                    }
                },

                /* These are cards and artifacts the ship will start a run with. The recommended card amount is 4, and the recommended artifact amount is 2 to 3 */
                cards =
                {
                    new CannonColorless(),
                    new DodgeColorless()
                    {
                        upgrade = Upgrade.A,
                    },
                    new DodgeColorless()
                    {
                        upgrade = Upgrade.B,
                    },
                    new BasicShieldColorless(),
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new DemoArtifactCounting()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                /* If you make some artifacts that you want only this ship to encounter in a run, here is where you place them */
                typeof(DemoArtifactCounting)
            },

            UnderChassisSprite = demoShipSpriteChassis,
            Name = this.AnyLocalizations.Bind(["ship", "DemoShip", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["ship", "DemoShip", "description"]).Localize,

        });
    }
}