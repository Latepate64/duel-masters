using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.StaticAbilities;
using DuelMastersModels.Abilities.TriggeredAbilities;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.Zones;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Managers
{
    public class AbilityManager
    {
        #region Public
        public bool IsAbilityBeingResolved => _abilityBeingResolved != null;

        public bool IsAbilityBeingResolvedSpellAbility => _abilityBeingResolved is SpellAbility;

        public void TriggerWhenYouPutThisCreatureIntoTheBattleZoneAbilities(Creature creature)
        {
            TriggerTriggerAbilities<WhenYouPutThisCreatureIntoTheBattleZone>(creature);
        }

        public void TriggerWheneverAnotherCreatureIsPutIntoTheBattleZoneAbilities(IEnumerable<Creature> creatures)
        {
            TriggerTriggerAbilities<WheneverAnotherCreatureIsPutIntoTheBattleZone>(creatures);
        }

        public void TriggerWheneverAPlayerCastsASpellAbilities(IEnumerable<Creature> creatures)
        {
            TriggerTriggerAbilities<WheneverAPlayerCastsASpell>(creatures);
        }

        public ICollection<ContinuousEffect> GetContinuousEffectsGeneratedByCard(Card card, Player player, BattleZone battleZone)
        {
            List<ContinuousEffect> continuousEffects = new();
            foreach (StaticAbility staticAbility in GetStaticAbilities().Where(a => a.Source == card))
            {
                continuousEffects.AddRange(player.GetContinuousEffectsGeneratedByStaticAbility(card, staticAbility, battleZone));
            }
            return continuousEffects;
        }

        public PlayerActionWithEndInformation ContinueResolution(Duel duel)
        {
            return _abilityBeingResolved.ContinueResolution(duel);
        }

        public void SetAbilityBeingResolved(NonStaticAbility ability)
        {
            _abilityBeingResolved = ability;
        }

        public void StartResolvingSpellAbility(Spell spell)
        {
            //TODO: spell may have more than one spell ability.
            SpellAbility spellAbility = GetSpellAbilities().First(a => a.Source == spell);
            SetAbilityBeingResolved(spellAbility);
        }

        public int GetSpellAbilityCount(Spell spell)
        {
            return GetSpellAbilities().Count(a => a.Source == spell);
        }

        public void RemovePendingAbility(NonStaticAbility ability)
        {
            _ = _pendingAbilities.Remove(ability);
        }

        public SelectAbilityToResolve TryGetSelectAbilityToResolve(Player player)
        {
            ReadOnlyCollection<NonStaticAbility> pendingAbilities = GetPendingAbilitiesForPlayer(player);
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
        private readonly Collection<NonStaticAbility> _pendingAbilities = new Collection<NonStaticAbility>();

        /// <summary>
        /// A non-static ability that is currently being resolved.
        /// </summary>
        private NonStaticAbility _abilityBeingResolved;

        private ReadOnlyCollection<TriggeredAbility> GetTriggerAbilities()
        {
            return new ReadOnlyTriggeredAbilityCollection(_abilities.Where(a => a is TriggeredAbility).Cast<TriggeredAbility>());
        }

        private ReadOnlyCollection<TriggeredAbility> GetTriggerAbilities<T>(Card card)
        {
            return new ReadOnlyCollection<TriggeredAbility>(GetTriggerAbilities().Where(ability => ability.Source == card && ability.TriggerCondition is T).ToList());
        }

        /// <summary>
        /// Once an ability has triggered, its controller puts it on the stack as an object that’s not a card the next time a player would receive priority.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="controller"></param>
        /// <param name="source"></param>
        private void TriggerTriggeredAbility(TriggeredAbility ability, Player controller, Card source)
        {
            _pendingAbilities.Add(ability.CreatePendingTriggeredAbility(controller, source));
        }

        private void TriggerTriggerAbilities<T>(Creature card)
        {
            foreach (TriggeredAbility ability in GetTriggerAbilities<T>(card))
            {
                TriggerTriggeredAbility(ability, ability.Controller, card);
            }
        }

        private void TriggerTriggerAbilities<T>(IEnumerable<Creature> creatures)
        {
            foreach (Creature creature in creatures)
            {
                TriggerTriggerAbilities<T>(creature);
            }
        }

        private ReadOnlyStaticAbilityCollection GetStaticAbilities()
        {
            return new ReadOnlyStaticAbilityCollection(_abilities.Where(a => a is StaticAbility).Cast<StaticAbility>());
        }

        private ReadOnlySpellAbilityCollection GetSpellAbilities()
        {
            return new ReadOnlySpellAbilityCollection(_abilities.Where(a => a is SpellAbility).Cast<SpellAbility>());
        }

        /// <summary>
        /// Non-static abilities controlled by a player that are waiting to be resolved.
        /// </summary>
        private ReadOnlyCollection<NonStaticAbility> GetPendingAbilitiesForPlayer(Player player)
        {
            return new ReadOnlyCollection<NonStaticAbility>(_pendingAbilities.Where(a => a.Controller == player).ToList());
        }
        #endregion Private
    }
}
