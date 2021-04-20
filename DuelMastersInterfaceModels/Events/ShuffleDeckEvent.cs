namespace DuelMastersInterfaceModels.Events
{
    public class ShuffleDeckEvent : DuelEvent
    {
        public int PlayerID { get; set; }

        public ShuffleDeckEvent(int playerID)
        {
            PlayerID = playerID;
        }
    }
}
