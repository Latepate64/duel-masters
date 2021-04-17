using System.Collections.Generic;
using System.Linq;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player may select up to one card.
    /// </summary>
    public abstract class OptionalCardSelection : SingleCardSelection
    {
        internal OptionalCardSelection(int player, IEnumerable<int> cards) : base(player, cards, true)
        { }

        //internal override void Validate(TCard card)
        //{
        //    if (!(card == null || Cards.Contains(card)))
        //    {
        //        throw new Exceptions.OptionalCardSelectionException(ToString());
        //    }
        //}
    }
}
