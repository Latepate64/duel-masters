﻿namespace DuelMastersModels.Cards
{
    internal abstract class DeckCard : Card, IDeckCard
    {
        protected internal DeckCard(ICard card) : base(card.Name, card.CardSet, card.Id, card.Cost, card.Text, card.Flavor, card.Illustrator, card.Civilizations, card.Rarity)
        {
        }

        public bool KnownToOwner { get; internal set; } = true;

        public bool KnownToOpponent { get; internal set; } = false;
    }
}
