using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player may/must select a card.
    /// </summary>
    public abstract class SingleCardSelection : CardSelection
    {
        internal SingleCardSelection(int player, IEnumerable<int> cards, bool optional) : base(player, cards, optional ? 0 : 1, 1)
        { }

        //internal abstract void Validate(TCard card);
    }
}
