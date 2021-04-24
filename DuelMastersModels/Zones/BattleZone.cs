using DuelMastersInterfaceModels.Cards;
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

        public IEnumerable<ICreature> Creatures => new ReadOnlyCollection<ICreature>(Cards.OfType<ICreature>().ToList());

        public IEnumerable<ICreature> GetTappedCreatures()
        {
            return new ReadOnlyCollection<ICreature>(Creatures.Where(creature => creature.Tapped).ToList());
        }

        public IEnumerable<ICreature> GetUntappedCreatures()
        {
            return new ReadOnlyCollection<ICreature>(Creatures.Where(creature => !creature.Tapped).ToList());
        }

        public IEnumerable<ICard> TappedCards => new ReadOnlyCollection<ICard>(Cards.Where(c => c.Tapped).ToList());

        public IDuel Duel { get; set; }

        public void UntapCards()
        {
            foreach (ICreature creature in GetTappedCreatures())
            {
                creature.Tapped = false;
            }
        }

        public override void Add(ICard card)
        {
            _cards.Add(card);
            if (card is ICreature creature)
            {
                Duel.TriggerWhenYouPutThisCreatureIntoTheBattleZoneAbilities(creature);
                Duel.TriggerWheneverAnotherCreatureIsPutIntoTheBattleZoneAbilities(creature);
            }
        }

        public override void Remove(ICard card)
        {
            _ = _cards.Remove(card);
        }

        public IEnumerable<ICreature> GetUntappedCreatures(IPlayer player)
        {
            return GetUntappedCreatures().Where(c => c.Owner == player);
        }

        public IEnumerable<ICreature> GetTappedCreatures(IPlayer player)
        {
            return GetTappedCreatures().Where(c => c.Owner == player);
        }
    }
}