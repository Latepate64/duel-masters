using System.Collections.ObjectModel;

namespace DuelMastersModels.Cards
{
    public class EvolutionCreature : Creature
    {
        public EvolutionCreature() : base()
        {
        }

        /// <summary>
        /// Creates an evolution creature.
        /// </summary>
        public EvolutionCreature(string name, string set, string id, Collection<string> civilizations, string rarity, int cost, string text, string flavor, string illustrator, string power, Collection<string> races) : base(name, set, id, civilizations, rarity, cost, text, flavor, illustrator, power, races)
        {
        }
    }
}
