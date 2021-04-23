using System.Linq;

namespace DuelMastersModels.Cards
{
    internal abstract class ManaZoneCard : Card, IManaZoneCard
    {
        protected internal ManaZoneCard(ICard card) : base(card.CardID, card.Cost, card.Civilizations)
        {
            Tapped = Civilizations.Count() > 1;
        }

        public bool Tapped { get; set; }
    }
}
