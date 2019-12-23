namespace DuelMastersModels.PlayerActions.AutomaticActions
{
    public class TapAllYourOpponentsCreaturesInTheBattleZone : AutomaticAction
    {
        public TapAllYourOpponentsCreaturesInTheBattleZone(Player player) : base(player) { }

        public override PlayerAction Perform(Duel duel)
        {
            foreach (Cards.GameCreature creature in duel.GetOpponent(Player).BattleZone.UntappedCreatures)
            {
                creature.Tapped = true;
            }
            return null;
        }
    }
}