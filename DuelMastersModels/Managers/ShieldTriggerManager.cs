using DuelMastersModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Managers
{
    internal class ShieldTriggerManager
    {
        internal IEnumerable<ICard> ShieldTriggersToUse => new ReadOnlyCollection<ICard>(_shieldTriggersToUse);

        internal void AddShieldTriggerToUse(ICard card)
        {
            _shieldTriggersToUse.Add(card);
        }

        internal void RemoveShieldTriggerToUse(ICard card)
        {
            _ = _shieldTriggersToUse.Remove(card);
        }

        private readonly Collection<ICard> _shieldTriggersToUse = new Collection<ICard>();
    }
}
