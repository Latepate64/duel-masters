using System.Collections.Generic;
using System.Linq;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player must select a card.
    /// </summary>
    public abstract class MandatoryCardSelection : SingleCardSelection
    {
        internal MandatoryCardSelection(int player, IEnumerable<int> cards) : base(player, cards, false)
        { }

        //internal override void Validate(TCard card)
        //{
        //    if (!Cards.Contains(card))
        //    {
        //        throw new Exceptions.MandatoryCardSelectionException(ToString());
        //    }
        //}
    }
}