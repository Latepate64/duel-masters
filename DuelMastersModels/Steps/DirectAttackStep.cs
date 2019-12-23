using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.CardSelections;
using System;

namespace DuelMastersModels.Steps
{
    public class DirectAttackStep : Step
    {
        public GameCreature AttackingCreature { get; private set; }
        public bool DirectAttack { get; private set; }
        private bool _breakingDone;
        //public ReadOnlyCardCollection BrokenShields { get; private set; }

        public DirectAttackStep(Player activePlayer, GameCreature attackingCreature, bool directAttack) : base(activePlayer)
        {
            AttackingCreature = attackingCreature;
            DirectAttack = directAttack;
        }

        public override PlayerAction PlayerActionRequired(Duel duel)
        {
            if (DirectAttack && !_breakingDone)
            {
                _breakingDone = true;
                if (AttackingCreature == null)
                {
                    throw new InvalidOperationException();
                }
                Player opponent = duel.GetOpponent(ActivePlayer);
                if (opponent.ShieldZone.Cards.Count > 0)
                {
                    //TODO: consider multibreaker
                    return new BreakShields(ActivePlayer, 1, new ReadOnlyCardCollection(duel.GetOpponent(ActivePlayer).ShieldZone.Cards), AttackingCreature);
                }
                else
                {
                    // 509.1. If the nonactive player has no shields left, that player loses the game. This is a state-based action.
                    duel.End(ActivePlayer);
                }
            }
            return null;
        }
    }
}
