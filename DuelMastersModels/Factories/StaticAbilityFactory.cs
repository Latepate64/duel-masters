using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Factories
{
    public static class StaticAbilityFactory
    {
        private static readonly ReadOnlyDictionary<string, Type> _staticAbilityDictionaryForCreatures = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>
        {
            #region Blocker
            { "Blocker (When an opponent's creature attacks, you may tap this creature to stop the attack. Then the two creatures battle.)", typeof(Blocker) },
            { "Blocker (When an opponent's creature attacks, you may tap this creature to stop the attack. Then the 2 creatures battle.)", typeof(Blocker) },
            { "Blocker (Whenever an opponent's creature attacks, you may tap this creature to stop the attack. Then the 2 creatures battle.)", typeof(Blocker) },
            { "Blocker", typeof(Blocker) },
            #endregion Blocker
            #region Shield trigger
            { "Shield trigger (When this creature is put into your hand from your shield zone, you may summon it immediately for no cost.)", typeof(CreatureShieldTrigger) },
            { "Shield trigger (When this creature is put into your hand from your shield zone, you may summon it for no cost.)", typeof(CreatureShieldTrigger) },
            { "Shield trigger (When this spell is put into your hand from your shield zone, you may cast it immediately for no cost.)", typeof(SpellShieldTrigger) },
            { "Shield trigger (When this spell is put into your hand from your shield zone, you may cast it for no cost.)", typeof(SpellShieldTrigger) },
            #endregion Shield trigger
            { "This creature can't attack players.", typeof(ThisCreatureCannotAttackPlayers) },
        });

        private static readonly ReadOnlyDictionary<string, Type> _staticAbilityDictionaryForSpells = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>
        {
            { "Shield trigger (When this spell is put into your hand from your shield zone, you may cast it immediately for no cost.)", typeof(SpellShieldTrigger) },
            { "Shield trigger (When this spell is put into your hand from your shield zone, you may cast it for no cost.)", typeof(SpellShieldTrigger) },
        });

        public static StaticAbility ParseStaticAbilityForCreature(string text, GameCreature creature)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            ParsedTypesAndObjects parsed = AbilityTypeFactory.GetTypeFromDictionary(text, _staticAbilityDictionaryForCreatures);
            return parsed?.ParsedType != null
                ? Activator.CreateInstance(parsed.ParsedType.TypesParsed[0], AbilityTypeFactory.GetInstanceParameters(/*owner, */creature, new Collection<object>(parsed.Objects.Values.ToList()))) as StaticAbility
                : null;
        }

        public static StaticAbility ParseStaticAbilityForSpell(string text, GameSpell spell)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            ParsedTypesAndObjects parsed = AbilityTypeFactory.GetTypeFromDictionary(text, _staticAbilityDictionaryForSpells);
            return parsed?.ParsedType != null
                ? Activator.CreateInstance(parsed.ParsedType.TypesParsed[0], AbilityTypeFactory.GetInstanceParameters(/*owner, */spell, new Collection<object>(parsed.Objects.Values.ToList()))) as StaticAbility
                : null;
        }
    }
}
