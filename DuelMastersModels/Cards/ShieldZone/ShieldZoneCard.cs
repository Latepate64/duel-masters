namespace DuelMastersModels.Cards
{
    internal abstract class ShieldZoneCard : Card, IShieldZoneCard
    {
        protected internal ShieldZoneCard(ICard card) : base(card.CardID, card.Cost, card.Civilizations)
        {
        }

        public bool KnownToOwner { get; internal set; } = false;

        public bool KnownToOpponent { get; internal set; } = false;
    }
}
