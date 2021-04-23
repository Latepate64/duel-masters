namespace DuelMastersModels.Cards
{
    /// <summary>
    /// Represents a battle zone card.
    /// </summary>
    public abstract class BattleZoneCard : Card, IBattleZoneCard
    {
        /// <summary>
        /// Creates a battle zone card.
        /// </summary>
        /// <param name="card"></param>
        protected BattleZoneCard(ICard card) : base(card.CardID, card.Cost, card.Civilizations)
        {
        }

        public bool Tapped { get; set; }
    }
}
