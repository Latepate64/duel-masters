using DuelMastersModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Zones
{
    /// <summary>
    /// Battle Zone is the main place of the game. Creatures, Cross Gears, Weapons, Fortresses, Beats and Fields are put into the battle zone, but no mana, shields, castles nor spells may be put into the battle zone.
    /// </summary>
    public class BattleZone : Zone
    {
        internal override bool Public { get; } = true;
        internal override bool Ordered { get; } = false;

        public IEnumerable<Creature> Creatures => new ReadOnlyCollection<Creature>(Cards.OfType<Creature>().ToList());

        public IEnumerable<Creature> GetTappedCreatures()
        {
            return new ReadOnlyCollection<Creature>(Creatures.Where(creature => creature.Tapped).ToList());
        }

        public IEnumerable<Creature> GetUntappedCreatures()
        {
            return new ReadOnlyCollection<Creature>(Creatures.Where(creature => !creature.Tapped).ToList());
        }

        public IEnumerable<Card> TappedCards => new ReadOnlyCollection<Card>(Cards.Where(c => c.Tapped).ToList());

        public Duel Duel { get; set; }

        public void UntapCards()
        {
            foreach (Creature creature in GetTappedCreatures())
            {
                creature.Tapped = false;
            }
        }

        public override void Add(Card card)
        {
            _cards.Add(card);
            if (card is Creature creature)
            {
                Duel.TriggerWhenYouPutThisCreatureIntoTheBattleZoneAbilities(creature);
                Duel.TriggerWheneverAnotherCreatureIsPutIntoTheBattleZoneAbilities(creature);
            }
        }

        public override void Remove(Card card)
        {
            _ = _cards.Remove(card);
        }

        public IEnumerable<Creature> GetUntappedCreatures(Player player)
        {
            return GetUntappedCreatures().Where(c => c.Owner == player);
        }

        public IEnumerable<Creature> GetTappedCreatures(Player player)
        {
            return GetTappedCreatures().Where(c => c.Owner == player);
        }
    }
}