﻿using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Abilities.Trigger;
using DuelMastersModels.Effects.OneShotEffects;
using DuelMastersModels.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Cards
{
    public class Creature : Card
    {
        public int Power { get; set; }
        public Collection<string> Races { get; } = new Collection<string>();

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

        public override Card DeepCopy
        {
            get
            {
                Creature creature = new Creature()
                {
                    Cost = Cost,
                    Flavor = Flavor,
                    GameId = GameId,
                    Id = Id,
                    Illustrator = Illustrator,
                    Name = Name,
                    Power = Power,
                    Rarity = Rarity,
                    Set = Set,
                    SummoningSickness = SummoningSickness,
                    Tapped = Tapped,
                    Text = Text
                };
                foreach (Civilization civilization in Civilizations)
                {
                    creature.Civilizations.Add(civilization);
                }
                foreach (string race in Races)
                {
                    creature.Races.Add(race);
                }
                return creature;
            }
        }

        public Creature() : base()
        {
        }

        /// <summary>
        /// Creates a creature.
        /// </summary>
        public Creature(string name, string set, string id, Collection<string> civilizations, string rarity, int cost, string text, string flavor, string illustrator, int gameId, string power, Collection<string> races, Player owner) : base(name, set, id, civilizations, rarity, cost, text, flavor, illustrator, gameId)
        {
            if (power == null)
            {
                throw new System.ArgumentNullException("power");
            }
            Power = int.Parse(power.Replace("+", "").Replace("-", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            Races = races;

            if (!string.IsNullOrEmpty(text))
            {
                IEnumerable<string> textParts = text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries).Where(t => !(t.StartsWith("(", System.StringComparison.CurrentCulture) && t.EndsWith(")", System.StringComparison.CurrentCulture)));
                foreach (string textPart in textParts)
                {
                    StaticAbility staticAbility = StaticAbilityFactory.ParseStaticAbilityForCreature(textPart, this);
                    if (staticAbility != null)
                    {
                        Abilities.Add(staticAbility);
                    }
                    else
                    {
                        TriggerConditionAndRemainingText triggerCondition = TriggerConditionFactory.ParseTriggerCondition(textPart);
                        if (triggerCondition != null)
                        {
                            Collection<OneShotEffect> effects = EffectFactory.ParseOneShotEffect(triggerCondition.RemainingText);
                            if (effects != null)
                            {
                                Abilities.Add(new TriggerAbility(triggerCondition.TriggerCondition, effects, owner, this));
                            }
                            else
                            {
                                Duel.NotParsedAbilities.Add(triggerCondition.RemainingText);
                            }
                        }
                        else
                        {
                            Duel.NotParsedAbilities.Add(textPart);
                        }
                    }
                }
            }
        }
    }
}
