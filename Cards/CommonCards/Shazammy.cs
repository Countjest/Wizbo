using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class CardShazammy : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Shazammy", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Shazammy", "name"]).Localize
        });

    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.None ? 1 : 1,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
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
                        thing = new FireMine
                        {
                            yAnimation = 0.0
                        }
                    } //And ofc, make sure to replace these ... for the actual info you want the action to have, this is familiar territory to you
                    ) // We close the whoooooooole thing up

                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {                   
                    ModEntry.Instance.KokoroApi.ActionCosts.Make( 
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost( 
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource( 
                                Status.heat, 
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player, 
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite, 
                                    ModEntry.Instance.HeatCostSatisfied.Sprite 
                            ), 
                            amount: 1 
                        ), 
                    new ASpawn
                    {
                        thing = new FireMine
                        {
                            yAnimation = 0.0
                        }
                    } 
                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make(
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost(
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource(
                                Status.heat,
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player,
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite,
                                    ModEntry.Instance.HeatCostSatisfied.Sprite
                            ),
                            amount: 1
                        ),
                    new ASpawn
                    {
                        offset = -1,
                        thing = new FireMine
                        {
                            yAnimation = 0.0
                        }
                    }
                    ),

                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    ModEntry.Instance.KokoroApi.ActionCosts.Make(
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost(
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource(
                                Status.heat,
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player,
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite,
                                    ModEntry.Instance.HeatCostSatisfied.Sprite
                            ),
                            amount: 1
                        ),
                    new ASpawn
                    {
                        thing = new FireMine
                        {
                            yAnimation = 0.0
                        }
                    }
                    ),
                    ModEntry.Instance.KokoroApi.ActionCosts.Make(
                        ModEntry.Instance.KokoroApi.ActionCosts.Cost(
                            ModEntry.Instance.KokoroApi.ActionCosts.StatusResource(
                                Status.heat,
                                    target: IKokoroApi.IActionCostApi.StatusResourceTarget.Player,
                                    ModEntry.Instance.HeatCostUnsatisfied.Sprite,
                                    ModEntry.Instance.HeatCostSatisfied.Sprite
                            ),
                            amount: 1
                        ),
                    new ASpawn
                    {
                        offset = 1,
                        thing = new FireMine
                        {
                            yAnimation = 0.0
                        }
                    }
                    ),
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
