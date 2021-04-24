using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// The mana zone is where cards are put in order to produce mana for using other cards. All cards are put into the mana zone upside down. However, multicolored cards are put into the mana zone tapped.
    /// </summary>
    public class ManaZone : Zone
    {
        internal override bool Public { get; } = true;
        internal override bool Ordered { get; } = false;

        public IEnumerable<ICard> TappedCards => new ReadOnlyCollection<ICard>(Cards.Where(card => card.Tapped).ToList());
        public IEnumerable<ICard> UntappedCards => new ReadOnlyCollection<ICard>(Cards.Where(card => !card.Tapped).ToList());

        public void UntapCards()
        {
            foreach (ICard card in TappedCards)
            {
                card.Tapped = false;
            }
        }

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