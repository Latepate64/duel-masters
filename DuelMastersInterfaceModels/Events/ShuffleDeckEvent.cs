namespace DuelMastersInterfaceModels.Events
{
    public class ShuffleDeckEvent : DuelEvent
    {
        public int PlayerID { get; }

        public ShuffleDeckEvent(int playerID)
        {
            PlayerID = playerID;
        }
    }
}
