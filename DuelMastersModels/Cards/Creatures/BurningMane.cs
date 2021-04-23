using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Cards.Creatures
{
    public class BurningMane : Creature
    {
        public BurningMane() : base(CardIdentifier.BurningMane, 2, Civilization.Nature, 2000, Race.BeastFolk)
        {
        }
    }
}
