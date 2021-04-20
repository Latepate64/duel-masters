using DuelMastersInterfaceModels.Events;

namespace DuelMastersInterfaceModels
{
    public class InterfaceDataWrapper
    {
        //TODO: ChoiceWrapper

        public EventWrapper Event { get; set; }

        public OtherWrapper Other { get; set; }
    }

    public enum DuelStartMode
    {
        Wait,
        First,
        Second,
        Random,
    }

    public class OtherWrapper
    {
        public string ChatMessage { get; set; }
        public string ChangeName { get; set; }
        public DuelStartMode DuelStartMode { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }
    }

    public class ConnectionInfo
    {
        public string Name { get; set; }
        public string OpponentName { get; set; }
        public DuelStartMode OpponentDuelStartMode { get; set; }
    }
}
