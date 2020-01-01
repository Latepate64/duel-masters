﻿namespace DuelMastersModels.GameActions.TurnBasedActions
{
    internal class UntapCards : TurnBasedAction
    {
        internal override void Perform(Duel duel)
        {
            if (duel == null)
            {
                throw new System.ArgumentNullException("duel");
            }
            foreach (Cards.Creature creature in duel.CurrentTurn.ActivePlayer.BattleZone.Creatures)
            {
                creature.SummoningSickness = false;
            }
            duel.CurrentTurn.ActivePlayer.BattleZone.UntapCards();
            duel.CurrentTurn.ActivePlayer.ManaZone.UntapCards();
        }
    }
}