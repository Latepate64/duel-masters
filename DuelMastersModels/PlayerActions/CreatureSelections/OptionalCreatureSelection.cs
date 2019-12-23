using DuelMastersModels.Cards;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public abstract class OptionalCreatureSelection : CreatureSelection
    {
        protected OptionalCreatureSelection() { }

        protected OptionalCreatureSelection(Player player, ReadOnlyCreatureCollection creatures) : base(player, 0, 1, creatures)
        { }

        public GameCreature SelectedCreature { get; set; }

        public override PlayerAction TryToPerformAutomatically(Duel duel)
        {
            if (Creatures.Count == 0)
            {
                return Perform(duel, null);
            }
            else
            {
                return this;
            }
            //return Creatures.Count == 0;
        }

        public bool Validate(GameCreature creature)
        {
            return creature == null || Creatures.Contains(creature);
        }

        public abstract PlayerAction Perform(Duel duel, GameCreature creature);
    }
}
