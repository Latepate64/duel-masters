using System.Collections.Generic;
using System.Linq;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player selects up to a number of cards.
    /// </summary>
    public abstract class OptionalMultipleCardSelection : MultipleCardSelection
    {
        internal OptionalMultipleCardSelection(int player, IEnumerable<int> cards) : base(player, cards, false, cards.Count())
        { }

        //internal override void Validate(IEnumerable<TCard> cards)
        //{
        //    if (cards.Except(Cards).Any())
        //    {
        //        throw new Exceptions.MultipleCardSelectionException(ToString());
        //    }
        //}
    }
}