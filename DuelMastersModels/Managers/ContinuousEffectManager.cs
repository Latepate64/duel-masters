﻿using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Managers
{
    public class ContinuousEffectManager : IContinuousEffectManager
    {
        public void AddContinuousEffect(IContinuousEffect continuousEffect)
        {
            _continuousEffects.Add(continuousEffect);
        }

        public void EndContinuousEffects<T>()
        {
            //TODO: Test
            _ = _continuousEffects.ToList().RemoveAll(c => c.Period is T);
            /*
            List<ContinuousEffect> suitableContinuousEffects = _continuousEffects.Where(c => c.Period.GetType() == type).ToList();
            while (suitableContinuousEffects.Count() > 0)
            {
                _continuousEffects.Remove(suitableContinuousEffects.First());
                suitableContinuousEffects.Remove(suitableContinuousEffects.First());
            }
            */
        }

        public IEnumerable<IBattleZoneCreature> GetAllBlockersPlayerHasInTheBattleZone(IPlayer player, IDuel duel, IAbilityManager abilityManager)
        {
            List<IBattleZoneCreature> blockers = new List<IBattleZoneCreature>();
            IEnumerable<BlockerEffect> blockerEffects = GetContinuousEffects<BlockerEffect>(duel, abilityManager);
            foreach (IBattleZoneCreature creature in player.BattleZone.Creatures)
            {
                blockers.AddRange(blockerEffects.Where(blockerEffect => blockerEffect.CreatureFilter.FilteredCreatures.Contains(creature)).Select(blockerEffect => creature));
            }
            return new ReadOnlyCollection<IBattleZoneCreature>(blockers);
        }

        public bool HasSpeedAttacker(IDuel duel, IAbilityManager abilityManager, IBattleZoneCreature creature)
        {
            return GetContinuousEffects<SpeedAttackerEffect>(duel, abilityManager).Any(e => e.CreatureFilter.FilteredCreatures.Contains(creature));
        }

        public bool HasShieldTrigger(IDuel duel, IAbilityManager abilityManager, ISpell spell)
        {
            foreach (SpellContinuousEffect spellContinuousEffect in GetContinuousEffects(duel, abilityManager).Where(e => e is SpellShieldTriggerEffect).Cast<SpellShieldTriggerEffect>())
            {
                if (spellContinuousEffect.SpellFilter.FilteredSpells.Contains(spell))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasShieldTrigger(IDuel duel, IAbilityManager abilityManager, IHandCreature creature)
        {
            foreach (CreatureShieldTriggerEffect creatureContinuousEffect in GetContinuousEffects(duel, abilityManager).OfType<CreatureShieldTriggerEffect>())
            {
                if (creatureContinuousEffect.CreatureFilter.FilteredCreatures.Contains(creature))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetPower(IDuel duel, IAbilityManager abilityManager, IBattleZoneCreature creature)
        {
            return creature.Power + GetContinuousEffects<PowerEffect>(duel, abilityManager).Where(e => e.CreatureFilter.FilteredCreatures.Contains(creature)).Sum(e => e.Power);
        }

        public IEnumerable<IBattleZoneCreature> GetCreaturesThatCannotAttack(IDuel duel, IAbilityManager abilityManager, IPlayer player)
        {
            return new ReadOnlyCollection<IBattleZoneCreature>(GetContinuousEffects<CannotAttackPlayersEffect>(duel, abilityManager).SelectMany(e => e.CreatureFilter.FilteredCreatures).Distinct().Where(c => !duel.GetCreaturesThatCanBeAttacked(player).Any()).ToList());
        }

        public bool AttacksIfAble(IDuel duel, IAbilityManager abilityManager, IBattleZoneCreature creature)
        {
            return GetContinuousEffects<AttacksIfAbleEffect>(duel, abilityManager).Any(e => e.CreatureFilter.FilteredCreatures.Contains(creature));
        }

        public IEnumerable<IBattleZoneCreature> GetCreaturesThatCannotAttackPlayers(IDuel duel, IAbilityManager abilityManager)
        {
            return new ReadOnlyCollection<IBattleZoneCreature>(GetContinuousEffects<CannotAttackPlayersEffect>(duel, abilityManager).SelectMany(e => e.CreatureFilter.FilteredCreatures).Distinct().ToList());
        }

        /// <summary>
        /// Continuous effects that are generated by non-static abilities. Use method GetContinuousEffects() to obtain all continuous effects generated by non-static and static abilities.
        /// </summary>
        private readonly Collection<IContinuousEffect> _continuousEffects = new Collection<IContinuousEffect>();

        private IEnumerable<T> GetContinuousEffects<T>(IDuel duel, IAbilityManager abilityManager)
        {
            return GetContinuousEffects(duel, abilityManager).Where(e => e is T).Cast<T>();
        }

        private ReadOnlyContinuousEffectCollection GetContinuousEffects(IDuel duel, IAbilityManager abilityManager)
        {
            List<IContinuousEffect> continuousEffects = _continuousEffects.ToList();
            foreach (Card card in duel.GetAllCards())
            {
                continuousEffects.AddRange(abilityManager.GetContinuousEffectsGeneratedByCard(card, card.Owner));
            }
            return new ReadOnlyContinuousEffectCollection(continuousEffects);
        }
    }
}
