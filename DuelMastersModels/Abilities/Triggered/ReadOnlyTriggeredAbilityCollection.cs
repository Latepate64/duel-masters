using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Abilities.TriggeredAbilities
{
    internal class ReadOnlyTriggeredAbilityCollection : ReadOnlyCollection<TriggeredAbility>
    {
        internal ReadOnlyTriggeredAbilityCollection(IEnumerable<TriggeredAbility> abilities) : base(abilities.ToList()) { }
    }
}
