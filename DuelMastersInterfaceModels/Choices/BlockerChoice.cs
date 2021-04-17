using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices
{
    public class BlockerChoice : Choice
    {
        public IEnumerable<int> PossibleBlockers { get; }

        public BlockerChoice(int player) : base(player)
        {
        }
    }
}
