namespace DuelMastersModels.Effects.OneShotEffects
{
    /// <summary>
    /// 610.1. A one-shot effect does something just once and doesn’t have a duration.
    /// </summary>
    public abstract class OneShotEffect : Effect
    {
        internal abstract DuelMastersInterfaceModels.Choices.IChoice Apply(Duel duel, Player player);
    }
}
