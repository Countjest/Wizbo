using Nickel;
using OneOf.Types;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

/* Like other namespaces, this can be named whatever
 * However it's recommended that you follow the structure defined by ModEntry of <AuthorName>.<ModName> or <AuthorName>.<ModName>.Cards*/
namespace CountJest.Wizbo.Cards.UncommonCards;

/* The Card's class name IS IMPORTANT, however. This is what the game will ask for when trying to get a card.
 * If your card's class shares the same name as a vanilla card, or shares it with a modded card, the game can't keep both, and will only use one
 * For this reason, we recommend to give a unique name that is unlikely to be repeated by others, such as incorporating AuthorName or ModName to it */
internal sealed class CardGreaterLesserBeam : Card, IDemoCard
{
    /* For a bit more info on the Register Method, look at InternalInterfaces.cs and 1. CARDS section in ModEntry */
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("GreaterLesserBeam", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                /* We don't assign cards to characters, but rather to decks! It's important to keep that in mind */
                deck = ModEntry.Instance.Wizbo_Deck.Deck,

                /* The vanilla rarities are Rarity.common, Rarity.uncommon, Rarity.rare */
                rarity = Rarity.uncommon,

                /* Some vanilla cards don't upgrade, some only upgrade to A, but most upgrade to either A or B */
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "GreaterLesserBeam", "name"]).Localize
            /* AnyLocalizations.Bind().Localize will find the 'name' of 'Foxtale' in the locale file and feed it here. The output for english in-game from this is 'Fox Tale' */
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade switch
            {
                Upgrade.A => 2,
                Upgrade.B => 3,
                _ => 3
            },
            exhaust = true,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new ASpawn
            {
                thing = new Bolt
                {
                    boltType = BType.Hex,
                    targetPlayer = false
                }
            }
        };
        /* The meat of the card, this is where we define what a card does, and some would say the most fun part of modding Cobalt Core happens here! */
        /*
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {

                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = -1,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = 1,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
             {

                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = -1,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = 1,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
             {

                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = -1,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = 1,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( //We access it and create a CardAction that will ask for a Cost and a CardAction to do for it
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( //We access it again and create the cost
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( //We access it again and declare our cost to be a status
                                Status.heat, //our status
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, //The target, in our case, we ask heat from the player
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, //We need to declare a custom sprite!!! Make sure to create one in ModEntry for it
                                    ModEntry.Instance.HeatCostSatisfied.Sprite //We also need one for the satisfied one!!! So also create it on ModEntry
                            ), //We close the status cost
                            amount: 1 //We declare the amout of the cost we're asking
                        ), //We close the cost
                        new ASpawn
                        {
                            offset = -2,
                            thing = new Bolt
                            {
                                boltType = BType.Chaos,
                                targetPlayer = false
                            }
                        }

                    ),
                };
                actions = cardActionList3;
                break;
        }
        return actions;
        */
    }
}
