using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices
{
    public class AttackerChoice : Choice, IAttackerChoice, IEndTurnChoice
    {
        public IEnumerable<int> AttackCreatures { get; }
        public bool TurnEndable { get; }

        public AttackerChoice(int player) : base(player)
        {
        }
    }
}
