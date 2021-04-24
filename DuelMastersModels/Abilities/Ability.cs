using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Abilities
{
    /// <summary>
    /// An ability can be one of three things: An ability can be a characteristic an object has that lets it affect the game; An ability can be something that a player has that changes how the game affects the player.; An ability can be an activated or triggered ability on the stack.
    /// </summary>
    public abstract class Ability : IAbility
    {
        /// <summary>
        /// The source of an ability is the object that generated it. The source of an activated ability on the stack is the object whose ability was activated. The source of a triggered ability (other than a delayed triggered ability) on the stack, or one that has triggered and is waiting to be put on the stack, is the object whose ability triggered.
        /// </summary>
        public ICard Source { get; set; }
    }
}
