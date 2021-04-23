using DuelMastersInterfaceModels.Cards;

namespace DuelMastersInterfaceModels.Events
{
    /// <summary>
    /// Player drew a card.
    /// </summary>
    public class DrawCardEvent : PlayerEvent
	{
        public CardWrapper Card { get; set; }
    }
}
