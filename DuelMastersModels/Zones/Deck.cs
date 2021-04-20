using DuelMastersModels.Cards;
using DuelMastersModels.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// When a game begins, each player’s deck becomes their deck.
    /// </summary>
    public class Deck : Zone<IDeckCard>, IDeck
    {
        internal override bool Public { get; } = false;
        internal override bool Ordered { get; } = true;

        public Deck(IEnumerable<ICard> cards)
        {
            foreach (ICard card in cards)
            {
                _cards.Add(CardFactory.GenerateDeckCard(card));
            }
        }

        public override void Add(IDeckCard card)
        {
            _cards.Add(card);
        }

        public override void Remove(IDeckCard card)
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
                IDeckCard value = _cards[k];
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
                IDeckCard topCard = Cards.Last();
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
