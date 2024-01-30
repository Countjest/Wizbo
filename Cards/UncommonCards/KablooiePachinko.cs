using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CountJest.Wizbo.Cards;

internal sealed class KablooiePachinko : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("KablooiePachinko", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Wizbo_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "KablooiePachinko", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 1,
            /* In a similar manner to how we localized card names, we'll localize their descriptions
             * For example, if Sheep Dream is upgraded to B, this description would try getting information from card > SheepDream > Description > B in the locale file */
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();
        int boostMod = 0;
        if (s.route is Combat)
            boostMod = s.ship.Get(Status.boost);
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AMedusaField()
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                   new AStatus()
                    {
                        shardcost = 1,
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, (GetShardAmt(s) > 0 ? (1 + boostMod) : 0))
                        /* We can add a status to the attack. This status will be applied if the attack hits a ship part, and Jupiter drones will also copy these when copying an AAttack
                         * To remember: AStatus applies a status to the enemy no matter where they are, but an AAttack with a status attached will only apply it if it hits
                         * In this case, we'll give the enemy 1 stack of Boost, so the next status they gain will get +1, sppoky!
                         * Note that Boost no longer gets used up by Status.shield or Status.tempShield. This change was implemented in the 1.0.2 patch */
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, (GetShardAmt(s) > 0 ? (1 + boostMod) : 1))
                        /* We can add a status to the attack. This status will be applied if the attack hits a ship part, and Jupiter drones will also copy these when copying an AAttack
                         * To remember: AStatus applies a status to the enemy no matter where they are, but an AAttack with a status attached will only apply it if it hits
                         * In this case, we'll give the enemy 1 stack of Boost, so the next status they gain will get +1, sppoky!
                         * Note that Boost no longer gets used up by Status.shield or Status.tempShield. This change was implemented in the 1.0.2 patch */
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                   new AStatus()
                    {
                        shardcost = 1,
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, (GetShardAmt(s) > 0 ? (1 + boostMod) : 1))
                        /* We can add a status to the attack. This status will be applied if the attack hits a ship part, and Jupiter drones will also copy these when copying an AAttack
                         * To remember: AStatus applies a status to the enemy no matter where they are, but an AAttack with a status attached will only apply it if it hits
                         * In this case, we'll give the enemy 1 stack of Boost, so the next status they gain will get +1, sppoky!
                         * Note that Boost no longer gets used up by Status.shield or Status.tempShield. This change was implemented in the 1.0.2 patch */
                    },
                    new AStatus()
                    {
                        shardcost = 1,
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AAttack()
                    {
                       damage = GetDmg(s, (GetShardAmt(s) > 1 ? (2 + boostMod) : ((GetShardAmt(s) > 0 ? (1 + boostMod) : 0))))
                        /* We can add a status to the attack. This status will be applied if the attack hits a ship part, and Jupiter drones will also copy these when copying an AAttack
                         * To remember: AStatus applies a status to the enemy no matter where they are, but an AAttack with a status attached will only apply it if it hits
                         * In this case, we'll give the enemy 1 stack of Boost, so the next status they gain will get +1, sppoky!
                         * Note that Boost no longer gets used up by Status.shield or Status.tempShield. This change was implemented in the 1.0.2 patch */
                    }
                };
                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
