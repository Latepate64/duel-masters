﻿using System.Xml.Serialization;

namespace DuelMastersInterfaceModels
{
    [XmlRoot(ElementName = "Data")]
    public class InterfaceDataWrapper
    {
        //TODO: ChoiceWrapper
        //TODO: EventWrapper
        [XmlElement("Other")]
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
    }
}
