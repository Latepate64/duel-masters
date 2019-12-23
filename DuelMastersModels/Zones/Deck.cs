using DuelMastersModels.Cards;
using System;

namespace DuelMastersModels.Zones
{
    public class Deck : Zone
    {
        public override bool Public { get; } = false;
        public override bool Ordered { get; } = true;

        public Deck(Player owner) : base(owner) { }

        public override void Add(GameCard card, Duel duel)
        {
            Cards.Add(card);
        }

        public override void Remove(GameCard card, Duel duel)
        {
            Cards.Remove(card);
        }

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void Shuffle()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int n = Cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                GameCard value = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = value;
            }
        }

        /// <summary>
        /// Removes the top card of the deck and returns it.
        /// </summary>
        public GameCard RemoveAndGetTopCard(Duel duel)
        {
            return GetTopCard(true, duel);
        }

        /// <summary>
        /// Returns the top card of a deck. It is also possible to remove the card from the deck.
        /// </summary>
        private GameCard GetTopCard(bool remove, Duel duel)
        {
            if (Cards.Count > 0)
            {
                GameCard topCard = Cards[Cards.Count - 1];
                if (remove)
                {
                    Remove(topCard, duel);
                }
                return topCard;
            }
            else
            {
                throw new ArgumentException("Deck is out of cards.");
            }
        }
    }
}
