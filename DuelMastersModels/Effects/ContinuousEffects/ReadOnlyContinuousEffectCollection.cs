using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    public class ReadOnlyContinuousEffectCollection : ReadOnlyCollection<ContinuousEffect>
    {
        internal ReadOnlyContinuousEffectCollection() : base(new List<ContinuousEffect>())
        {
        }

        internal ReadOnlyContinuousEffectCollection(IEnumerable<ContinuousEffect> continuousEffects) : base(continuousEffects.ToList()) { }

        internal ReadOnlyContinuousEffectCollection(ContinuousEffect continuousEffect) : base(new List<ContinuousEffect>() { continuousEffect }) { }
    }
}
