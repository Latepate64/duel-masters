using DuelMastersInterfaceModels.Events;
using System;

namespace DuelMastersModels.Managers
{
    public class EventManager : IEventManager
    {
        public event EventHandler<DuelEvent> EventRaised;

        internal void Raise(DuelEvent duelEvent)
        {
            EventRaised(this, duelEvent);
        }
    }
}
