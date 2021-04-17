using System.Collections.ObjectModel;

namespace DuelMastersInterfaceModels.Choices
{
    /// <summary>
    /// If there are pending abilities, the active player chooses which one to resolve first among their pending abilities in any order. It does not matter in which order the abilities became pending abilities.
    /// </summary>
    public class SelectAbilityToResolve : Choice
    {
        /// <summary>
        /// If there are pending abilities, the active player chooses which one to resolve first among their pending abilities in any order. It does not matter in which order the abilities became pending abilities.
        /// </summary>
        //public ReadOnlyCollection<INonStaticAbility> Abilities { get; private set; }

        //internal INonStaticAbility SelectedAbility { get; set; }

        public SelectAbilityToResolve(int player/*, ReadOnlyCollection<INonStaticAbility> abilities*/) : base(player)
        {
            //Abilities = abilities;
        }
    }
}
