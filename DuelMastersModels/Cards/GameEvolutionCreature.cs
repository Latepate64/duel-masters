namespace DuelMastersModels.Cards
{
    public class GameEvolutionCreature : GameCreature
    {
        public GameEvolutionCreature(EvolutionCreature evolutionCreature, int gameId, Player owner) : base(evolutionCreature, gameId, owner)
        {
        }
    }
}
