using DuelMastersInterfaceModels.Events;
using System.Collections.Generic;

namespace DuelMastersModels.Managers
{
    public interface IEventManager
    {
        Queue<DuelEvent> Events { get; set; }
        Queue<DuelEvent> NewEvents { get; set; }
    }
}
