﻿using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using System;

namespace DuelMastersModels.Steps
{
    /// <summary>
    /// 508.1. If the attacking creature was declared to attack another creature or if the attack was redirected to target a creature, that creature and the attacking creature battle.
    /// </summary>
    internal class BattleStep : Step
    {
        internal Creature AttackingCreature { get; private set; }
        internal Creature AttackedCreature { get; private set; }
        internal Creature BlockingCreature { get; private set; }

        internal BattleStep(Player activePlayer, Creature attackingCreature, Creature attackedCreature, Creature blockingCreature) : base(activePlayer)
        {
            AttackingCreature = attackingCreature;
            AttackedCreature = attackedCreature;
            BlockingCreature = blockingCreature;
        }

        internal override PlayerAction PlayerActionRequired(Duel duel)
        {
            if (duel == null)
            {
                throw new ArgumentNullException(nameof(duel));
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
