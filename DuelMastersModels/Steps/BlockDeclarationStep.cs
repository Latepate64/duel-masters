using DuelMastersInterfaceModels.Cards;
using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public class BlockDeclarationStep : TurnBasedActionStep
    {
        internal ICreature AttackingCreature { get; private set; }
        internal ICreature AttackedCreature { get; private set; }
        internal ICreature BlockingCreature { get; set; }

        public BlockDeclarationStep(IPlayer activePlayer, ICreature attackingCreature, ICreature attackedCreature) : base(activePlayer)
        {
            AttackingCreature = attackingCreature;
            AttackedCreature = attackedCreature;
        }

        public override IChoice PerformTurnBasedAction()
        {
            // TODO: Check if blocking is possible
            bool possibleToBlock = false;
            if (possibleToBlock)
            {
                return new BlockerChoice(ActivePlayer.ID);
            }
            else
            {
                return null;
            }
        }

        public override IStep GetNextStep()
        {
            if (BlockingCreature != null)
            {
                return new BattleStep(ActivePlayer, AttackingCreature, BlockingCreature);
            }
            else if (AttackedCreature != null)
            {
                return new BattleStep(ActivePlayer, AttackingCreature, AttackedCreature);
            }
            else
            {
                return new DirectAttackStep(ActivePlayer, AttackingCreature);
            }
        }

        //public IChoice PerformTurnBasedActions(IDuel duel)
        //{
        //    //IEnumerable<IBattleZoneCreature> creatures = duel.GetCreaturesThatCanBlock(AttackingCreature);
        //    throw new System.NotImplementedException();
        //    //return creatures.Any() ? new DeclareBlock(ActivePlayer.Opponent, creatures) : null;
        //}
    }
}
