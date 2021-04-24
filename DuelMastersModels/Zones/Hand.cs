using DuelMastersModels.Cards;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// The hand is where a player holds cards that have been drawn. Cards can be put into a player’s hand by other effects as well. At the beginning of the game, each player draws five cards.
    /// </summary>
    public class Hand : Zone
    {
        internal override bool Public { get; } = false;
        internal override bool Ordered { get; } = false;

        public override void Add(Card card)
        {
            _cards.Add(card);
        }

        public override void Remove(Card card)
        {
            _ = _cards.Remove(card);
        }
    }
}