﻿using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Cards;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Zones
{
    public abstract class Zone
    {
        #region Properties
        /// <summary>
        /// The cards that are in the zone.
        /// </summary>
        public ObservableCollection<Card> Cards { get; } = new ObservableCollection<Card>();

        public Collection<Creature> Creatures => new Collection<Creature>(Cards.Where(card => card is Creature).Cast<Creature>().ToList());

        public Collection<Card> TappedCards => new Collection<Card>(Cards.Where(card => card.Tapped).ToList());

        public Collection<Creature> TappedCreatures => new Collection<Creature>(Creatures.Where(creature => creature.Tapped).ToList());

        //public Collection<Creature> UntappedBlockers => new Collection<Creature>(UntappedCreatures.Where(c => c.StaticAbilities.Count(a => a.GetType() == typeof(Blocker)) > 0).ToList());

        public Collection<Card> UntappedCards => new Collection<Card>(Cards.Where(card => !card.Tapped).ToList());

        public Collection<Creature> UntappedCreatures => new Collection<Creature>(Creatures.Where(creature => !creature.Tapped).ToList());

        //public Collection<Creature> CreaturesThatCanAttack => new Collection<Creature>(UntappedCreatures.Where(creature => !creature.SummoningSickness).ToList());

        /// <summary>
        /// True if the zone is public, false if it is private.
        /// 400.2a Public zone is a zone where all players can see cards that are not facing downside It is.
        /// 400.2b Private zone is not all players can see the table of cards It is a zone.
        /// </summary>
        public abstract bool Public { get; }

        /// <summary>
        /// 400.4. The order of the cards in the shield zone or deck will be aligned unless it is effect or rule It can not be changed. Other cards in other zones, as the player wishes You can sort them. However, whether or not you tap it, the card attached to it Something must remain obvious to all players.
        /// </summary>
        public abstract bool Ordered { get; }

        public Player Owner { get; private set; }
        #endregion Properties

        protected Zone(Player owner) { Owner = owner; }

        #region Methods
        ///<summary>
        /// Adds a card to the zone.
        ///</summary>
        public abstract void Add(Card card, Duel duel);

        ///<summary>
        /// Removes a card from the zone.
        ///</summary>
        public abstract void Remove(Card card, Duel duel);

        public Collection<Card> UntappedCardsWithCivilizations(Collection<Civilization> civilizations)
        {
            return new Collection<Card>(UntappedCards.Where(card => card.Civilizations.Intersect(civilizations).Count() > 0).ToList());
        }

        public void UntapCards()
        {
            foreach (Card card in TappedCards)
            {
                card.Tapped = false;
            }
        }

        public Card GetCard(int gameId)
        {
            return Cards.First(card => card.GameId == gameId);
        }
        #endregion Methods
    }
}
