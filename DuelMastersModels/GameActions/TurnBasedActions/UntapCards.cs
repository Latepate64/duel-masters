namespace DuelMastersModels.GameActions.TurnBasedActions
{
    public class UntapCards : TurnBasedAction
    {
        public override void Perform(Duel duel)
        {
            if (duel == null)
            {
                throw new System.ArgumentNullException("duel");
            }
            foreach (Cards.GameCreature creature in duel.CurrentTurn.ActivePlayer.BattleZone.Creatures)
            {
                creature.SummoningSickness = false;
            }
            duel.CurrentTurn.ActivePlayer.BattleZone.UntapCards();
            duel.CurrentTurn.ActivePlayer.ManaZone.UntapCards();
        }
    }
}