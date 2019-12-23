using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Cards
{
    public class ObservableCardCollection : ObservableCollection<GameCard>
    {
        public ObservableCardCollection() { }

        public ObservableCardCollection(ObservableCardCollection list) : base(list)
        {
        }

        #region ReadOnlyCardCollection
        public ReadOnlyCardCollection TappedCards => new ReadOnlyCardCollection(Items.Where(card => card.Tapped));
        public ReadOnlyCardCollection UntappedCards => new ReadOnlyCardCollection(Items.Where(card => !card.Tapped));

        public ReadOnlyCardCollection UntappedCardsWithCivilizations(ReadOnlyCivilizationCollection civilizations)
        {
            return new ReadOnlyCardCollection(UntappedCards.Where(card => card.Civilizations.Intersect(civilizations).Count() > 0));
        }
        #endregion ReadOnlyCardCollection

        #region ReadOnlyCreatureCollection
        public ReadOnlyCreatureCollection Creatures => new ReadOnlyCreatureCollection(Items.Where(card => card is GameCreature).Cast<GameCreature>());
        public ReadOnlyCreatureCollection TappedCreatures => new ReadOnlyCreatureCollection(Creatures.TappedCreatures);
        public ReadOnlyCreatureCollection UntappedCreatures => new ReadOnlyCreatureCollection(Creatures.Where(creature => !creature.Tapped));
        public ReadOnlyCreatureCollection NonEvolutionCreatures => new ReadOnlyCreatureCollection(Creatures.Where(c => !(c is GameEvolutionCreature)));
        public ReadOnlyCreatureCollection NonEvolutionCreaturesThatCostTheSameAsOrLessThanTheNumberOfCardsInTheZone => new ReadOnlyCreatureCollection(NonEvolutionCreatures.Where(c => c.Cost <= Items.Count));
        #endregion ReadOnlyCreatureCollection
    }

    public class CardCollection : ReadOnlyCardCollection
    {
        public CardCollection() : base(new List<GameCard>())
        {
        }

        public void Add(GameCard card)
        {
            Items.Add(card);
        }

        public void Remove(GameCard card)
        {
            Items.Remove(card);
        }
    }

    public class ReadOnlyCardCollection : ReadOnlyCollection<GameCard>
    {
        public ReadOnlyCardCollection(IEnumerable<GameCard> cards) : base(cards.ToList()) { }

        public ReadOnlyCardCollection(GameCard card) : base(new List<GameCard>() { card }) { }
    }

    public class ReadOnlyCreatureCollection : ReadOnlyCollection<GameCreature>
    {
        public ReadOnlyCreatureCollection(IEnumerable<GameCreature> creatures) : base(creatures.ToList()) { }

        public ReadOnlyCreatureCollection(GameCreature creature) : base(new List<GameCreature>() { creature }) { }

        public ReadOnlyCreatureCollection TappedCreatures => new ReadOnlyCreatureCollection(Items.Where(creature => creature.Tapped));
    }

    public class ReadOnlySpellCollection : ReadOnlyCollection<GameSpell>
    {
        public ReadOnlySpellCollection(IEnumerable<GameSpell> spells) : base(spells.ToList()) { }

        public ReadOnlySpellCollection(GameSpell spell) : base(new List<GameSpell>() { spell }) { }
    }

    public class SpellCollection : ReadOnlySpellCollection
    {
        public SpellCollection() : base(new List<GameSpell>())
        {
        }

        public void Add(GameSpell spell)
        {
            Items.Add(spell);
        }

        public void Remove(GameSpell spell)
        {
            Items.Remove(spell);
        }
    }
}
