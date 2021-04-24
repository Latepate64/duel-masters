using DuelMastersInterfaceModels.Cards;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.StaticAbilities;
using DuelMastersModels.Abilities.TriggeredAbilities;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.Zones;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Managers
{
    public class AbilityManager : IAbilityManager
    {
        #region Public
        public bool IsAbilityBeingResolved => _abilityBeingResolved != null;

        public bool IsAbilityBeingResolvedSpellAbility => _abilityBeingResolved is SpellAbility;

        public void TriggerWhenYouPutThisCreatureIntoTheBattleZoneAbilities(ICreature creature)
        {
            TriggerTriggerAbilities<WhenYouPutThisCreatureIntoTheBattleZone>(creature);
        }

        public void TriggerWheneverAnotherCreatureIsPutIntoTheBattleZoneAbilities(IEnumerable<ICreature> creatures)
        {
            TriggerTriggerAbilities<WheneverAnotherCreatureIsPutIntoTheBattleZone>(creatures);
        }

        public void TriggerWheneverAPlayerCastsASpellAbilities(IEnumerable<ICreature> creatures)
        {
            TriggerTriggerAbilities<WheneverAPlayerCastsASpell>(creatures);
        }

        public ICollection<IContinuousEffect> GetContinuousEffectsGeneratedByCard(ICard card, IPlayer player, BattleZone battleZone)
        {
            List<IContinuousEffect> continuousEffects = new();
            foreach (IStaticAbility staticAbility in GetStaticAbilities().Where(a => a.Source == card))
            {
                continuousEffects.AddRange(player.GetContinuousEffectsGeneratedByStaticAbility(card, staticAbility, battleZone));
            }
            return continuousEffects;
        }

        public PlayerActionWithEndInformation ContinueResolution(IDuel duel)
        {
            return _abilityBeingResolved.ContinueResolution(duel);
        }

        public void SetAbilityBeingResolved(INonStaticAbility ability)
        {
            _abilityBeingResolved = ability;
        }

        public void StartResolvingSpellAbility(ISpell spell)
        {
            //TODO: spell may have more than one spell ability.
            SpellAbility spellAbility = GetSpellAbilities().First(a => a.Source == spell);
            SetAbilityBeingResolved(spellAbility);
        }

        public int GetSpellAbilityCount(ISpell spell)
        {
            return GetSpellAbilities().Count(a => a.Source == spell);
        }

        public void RemovePendingAbility(INonStaticAbility ability)
        {
            _ = _pendingAbilities.Remove(ability);
        }

        public SelectAbilityToResolve TryGetSelectAbilityToResolve(IPlayer player)
        {
            ReadOnlyCollection<INonStaticAbility> pendingAbilities = GetPendingAbilitiesForPlayer(player);
            return pendingAbilities.Count > 0 ? new SelectAbilityToResolve(player.ID/*, pendingAbilities*/) : null;
        }
        #endregion Public

        #region Private
        /// <summary>
        /// An ability can be a characteristic an card has that lets it affect the game. A card's abilities are defined by its rules text or by the effect that created it.
        /// </summary>
        private readonly AbilityCollection _abilities = new AbilityCollection();

        /// <summary>
        /// Non-static abilities that are waiting to be resolved.
        /// </summary>
        private readonly Collection<INonStaticAbility> _pendingAbilities = new Collection<INonStaticAbility>();

        /// <summary>
        /// A non-static ability that is currently being resolved.
        /// </summary>
        private INonStaticAbility _abilityBeingResolved;

        private ReadOnlyCollection<ITriggeredAbility> GetTriggerAbilities()
        {
            return new ReadOnlyTriggeredAbilityCollection(_abilities.Where(a => a is ITriggeredAbility).Cast<ITriggeredAbility>());
        }

        private ReadOnlyCollection<ITriggeredAbility> GetTriggerAbilities<T>(ICard card)
        {
            return new ReadOnlyCollection<ITriggeredAbility>(GetTriggerAbilities().Where(ability => ability.Source == card && ability.TriggerCondition is T).ToList());
        }

        /// <summary>
        /// Once an ability has triggered, its controller puts it on the stack as an object that’s not a card the next time a player would receive priority.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="controller"></param>
        /// <param name="source"></param>
        private void TriggerTriggeredAbility(ITriggeredAbility ability, IPlayer controller, ICard source)
        {
            _pendingAbilities.Add(ability.CreatePendingTriggeredAbility(controller, source));
        }

        private void TriggerTriggerAbilities<T>(ICreature card)
        {
            foreach (TriggeredAbility ability in GetTriggerAbilities<T>(card))
            {
                TriggerTriggeredAbility(ability, ability.Controller, card);
            }
        }

        private void TriggerTriggerAbilities<T>(IEnumerable<ICreature> creatures)
        {
            foreach (ICreature creature in creatures)
            {
                TriggerTriggerAbilities<T>(creature);
            }
        }

        private ReadOnlyStaticAbilityCollection GetStaticAbilities()
        {
            return new ReadOnlyStaticAbilityCollection(_abilities.Where(a => a is IStaticAbility).Cast<IStaticAbility>());
        }

        private ReadOnlySpellAbilityCollection GetSpellAbilities()
        {
            return new ReadOnlySpellAbilityCollection(_abilities.Where(a => a is SpellAbility).Cast<SpellAbility>());
        }

        /// <summary>
        /// Non-static abilities controlled by a player that are waiting to be resolved.
        /// </summary>
        private ReadOnlyCollection<INonStaticAbility> GetPendingAbilitiesForPlayer(IPlayer player)
        {
            return new ReadOnlyCollection<INonStaticAbility>(_pendingAbilities.Where(a => a.Controller == player).ToList());
        }
        #endregion Private
    }
}
