﻿using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Managers
{
    public class ContinuousEffectManager : IContinuousEffectManager
    {
        public IAbilityManager AbilityManager { get; set; }
        public IDuel Duel { get; set; }

        public ContinuousEffectManager(IDuel duel, IAbilityManager abilityManager)
        {
            Duel = duel;
            AbilityManager = abilityManager;
        }

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

        public IEnumerable<IBattleZoneCreature> GetAllBlockersPlayerHasInTheBattleZone(IPlayer player)
        {
            List<IBattleZoneCreature> blockers = new List<IBattleZoneCreature>();
            IEnumerable<BlockerEffect> blockerEffects = GetContinuousEffects<BlockerEffect>();
            foreach (IBattleZoneCreature creature in Duel.BattleZone.Creatures)
            {
                blockers.AddRange(blockerEffects.Where(blockerEffect => blockerEffect.CreatureFilter.FilteredCreatures.Contains(creature)).Select(blockerEffect => creature));
            }
            return new ReadOnlyCollection<IBattleZoneCreature>(blockers);
        }

        public bool HasSpeedAttacker(IBattleZoneCreature creature)
        {
            return GetContinuousEffects<SpeedAttackerEffect>().Any(e => e.CreatureFilter.FilteredCreatures.Contains(creature));
        }

        public bool HasShieldTrigger(ISpell spell)
        {
            foreach (SpellContinuousEffect spellContinuousEffect in GetContinuousEffects().Where(e => e is SpellShieldTriggerEffect).Cast<SpellShieldTriggerEffect>())
            {
                if (spellContinuousEffect.SpellFilter.FilteredSpells.Contains(spell))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasShieldTrigger(IHandCreature creature)
        {
            foreach (CreatureShieldTriggerEffect creatureContinuousEffect in GetContinuousEffects().OfType<CreatureShieldTriggerEffect>())
            {
                if (creatureContinuousEffect.CreatureFilter.FilteredCreatures.Contains(creature))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetPower(IBattleZoneCreature creature)
        {
            return creature.Power + GetContinuousEffects<PowerEffect>().Where(e => e.CreatureFilter.FilteredCreatures.Contains(creature)).Sum(e => e.Power);
        }

        public IEnumerable<IBattleZoneCreature> GetCreaturesThatCannotAttack(IPlayer player)
        {
            return new ReadOnlyCollection<IBattleZoneCreature>(GetContinuousEffects<CannotAttackPlayersEffect>().SelectMany(e => e.CreatureFilter.FilteredCreatures).Distinct().Where(c => !Duel.GetCreaturesThatCanBeAttacked(player).Any()).ToList());
        }

        public bool AttacksIfAble(IBattleZoneCreature creature)
        {
            return GetContinuousEffects<AttacksIfAbleEffect>().Any(e => e.CreatureFilter.FilteredCreatures.Contains(creature));
        }

        public IEnumerable<IBattleZoneCreature> GetCreaturesThatCannotAttackPlayers()
        {
            return new ReadOnlyCollection<IBattleZoneCreature>(GetContinuousEffects<CannotAttackPlayersEffect>().SelectMany(e => e.CreatureFilter.FilteredCreatures).Distinct().ToList());
        }

        /// <summary>
        /// Continuous effects that are generated by non-static abilities. Use method GetContinuousEffects() to obtain all continuous effects generated by non-static and static abilities.
        /// </summary>
        private readonly Collection<IContinuousEffect> _continuousEffects = new Collection<IContinuousEffect>();

        private IEnumerable<T> GetContinuousEffects<T>()
        {
            return GetContinuousEffects().Where(e => e is T).Cast<T>();
        }

        private ReadOnlyContinuousEffectCollection GetContinuousEffects()
        {
            List<IContinuousEffect> continuousEffects = _continuousEffects.ToList();
            foreach (ICard card in Duel.GetAllCards())
            {
                continuousEffects.AddRange(AbilityManager.GetContinuousEffectsGeneratedByCard(card, card.Owner, Duel.BattleZone));
            }
            return new ReadOnlyContinuousEffectCollection(continuousEffects);
        }
    }
}
