using DuelMastersModels.Cards;

namespace DuelMastersModels.Steps
{
    public class DirectAttackStep : Step
    {
        internal Creature AttackingCreature { get; private set; }
        //private bool _breakingDone;
        //public ReadOnlyCardCollection BrokenShields { get; private set; }

        public DirectAttackStep(Player activePlayer, Creature attackingCreature) : base(activePlayer)
        {
            AttackingCreature = attackingCreature;
        }

        public override Step GetNextStep()
        {
            return new EndOfAttackStep(ActivePlayer);
        }

        //TODO
        //public IChoice PlayerActionRequired(Duel duel)
        //{
        //    if (DirectAttack && !_breakingDone)
        //    {
        //        _breakingDone = true;
        //        if (AttackingCreature == null)
        //        {
        //            throw new InvalidOperationException();
        //        }
        //        Player opponent = ActivePlayer.Opponent;
        //        if (opponent.ShieldZone.Cards.Any())
        //        {
        //            //TODO: consider multibreaker
        //            throw new NotImplementedException();
        //            //return new BreakShields(ActivePlayer, 1, ActivePlayer.Opponent.ShieldZone.Cards, AttackingCreature);
        //        }
        //        else
        //        {
        //            // 509.1. If the nonactive player has no shields left, that player loses the game. This is a state-based action.
        //            duel.End(ActivePlayer);
        //        }
        //    }
        //    return null;
        //}
    }
}
