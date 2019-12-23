using DuelMastersModels.Cards;

namespace DuelMastersModels.Zones
{
    public class ManaZone : Zone
    {
        public override bool Public { get; } = true;
        public override bool Ordered { get; } = false;

        public ManaZone(Player owner) : base(owner) { }

        public override void Add(GameCard card, Duel duel)
        {
            if (card.Civilizations.Count > 1)
            {
                card.Tapped = true;
            }
            Cards.Add(card);
            card.KnownToOwner = true;
            card.KnownToOpponent = true;
        }

        public override void Remove(GameCard card, Duel duel)
        {
            Cards.Remove(card);
            card.Tapped = false;
        }
    }
}