using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.CreatureSelections;

namespace DuelMastersModels.Steps
{
    public class AttackDeclarationStep : Step
    {
        public GameCreature AttackingCreature { get; set; }
        public GameCreature AttackedCreature { get; set; }
        public Player NonactivePlayer { get; private set; }
        public bool TargetOfAttackDeclared { get; set; }

        public AttackDeclarationStep(Player activePlayer, Player nonactivePlayer) : base(activePlayer)
        {
            NonactivePlayer = nonactivePlayer;
        }

        public override PlayerAction PlayerActionRequired(Duel duel)
        {
            if (AttackingCreature != null && !TargetOfAttackDeclared)
            {
                //TODO: If attacked creature is not null, check that it can be attacked.
                return new DeclareTargetOfAttack(ActivePlayer, duel.GetCreaturesThatCanBeAttacked(ActivePlayer, AttackingCreature));
            }
            else
            {
                return null;
            }
        }

        public override PlayerAction ProcessTurnBasedActions(Duel duel)
        {
            if (duel == null)
            {
                throw new System.ArgumentNullException("duel");
            }
            ReadOnlyCreatureCollection creatures = duel.GetCreaturesThatCanAttack(ActivePlayer);
            if (creatures.Count > 0)
            {
                return new DeclareAttacker(ActivePlayer, creatures);
            }
            else
            {
                return null;
            }
        }
    }
}
