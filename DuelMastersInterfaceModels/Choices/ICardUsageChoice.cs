using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices
{
    public interface ICardUsageChoice
    {
        IEnumerable<int> UseCards { get; }
    }
}