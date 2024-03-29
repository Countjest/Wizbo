﻿using Nickel;
using OneOf.Types;
using System.Collections.Generic;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

/* Like other namespaces, this can be named whatever
 * However it's recommended that you follow the structure defined by ModEntry of <AuthorName>.<ModName> or <AuthorName>.<ModName>.Cards*/
namespace CountJest.Wizbo.Cards;

/* The Card's class name IS IMPORTANT, however. This is what the game will ask for when trying to get a card.
 * If your card's class shares the same name as a vanilla card, or shares it with a modded card, the game can't keep both, and will only use one
 * For this reason, we recommend to give a unique name that is unlikely to be repeated by others, such as incorporating AuthorName or ModName to it */
internal sealed class CardMiazbo : Card, IDemoCard
{
    /* For a bit more info on the Register Method, look at InternalInterfaces.cs and 1. CARDS section in ModEntry */
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Miazbo", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                /* We don't assign cards to characters, but rather to decks! It's important to keep that in mind */
                deck = ModEntry.Instance.Wizbo_Deck.Deck,

                /* The vanilla rarities are Rarity.common, Rarity.uncommon, Rarity.rare */
                rarity = Rarity.common,

                /* Some vanilla cards don't upgrade, some only upgrade to A, but most upgrade to either A or B */
                upgradesTo = [],
                
                dontOffer = true
            },
            /* AnyLocalizations.Bind().Localize will find the 'name' of 'Foxtale' in the locale file and feed it here. The output for english in-game from this is 'Fox Tale' */
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Miazbo", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 0,
            temporary = true,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "Miazbo", "description", upgrade.ToString()])
            /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
            /* if we don't set a card specific 'art' here, the game will give it the deck's 'DefaultCardArt' */
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        /* The meat of the card, this is where we define what a card does, and some would say the most fun part of modding Cobalt Core happens here! */
        List<CardAction> actions = new();

        /* Since we want to have different actions for each Upgrade, we use a switch that covers the Upgrade paths we've defined */
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    /* AStatus is a card action that gives the target a status and, unlike attacks, its effect is unavoidable
                     * Of Note: AStatuses will default to targetPlayer = false, If what you want is for the card to give the player a status, you want to set it targetPlayer = true
                     * Other types of CardActions like AMove, AHurt or AHeal operate as such too, so it's important to remember */
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    /* AStatus is a card action that gives the target a status and, unlike attacks, its effect is unavoidable
                     * Of Note: AStatuses will default to targetPlayer = false, If what you want is for the card to give the player a status, you want to set it targetPlayer = true
                     * Other types of CardActions like AMove, AHurt or AHeal operate as such too, so it's important to remember */
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    /* AStatus is a card action that gives the target a status and, unlike attacks, its effect is unavoidable
                     * Of Note: AStatuses will default to targetPlayer = false, If what you want is for the card to give the player a status, you want to set it targetPlayer = true
                     * Other types of CardActions like AMove, AHurt or AHeal operate as such too, so it's important to remember */
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
    public override void OnDraw(State s, Combat c)
    {
        c.QueueImmediate
            ([
                new AStatus()
                {
                    status = Status.heat,
                    statusAmount = 2,
                    targetPlayer = false
                }
            ]);
    }
    
}
