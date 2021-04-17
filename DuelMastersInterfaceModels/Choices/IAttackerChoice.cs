using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices
{
    public interface IAttackerChoice
    {
        IEnumerable<int> AttackCreatures { get; }
    }
}