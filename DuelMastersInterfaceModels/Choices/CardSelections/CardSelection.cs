using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player selects cards.
    /// </summary>
    public abstract class CardSelection : Choice
    {
        /// <summary>
        /// Cards player can select from.
        /// </summary>
        public IEnumerable<int> Cards { get; private set; }

        internal int MinimumSelection { get; set; }

        internal int MaximumSelection { get; set; }

        internal CardSelection(int player, IEnumerable<int> cards, int minimumSelection, int maximumSelection) : base(player)
        {
            MinimumSelection = minimumSelection;
            MaximumSelection = maximumSelection;
            Cards = cards;
        }
    }
}
