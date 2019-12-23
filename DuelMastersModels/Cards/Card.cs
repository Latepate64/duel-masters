using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Abilities.Trigger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Cards
{
    public enum Rarity { Common, Uncommon, Rare, VeryRare, SuperRare, None };
    public enum Civilization { Light, Water, Darkness, Fire, Nature };

    /// <summary>
    /// Represent a Duel Masters card.
    /// </summary>
    public abstract class Card
    {
        #region Constants
        private const string LightText = "Light";
        private const string WaterText = "Water";
        private const string DarknessText = "Darkness";
        private const string FireText = "Fire";
        private const string NatureText = "Nature";

        private const string CommonText = "Common";
        private const string UncommonText = "Uncommon";
        private const string RareText = "Rare";
        private const string VeryRareText = "Very Rare";
        private const string SuperRareText = "Super Rare";
        private const string NoRarityText = "No Rarity";
        #endregion Constants

        #region Properties
        public string Name { get; private set; }
        public string Set { get; private set; }
        public string Id { get; private set; }
        public ReadOnlyCivilizationCollection Civilizations { get; }
        public Rarity Rarity { get; private set; }
        public int Cost { get; private set; }
        public string Text { get; private set; }
        public string Flavor { get; private set; }
        public string Illustrator { get; private set; }

        public AbilityCollection Abilities { get; } = new AbilityCollection();
        public ReadOnlyStaticAbilityCollection StaticAbilities => new ReadOnlyStaticAbilityCollection(Abilities.Where(a => a is StaticAbility).Cast<StaticAbility>());
        public ReadOnlyTriggerAbilityCollection TriggerAbilities => new ReadOnlyTriggerAbilityCollection(Abilities.Where(a => a is TriggerAbility).Cast<TriggerAbility>());
        #endregion Properties

        #region Fields
        private static readonly Dictionary<string, Rarity> _rarities = new Dictionary<string, Rarity>()
        {
            { CommonText, Rarity.Common },
            { UncommonText, Rarity.Uncommon },
            { RareText, Rarity.Rare },
            { VeryRareText, Rarity.VeryRare },
            { SuperRareText, Rarity.SuperRare },
            { NoRarityText, Rarity.None },
        };
        #endregion Fields

        protected Card() { }

        /// <summary>
        /// Creates a card.
        /// </summary>
        protected Card(string name, string set, string id, Collection<string> civilizations, string rarity, int cost, string text, string flavor, string illustrator)
        {
            Name = name;
            Set = set;
            Id = id;
            Civilizations = GetCivilizations(civilizations);
            Rarity = GetRarity(rarity);
            Cost = cost;
            Flavor = flavor;
            Illustrator = illustrator;
            Text = text;
        }

        #region Private methods
        /// <summary>
        /// Returns a civilization from text.
        /// </summary>
        private static ReadOnlyCivilizationCollection GetCivilizations(Collection<string> civilizationTexts)
        {
            if (civilizationTexts == null)
            {
                throw new ArgumentNullException("civilizationTexts");
            }
            else
            {
                List<Civilization> civilizations = new List<Civilization>();
                if (civilizationTexts.Contains(LightText))
                {
                    civilizations.Add(Civilization.Light);
                }
                if (civilizationTexts.Contains(WaterText))
                {
                    civilizations.Add(Civilization.Water);
                }
                if (civilizationTexts.Contains(DarknessText))
                {
                    civilizations.Add(Civilization.Darkness);
                }
                if (civilizationTexts.Contains(FireText))
                {
                    civilizations.Add(Civilization.Fire);
                }
                if (civilizationTexts.Contains(NatureText))
                {
                    civilizations.Add(Civilization.Nature);
                }
                if (civilizations.Count == 0)
                {
                    throw new ArgumentException("Failed to identify a civilization: " + civilizationTexts);
                }
                return new ReadOnlyCivilizationCollection(civilizations);
            }
        }

        /// <summary>
        /// Returns a rarity from text.
        /// </summary>
        private static Rarity GetRarity(string text)
        {
            return _rarities[text];
        }
        #endregion Private methods
    }
}
