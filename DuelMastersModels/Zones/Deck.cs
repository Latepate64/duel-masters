using DuelMastersInterfaceModels.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// When a game begins, each player’s deck becomes their deck.
    /// </summary>
    public class Deck : Zone
    {
        internal override bool Public { get; } = false;
        internal override bool Ordered { get; } = true;

        public Deck(IEnumerable<ICard> cards)
        {
            foreach (ICard card in cards)
            {
                _cards.Add(card);
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

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void Shuffle()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ICard value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }

        /// <summary>
        /// Removes the top card of the deck and returns it.
        /// </summary>
        public ICard RemoveAndGetTopCard()
        {
            return GetTopCard(true);
        }

        /// <summary>
        /// Returns the top card of a deck. It is also possible to remove the card from the deck.
        /// </summary>
        private ICard GetTopCard(bool remove)
        {
            if (Cards.Any())
            {
                ICard topCard = Cards.Last();
                if (remove)
                {
                    Remove(topCard);
                }
                return topCard;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
