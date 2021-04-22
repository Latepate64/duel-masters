using DuelMastersInterfaceModels.Events;
using System.Collections.Generic;

namespace DuelMastersModels.Managers
{
    public class EventManager : IEventManager
    {
        public Queue<DuelEvent> Events { get; set; } = new Queue<DuelEvent>();
        public Queue<DuelEvent> NewEvents { get; set; } = new Queue<DuelEvent>();

        internal void Raise(DuelEvent duelEvent)
        {
            Events.Enqueue(duelEvent);
            NewEvents.Enqueue(duelEvent);
        }
    }
}
