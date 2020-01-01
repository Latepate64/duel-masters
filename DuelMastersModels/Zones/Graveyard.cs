﻿using DuelMastersModels.Cards;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// A player’s graveyard is their discard pile. Discarded cards, destroyed creatures and spells cast are put in their owner's graveyard.
    /// </summary>
    public class Graveyard : Zone
    {
        internal override bool Public { get; } = true;
        internal override bool Ordered { get; } = false;

        internal Graveyard(Player owner) : base(owner) { }

        internal override void Add(Card card, Duel duel)
        {
            Cards.Add(card);
            card.KnownToOwner = true;
            card.KnownToOpponent = true;
        }

        internal override void Remove(Card card, Duel duel)
        {
            Cards.Remove(card);
        }
    }
}