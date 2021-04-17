using DuelMastersInterfaceModels.Events;
using System;

namespace DuelMastersModels.Managers
{
    public interface IEventManager
    {
        event EventHandler<DuelEvent> EventRaised;
    }
}
