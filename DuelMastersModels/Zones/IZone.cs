using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;

namespace DuelMastersModels.Zones
{
    public interface IZone
    {
        IEnumerable<ICard> Cards { get; }

        ///<summary>
        /// Adds a card to the zone.
        ///</summary>
        void Add(ICard card);

        ///<summary>
        /// Removes a card from the zone.
        ///</summary>
        void Remove(ICard card);
    }
}