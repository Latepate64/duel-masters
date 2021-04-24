using DuelMastersModels.Abilities;
using DuelMastersModels.Cards;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.Zones;
using System.Collections.Generic;

namespace DuelMastersModels.Managers
{
    public interface IAbilityManager
    {
        bool IsAbilityBeingResolved { get; }
        bool IsAbilityBeingResolvedSpellAbility { get; }

        PlayerActionWithEndInformation ContinueResolution(IDuel duel);
        ICollection<IContinuousEffect> GetContinuousEffectsGeneratedByCard(ICard card, IPlayer player, BattleZone battleZone);
        int GetSpellAbilityCount(ISpell spell);
        void RemovePendingAbility(INonStaticAbility ability);
        void SetAbilityBeingResolved(INonStaticAbility ability);
        void StartResolvingSpellAbility(ISpell spell);
        void TriggerWheneverAnotherCreatureIsPutIntoTheBattleZoneAbilities(IEnumerable<ICreature> creatures);
        void TriggerWheneverAPlayerCastsASpellAbilities(IEnumerable<ICreature> creatures);
        void TriggerWhenYouPutThisCreatureIntoTheBattleZoneAbilities(ICreature creature);
        SelectAbilityToResolve TryGetSelectAbilityToResolve(IPlayer player);
    }
}