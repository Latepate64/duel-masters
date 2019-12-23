using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using System;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public class DeclareBlock : OptionalCreatureSelection
    {
        public DeclareBlock(Player player, ReadOnlyCreatureCollection creatures) : base(player, creatures)
        { }

        public override PlayerAction Perform(Duel duel, GameCreature creature)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            if (creature != null)
            {
                // 507.1b The chosen creature becomes a blocking creature. It remains a blocking creature until it's removed from the attack or until the end of attack step, whichever comes first.
                BlockDeclarationStep blockDeclarationStep = duel.CurrentTurn.CurrentStep as BlockDeclarationStep;
                blockDeclarationStep.BlockingCreature = creature;
                // 507.1c The nonactive player taps the chosen creature.
                creature.Tapped = true;
                // 507.1d The attacking creature becomes a blocked creature. It remains a blocked creature until it's removed from the attack or until the end of attack step, whichever comes first. A creature remains blocked even if all the blocking creature is removed from the attack.
            }
            return null;
        }
    }
}
