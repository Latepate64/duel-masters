using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player may/must select cards.
    /// </summary>
    public abstract class MultipleCardSelection : CardSelection
    {
        internal MultipleCardSelection(int player, IEnumerable<int> cards, bool optional, int maximum) : base(player, cards, optional ? 0 : maximum, maximum)
        { }

        //internal abstract void Validate(IEnumerable<TCard> cards);
    }
}
