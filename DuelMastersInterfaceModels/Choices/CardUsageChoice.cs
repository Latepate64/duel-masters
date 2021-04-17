using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices
{
    public class CardUsageChoice : Choice, ICardUsageChoice, IAttackerChoice, IEndTurnChoice
    {
        public IEnumerable<int> UseCards { get; }
        public IEnumerable<int> AttackCreatures { get; }
        public bool TurnEndable { get; }

        public CardUsageChoice(int player) : base(player)
        {
        }
    }
}
