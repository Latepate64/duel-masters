using DuelMastersModels.Cards;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// At the beginning of the game, each player puts five shields into their shield zone. Castles are put into the shield zone to fortify a shield.
    /// </summary>
    public class ShieldZone : Zone
    {
        internal override bool Public { get; } = false;
        internal override bool Ordered { get; } = true;

        public override void Add(ICard card)
        {
            _cards.Add(card);
        }

        public override void Remove(ICard card)
        {
            _ = _cards.Remove(card);
        }
    }
}