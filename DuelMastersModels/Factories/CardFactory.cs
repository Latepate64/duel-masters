using DuelMastersModels.Cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Factories
{
    /// <summary>
    /// Manages the instantiation of cards.
    /// </summary>
    public static class CardFactory
    {
        #region Constants
        private const string CreatureText = "Creature";
        private const string SpellText = "Spell";
        private const string EvolutionCreatureText = "Evolution Creature";
        private const string CrossGearText = "Cross Gear";
        #endregion Constants

        public static ReadOnlyCardCollection GetCardsFromJsonCards(Collection<JsonCard> jsonCards, ref int gameId, Player owner)
        {
            if (jsonCards == null)
            {
                throw new ArgumentNullException("jsonCards");
            }
            List<GameCard> cards = new List<GameCard>();
            foreach (JsonCard jsonCard in jsonCards)
            {
                cards.Add(GetCardFromJsonCard(jsonCard, gameId++, owner));
            }
            return new ReadOnlyCardCollection(cards);
        }

        /// <summary>
        /// Returns a card for card template.
        /// </summary>
        private static GameCard GetCardFromJsonCard(JsonCard jsonCard, int gameId, Player owner)
        {
            switch (jsonCard.CardType)
            {
                case CreatureText:
                    return new GameCreature(new Creature(jsonCard.Name, jsonCard.Set, jsonCard.Id, jsonCard.Civilizations, jsonCard.Rarity, jsonCard.Cost, jsonCard.Text, jsonCard.Flavor, jsonCard.Illustrator, jsonCard.Power, jsonCard.Races), gameId, owner);
                case SpellText:
                    return new GameSpell(new Spell(jsonCard.Name, jsonCard.Set, jsonCard.Id, jsonCard.Civilizations, jsonCard.Rarity, jsonCard.Cost, jsonCard.Text, jsonCard.Flavor, jsonCard.Illustrator), gameId, owner);
                case EvolutionCreatureText:
                    return new GameEvolutionCreature(new EvolutionCreature(jsonCard.Name, jsonCard.Set, jsonCard.Id, jsonCard.Civilizations, jsonCard.Rarity, jsonCard.Cost, jsonCard.Text, jsonCard.Flavor, jsonCard.Illustrator, jsonCard.Power, jsonCard.Races), gameId, owner);
                //case CrossGearText:
                //    return new GameCrossGear(jsonCard.Name, jsonCard.Set, jsonCard.Id, jsonCard.Civilizations, jsonCard.Rarity, jsonCard.Cost, jsonCard.Text, jsonCard.Flavor, jsonCard.Illustrator, gameId);
                default:
                    throw new ArgumentException("Unknown card type: " + jsonCard.CardType);
            }
        }
    }
}
