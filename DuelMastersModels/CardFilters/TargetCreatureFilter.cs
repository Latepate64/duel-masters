using DuelMastersModels.Cards;

namespace DuelMastersModels.CardFilters
{
    public class TargetCreatureFilter : CreatureFilter
    {
        public GameCreature Creature { get; private set; }

        public TargetCreatureFilter(GameCreature creature)
        {
            Creature = creature;
        }

        public override ReadOnlyCreatureCollection FilteredCreatures => new ReadOnlyCreatureCollection(Creature);
    }
}
