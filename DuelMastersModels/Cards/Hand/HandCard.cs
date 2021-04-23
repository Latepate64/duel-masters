namespace DuelMastersModels.Cards
{
    internal abstract class HandCard : Card, IHandCard
    {
        protected internal HandCard(ICard card) : base(card.CardID, card.Cost, card.Civilizations)
        {
        }

        public bool KnownToOwner { get; internal set; } = true;

        public bool KnownToOpponent { get; internal set; } = false;
    }
}
