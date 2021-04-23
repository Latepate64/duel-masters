using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Cards
{
    public enum Race
    {
        LiquidPeople,
        BeastFolk,
    }

    public class CreatureWrapper : CardWrapper
    {
        public int Power { get;  set; }

        public List<Race> Races { get; set; }
    }
}
