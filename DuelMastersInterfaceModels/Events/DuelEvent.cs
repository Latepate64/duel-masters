using System;

namespace DuelMastersInterfaceModels.Events
{
    /// <summary>
    /// Anything that happens in a game is an event. Multiple events may take place during the resolution of a spell or ability. The text of triggered abilities and replacement effects defines the event they’re looking for. One “happening” may be treated as a single event by one ability and as multiple events by another.
    /// </summary>
    public abstract class DuelEvent : EventArgs
    {
    }
}
