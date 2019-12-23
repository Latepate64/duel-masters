using DuelMastersModels.Cards;

namespace DuelMastersModels.Zones
{
    public class ShieldZone : Zone
    {
        public override bool Public { get; } = false;
        public override bool Ordered { get; } = true;

        public ShieldZone(Player owner) : base(owner) { }

        public override void Add(GameCard card, Duel duel)
        {
            Cards.Add(card);
        }

        public override void Remove(GameCard card, Duel duel)
        {
            Cards.Remove(card);
        }
    }
}