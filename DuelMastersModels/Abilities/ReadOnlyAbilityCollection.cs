using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Abilities
{
    internal class ReadOnlyAbilityCollection : ReadOnlyCollection<Ability>
    {
        internal ReadOnlyAbilityCollection(IEnumerable<Ability> abilities) : base(abilities.ToList()) { }
    }
}
