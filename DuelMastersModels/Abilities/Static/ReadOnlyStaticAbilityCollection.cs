using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    internal class ReadOnlyStaticAbilityCollection : ReadOnlyCollection<StaticAbility>
    {
        internal ReadOnlyStaticAbilityCollection(IEnumerable<StaticAbility> abilities) : base(abilities.ToList()) { }
    }
}
