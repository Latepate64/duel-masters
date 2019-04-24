﻿using DuelMastersModels.Abilities.Static;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Cards
{
    public class Creature : Card
    {
        public int Power { get; set; }
        public Collection<string> Races { get; } = new Collection<string>();
        public bool SummoningSickness { get; set; } = true;
        public Collection<StaticAbility> StaticAbilities { get; } = new Collection<StaticAbility>();

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
        public Creature(string name, string set, string id, Collection<string> civilizations, string rarity, int cost, string text, string flavor, string illustrator, int gameId, string power, Collection<string> races) : base(name, set, id, civilizations, rarity, cost, text, flavor, illustrator, gameId)
        {
            if (power == null)
            {
                throw new System.ArgumentNullException("power");
            }
            Power = int.Parse(power.Replace("+", "").Replace("-", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            Races = races;
        }
    }
}
