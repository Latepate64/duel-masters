using DuelMastersInterfaceModels.Cards;
using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public class AttackDeclarationStep : TurnBasedActionStep
    {
        internal ICreature AttackingCreature { get; set; }
        internal ICreature AttackedCreature { get; set; }

        public AttackDeclarationStep(IPlayer activePlayer) : base(activePlayer)
        {
        }

        public override IChoice PerformTurnBasedAction()
        {
            // TODO: Check if there are creatures that can attack
            bool possibleToAttack = true;
            if (possibleToAttack)
            {
                return new AttackerChoice(ActivePlayer.ID);
            }
            else
            {
                return null;
            }
        }

        public override IStep GetNextStep()
        {
            if (AttackingCreature != null)
            {
                return new BlockDeclarationStep(ActivePlayer, AttackingCreature, AttackedCreature);
            }
            // 506.2. If an attacking creature is not specified, the other substeps are skipped.
            else
            {
                return new EndOfTurnStep(ActivePlayer);
            }
        }

        public IChoice DeclareAttackOnCreature(ICreature attacker, ICreature target)
        {
            AttackingCreature = attacker;
            AttackedCreature = target;
            return Proceed();
        }

        public IChoice DeclareAttackOnOpponent(ICreature attacker)
        {
            AttackingCreature = attacker;
            return Proceed();
        }

        //TODO
        //public override IChoice PlayerActionRequired(IDuel duel)
        //{
        //    if (AttackingCreature != null && !TargetOfAttackDeclared)
        //    {
        //        //TODO: If attacked creature is not null, check that it can be attacked.
        //        throw new System.NotImplementedException();
        //        //return new DeclareTargetOfAttack(ActivePlayer, duel.GetCreaturesThatCanBeAttacked(ActivePlayer));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public IChoice PerformTurnBasedActions(IDuel duel)
        //{
        //    //IEnumerable<IBattleZoneCreature> creatures = duel.GetCreaturesThatCanAttack(ActivePlayer);
        //    throw new System.NotImplementedException();
        //    //return creatures.Any()
        //    //    ? creatures.Any(creature => duel.AttacksIfAble(creature))
        //    //        ? new DeclareAttackerMandatory(ActivePlayer, creatures)
        //    //        : (Choice)new DeclareAttacker(ActivePlayer, creatures)
        //    //    : null;
        //}
    }
}
