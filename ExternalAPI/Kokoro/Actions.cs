using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountJest.Wizbo;

public partial interface IKokoroApi
{
    IActionApi Actions { get; }
    public interface IActionApi
    {
        CardAction MakeExhaustEntireHandImmediate();
        CardAction MakePlaySpecificCardFromAnywhere(int cardId, bool showTheCardIfNotInHand = true);
        CardAction MakePlayRandomCardsFromAnywhere(IEnumerable<int> cardIds, int amount = 1, bool showTheCardIfNotInHand = true);

        CardAction MakeContinue(out Guid id);
        CardAction MakeContinued(Guid id, CardAction action);
        IEnumerable<CardAction> MakeContinued(Guid id, IEnumerable<CardAction> action);
        CardAction MakeStop(out Guid id);
        CardAction MakeStopped(Guid id, CardAction action);
        IEnumerable<CardAction> MakeStopped(Guid id, IEnumerable<CardAction> action);
        AVariableHint SetTargetPlayer(AVariableHint action, bool targetPlayer);
        AVariableHint MakeEnergyX(AVariableHint? action = null, bool energy = true, int? tooltipOverride = null);
        AStatus MakeEnergy(AStatus action, bool energy = true);
    }
}