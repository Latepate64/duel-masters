using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using System;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public class DeclareTargetOfAttack : OptionalCreatureSelection
    {
        public DeclareTargetOfAttack(Player player, ReadOnlyCreatureCollection creatures) : base(player, creatures)
        { }

        public override PlayerAction Perform(Duel duel, GameCreature creature)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            AttackDeclarationStep step = (duel.CurrentTurn.CurrentStep as AttackDeclarationStep);
            step.AttackedCreature = creature;
            step.TargetOfAttackDeclared = true;
            return null;
        }

        public override PlayerAction TryToPerformAutomatically(Duel duel)
        {
            if (!duel.CanAttackOpponent((duel.CurrentTurn.CurrentStep as AttackDeclarationStep).AttackingCreature) && Creatures.Count == 1)
            {
                return Perform(duel, Creatures[0]);
            }
            else if (Creatures.Count == 0)
            {
                return Perform(duel, null);
            }
            else
            {
                return this;
            }
        }
    }
}
