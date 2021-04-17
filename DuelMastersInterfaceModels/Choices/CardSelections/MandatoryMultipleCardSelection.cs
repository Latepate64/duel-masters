using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices.CardSelections
{
    /// <summary>
    /// Player must select a number of cards.
    /// </summary>
    public abstract class MandatoryMultipleCardSelection: MultipleCardSelection
    {
        internal MandatoryMultipleCardSelection(int player, int amount, IEnumerable<int> cards) : base(player, cards, false, amount)
        {
        }

        //internal override void Validate(IEnumerable<TCard> cards)
        //{
        //    if (!(cards.Count() == MinimumSelection && !cards.Except(Cards).Any()))
        //    {
        //        throw new Exceptions.MandatoryMultipleCardSelectionException(ToString());
        //    }
        //}
    }
}
