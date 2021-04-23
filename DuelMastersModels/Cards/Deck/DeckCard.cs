namespace DuelMastersModels.Cards
{
    internal abstract class DeckCard : Card, IDeckCard
    {
        protected internal DeckCard(ICard card) : base(card.CardID, card.Cost, card.Civilizations)
        {
        }

        public bool KnownToOwner { get; internal set; } = true;

        public bool KnownToOpponent { get; internal set; } = false;
    }
}
