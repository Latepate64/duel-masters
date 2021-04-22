using DuelMastersInterfaceModels.Events;
using System.Collections.Generic;

namespace DuelMastersInterfaceModels
{
    public class InterfaceDataWrapper
    {
        //TODO: ChoiceWrapper

        public List<EventWrapper> Events { get; set; }

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
        public PlayerWrapper PlayerWrapper { get; set; }
    }

    public class ConnectionInfo
    {
        public string Name { get; set; }
        public string OpponentName { get; set; }
        public DuelStartMode OpponentDuelStartMode { get; set; }
    }

    public class PlayerInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class PlayerWrapper
    {
        public PlayerInfo Player { get; set; }
        public PlayerInfo Opponent { get; set; }
    }
}
