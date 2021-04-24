using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using System.Collections.Generic;

namespace DuelMastersModels.Managers
{
    public interface IContinuousEffectManager
    {
        IDuel Duel { get; set; }
        IAbilityManager AbilityManager { get; set; }

        void AddContinuousEffect(IContinuousEffect continuousEffect);
        bool AttacksIfAble(ICreature creature);
        void EndContinuousEffects<T>();
        IEnumerable<ICreature> GetAllBlockersPlayerHasInTheBattleZone(IPlayer player);
        IEnumerable<ICreature> GetCreaturesThatCannotAttack(IPlayer player);
        IEnumerable<ICreature> GetCreaturesThatCannotAttackPlayers();
        int GetPower(ICreature creature);
        bool HasShieldTrigger(ICreature creature);
        bool HasShieldTrigger(ISpell spell);
        bool HasSpeedAttacker(ICreature creature);
    }
}