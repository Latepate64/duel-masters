namespace DuelMastersInterfaceModels.Events
{
    public class EventWrapper
    {
        public int PlayerID { get; set; }
        public ShuffleDeckEvent ShuffleDeckEvent { get; set; }
        public DeckTopCardToShieldEvent DeckTopCardToShieldEvent { get; set; }
        public DrawCardEvent DrawCardEvent { get; set; }
        public TurnStartEvent TurnStartEvent { get; set; }
    }
}
