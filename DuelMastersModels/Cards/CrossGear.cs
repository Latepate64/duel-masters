using System.Collections.ObjectModel;

namespace DuelMastersModels.Cards
{
    public class CrossGear : Card
    {
        public CrossGear() : base()
        {
        }

        /// <summary>
        /// Creates a cross gear.
        /// </summary>
        public CrossGear(string name, string set, string id, Collection<string> civilizations, string rarity, int cost, string text, string flavor, string illustrator, int gameId) : base(name, set, id, civilizations, rarity, cost, text, flavor, illustrator)
        {
        }
    }
}
