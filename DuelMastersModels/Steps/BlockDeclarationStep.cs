using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.CreatureSelections;

namespace DuelMastersModels.Steps
{
    public class BlockDeclarationStep : Step
    {
        public GameCreature AttackingCreature { get; private set; }
        public GameCreature BlockingCreature { get; set; }

        public BlockDeclarationStep(Player activePlayer, GameCreature attackingCreature) : base(activePlayer)
        {
            AttackingCreature = attackingCreature;
        }

        public override PlayerAction PlayerActionRequired(Duel duel)
        {
            return null;
        }

        public override PlayerAction ProcessTurnBasedActions(Duel duel)
        {
            if (duel == null)
            {
                throw new System.ArgumentNullException("duel");
            }
            Player nonActivePlayer = duel.GetOpponent(ActivePlayer);
            ReadOnlyCreatureCollection creatures = duel.GetCreaturesThatCanBlock(AttackingCreature);
            if (creatures.Count > 0)
            {
                return new DeclareBlock(nonActivePlayer, creatures);
            }
            else
            {
                return null;
            }
        }
    }
}
