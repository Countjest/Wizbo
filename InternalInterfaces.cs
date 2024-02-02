using Nickel;
using System.Collections.Generic;

namespace CountJest.Wizbo;

/* Much like a namespace, these interfaces can be named whatever you'd like.
 * We recommend using descriptive names for what they're supposed to do.
 * In this case, we use the IDemoCard interface to call for a Card's 'Register' method */
internal interface IDemoCard
{
    static abstract void Register(IModHelper helper);
    List<CardAction> GetActions(State s, Combat c);
}

internal interface IDemoArtifact
{
    static abstract void Register(IModHelper helper);
}
