using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using System;

namespace DuelMastersModels.Steps
{
    /// <summary>
    /// 508.1. If the attacking creature was declared to attack another creature or if the attack was redirected to target a creature, that creature and the attacking creature battle.
    /// </summary>
    public class BattleStep : Step
    {
        public GameCreature AttackingCreature { get; private set; }
        public GameCreature AttackedCreature { get; private set; }
        public GameCreature BlockingCreature { get; private set; }

        public BattleStep(Player activePlayer, GameCreature attackingCreature, GameCreature attackedCreature, GameCreature blockingCreature) : base(activePlayer)
        {
            AttackingCreature = attackingCreature;
            AttackedCreature = attackedCreature;
            BlockingCreature = blockingCreature;
        }

        public override PlayerAction PlayerActionRequired(Duel duel)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            if (AttackingCreature == null)
            {
                throw new InvalidOperationException("There should be an attacking creature.");
            }
            if (BlockingCreature != null)
            {
                duel.Battle(AttackingCreature, BlockingCreature, ActivePlayer, duel.CurrentTurn.NonActivePlayer);
            }
            else if (AttackedCreature != null)
            {
                duel.Battle(AttackingCreature, AttackedCreature, ActivePlayer, duel.CurrentTurn.NonActivePlayer);
            }
            return null;
        }
    }
}
