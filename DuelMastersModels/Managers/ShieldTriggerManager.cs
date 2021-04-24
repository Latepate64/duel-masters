using DuelMastersModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Managers
{
    internal class ShieldTriggerManager
    {
        internal IEnumerable<Card> ShieldTriggersToUse => new ReadOnlyCollection<Card>(_shieldTriggersToUse);

        internal void AddShieldTriggerToUse(Card card)
        {
            _shieldTriggersToUse.Add(card);
        }

        internal void RemoveShieldTriggerToUse(Card card)
        {
            _ = _shieldTriggersToUse.Remove(card);
        }

        private readonly Collection<Card> _shieldTriggersToUse = new Collection<Card>();
    }
}
