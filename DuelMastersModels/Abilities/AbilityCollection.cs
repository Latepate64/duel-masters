using System.Collections.Generic;

namespace DuelMastersModels.Abilities
{
    internal class AbilityCollection : ReadOnlyAbilityCollection
    {
        internal AbilityCollection() : base(new List<Ability>())
        {
        }

        internal void Add(Ability ability)
        {
            Items.Add(ability);
        }
    }
}
