using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Abilities.Trigger;
using DuelMastersModels.Effects.OneShotEffects;
using DuelMastersModels.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Cards
{
    public class GameCreature : GameCard
    {
        public int Power { get; set; }
        public Collection<string> Races => (BaseCard as Creature).Races;

        private bool _summoningSickness = true;
        public bool SummoningSickness
        {
            get => _summoningSickness;
            set
            {
                if (value != _summoningSickness)
                {
                    _summoningSickness = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public GameCreature(Creature creature, int gameId, Player owner) : base(creature, gameId)
        {
            Power = (BaseCard as Creature).Power;

            if (!string.IsNullOrEmpty(Text))
            {
                IEnumerable<string> textParts = Text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries).Where(t => !(t.StartsWith("(", System.StringComparison.CurrentCulture) && t.EndsWith(")", System.StringComparison.CurrentCulture)));
                foreach (string textPart in textParts)
                {
                    StaticAbility staticAbility = StaticAbilityFactory.ParseStaticAbilityForCreature(textPart, this);
                    if (staticAbility != null)
                    {
                        Abilities.Add(staticAbility);
                    }
                    else
                    {
                        Abilities.Ability nonStaticAbility = ParseTriggerAbility(owner, textPart);
                        if (nonStaticAbility != null)
                        {
                            Abilities.Add(nonStaticAbility);
                        }
                    }
                }
            }
        }

        private Abilities.Ability ParseTriggerAbility(Player owner, string textPart)
        {
            TriggerConditionAndRemainingText triggerCondition = TriggerConditionFactory.ParseTriggerCondition(textPart);
            if (triggerCondition != null)
            {
                ReadOnlyOneShotEffectCollection effects = EffectFactory.ParseOneShotEffect(triggerCondition.RemainingText, owner);
                if (effects != null)
                {
                    return new TriggerAbility(triggerCondition.TriggerCondition, effects, owner, this);
                }
                else
                {
                    return ParseTriggerAbilityWithOneShotEffectForCreature(owner, triggerCondition);
                }
            }
            else
            {
                Duel.NotParsedAbilities.Add(textPart);
                return null;
            }
        }

        private Abilities.Ability ParseTriggerAbilityWithOneShotEffectForCreature(Player owner, TriggerConditionAndRemainingText triggerCondition)
        {
            OneShotEffectForCreature oneShotEffectForCreature = EffectFactory.ParseOneShotEffectForCreature(triggerCondition.RemainingText, this);
            if (oneShotEffectForCreature != null)
            {
                return new TriggerAbility(triggerCondition.TriggerCondition, new ReadOnlyOneShotEffectCollection(oneShotEffectForCreature), owner, this);
            }
            else
            {
                Duel.NotParsedAbilities.Add(triggerCondition.RemainingText);
                return null;
            }
        }
    }
}
