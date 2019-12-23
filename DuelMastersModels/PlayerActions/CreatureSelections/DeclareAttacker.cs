using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using System;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public class DeclareAttacker : OptionalCreatureSelection
    {
        public DeclareAttacker(Player player, ReadOnlyCreatureCollection creatures) : base(player, creatures)
        { }

        public override PlayerAction Perform(Duel duel, GameCreature creature)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            if (creature != null)
            {
                creature.Tapped = true;
                AttackDeclarationStep step = (duel.CurrentTurn.CurrentStep as AttackDeclarationStep);
                step.AttackingCreature = creature;
            }
            return null;
        }
    }
}
