using System;
using System.Collections.Generic;

namespace CountJest.Wizbo;

public interface IMoreDifficultiesApi
{
    void RegisterAltStarters(Deck deck, StarterDeck starterDeck);
}