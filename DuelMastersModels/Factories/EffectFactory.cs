using DuelMastersModels.Effects.OneShotEffects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Factories
{
    public static class EffectFactory
    {
        private static readonly ReadOnlyDictionary<string, Type> _effectDictionary = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>
        {
            { "Put the top card of your deck into your mana zone.", typeof(PutTheTopCardOfYourDeckIntoYourManaZoneEffect) },
            { "Tap all your opponent's creatures in the battle zone.", typeof(TapAllYourOpponentsCreaturesInTheBattleZoneEffect) },
            #region You may
            { "You may add a card from your hand to your shields face down. If you do, choose one of your shields and put it into your hand. You can't use the \"shield trigger\" ability of that shield.", typeof(YouMayAddACardFromYourHandToYourShieldsFaceDownIfYouDoChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShieldEffect) },
            { "You may choose a creature in the battle zone and put it into its owner's mana zone. If you do, choose a non-evolution creature in that player's mana zone that costs the same as or less than the number of cards in that mana zone. That player puts that creature into the battle zone.", typeof(SoulSwapEffect/*YouMayChooseACreatureInTheBattleZoneAndPutItIntoItsOwnersManaZoneIfYouDoChooseANonEvolutionCreatureInThatPlayersManaZoneThatCostsTheSameAsOrLessThanTheNumberOfCardsInThatManaZoneThatPlayerPutsThatCreatureIntoTheBattleZone*/) },
            { "You may choose a creature in the battle zone and return it to its owner's hand.", typeof(YouMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHandEffect) },
            { "You may draw a card.", typeof(YouMayDrawACardEffect) },
            #endregion You may
        });

        private static readonly ReadOnlyDictionary<string, Type[]> _effectsDictionary = new ReadOnlyDictionary<string, Type[]>(new Dictionary<string, Type[]>
        {
            { "Put the top card of your deck into your mana zone. Then add the top card of your deck to your shields face down.", new Type[] { typeof(PutTheTopCardOfYourDeckIntoYourManaZoneEffect), typeof(AddTheTopCardOfYourDeckToYourShieldsFaceDownEffect) } },
        });

        private static readonly ReadOnlyDictionary<string, Type> _creatureEffectDictionary = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>
        {
            { "This creature gets $plusinteger power until the end of the turn.", typeof(ThisCreatureGetsXPowerUntilTheEndOfTheTurn) },
            { "This creature gets $plusinteger power until the end of the turn. (Do what the spell says before this creature gets the extra power.)", typeof(ThisCreatureGetsXPowerUntilTheEndOfTheTurn) },
        });

        public static ReadOnlyOneShotEffectCollection ParseOneShotEffect(string text, Player owner)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            ParsedTypesAndObjects parsed = AbilityTypeFactory.GetTypeFromDictionary(text, _effectDictionary);
            if (parsed?.ParsedType != null && string.IsNullOrEmpty(parsed.ParsedType.RemainingText))
            {
                OneShotEffect oneShotEffect = Activator.CreateInstance(parsed.ParsedType.TypesParsed[0]) as OneShotEffect;
                return new ReadOnlyOneShotEffectCollection(oneShotEffect);
                //return Activator.CreateInstance(parsedType.TypeParsed, AbilityTypeFactory.GetInstanceParameters(owner, creature, new Collection<object>(parsedObjects.Values.ToList()))) as PlayerAction;
            }
            else
            {
                return TryParseMultipleEffects(text);
            }
        }

        public static OneShotEffectForCreature ParseOneShotEffectForCreature(string text, Cards.GameCreature creature)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            ParsedTypesAndObjects parsedTypesAndObjects = AbilityTypeFactory.GetTypeFromDictionary(text, _creatureEffectDictionary);
            if (parsedTypesAndObjects?.ParsedType != null && string.IsNullOrEmpty(parsedTypesAndObjects.ParsedType.RemainingText))
            {
                return Activator.CreateInstance(parsedTypesAndObjects.ParsedType.TypesParsed[0], AbilityTypeFactory.GetInstanceParameters(creature, new Collection<object>(parsedTypesAndObjects.Objects.Values.ToList()))) as OneShotEffectForCreature;
            }
            else
            {
                return null;
            }
        }

        public static string GetTextForEffects(ReadOnlyOneShotEffectCollection oneShotEffects)
        {
            if (oneShotEffects.Count == 1)
            {
                OneShotEffect oneShotEffect = oneShotEffects.First();
                if (oneShotEffect is OneShotEffectForCreature oneShotEffectForCreature)
                {
                    string text = _creatureEffectDictionary.First(effect => effect.Value == oneShotEffectForCreature.GetType()).Key;
                    if (oneShotEffectForCreature is ThisCreatureGetsXPowerUntilTheEndOfTheTurn thisCreatureGetsXPowerUntilTheEndOfTheTurn)
                    {
                        text = text.Replace("$plusinteger", string.Concat('+', thisCreatureGetsXPowerUntilTheEndOfTheTurn.Power));
                    }
                    return text;
                }
                else
                {
                    return _effectDictionary.First(effect => effect.Value == oneShotEffect.GetType()).Key;
                }
            }
            else if (oneShotEffects.Count > 1)
            {
                Type[] types = oneShotEffects.Select(e => e.GetType()).ToArray();
                return _effectsDictionary.First(effects => Enumerable.SequenceEqual(effects.Value, types)).Key;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static ReadOnlyOneShotEffectCollection TryParseMultipleEffects(string text)
        {
            ParsedTypesAndObjects parsed = AbilityTypeFactory.GetTypeFromDictionary(text, _effectsDictionary);
            if (parsed?.ParsedType != null)
            {
                return new ReadOnlyOneShotEffectCollection(parsed.ParsedType.TypesParsed.Select(type => Activator.CreateInstance(type) as OneShotEffect));
            }
            else
            {
                return null;
            }
        }
    }
}
