using DuelMastersModels.Cards;
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

        public Deck(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                _cards.Add(card);
            }
        }

        public override void Add(Card card)
        {
            _cards.Add(card);
        }

        public override void Remove(Card card)
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
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }

        /// <summary>
        /// Removes the top card of the deck and returns it.
        /// </summary>
        public Card RemoveAndGetTopCard()
        {
            return GetTopCard(true);
        }

        /// <summary>
        /// Returns the top card of a deck. It is also possible to remove the card from the deck.
        /// </summary>
        private Card GetTopCard(bool remove)
        {
            if (Cards.Any())
            {
                Card topCard = Cards.Last();
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
