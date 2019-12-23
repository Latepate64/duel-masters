using DuelMastersModels.Cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DuelMastersModels.Factories
{
    public class ParsedType
    {
        public Collection<Type> TypesParsed { get; private set; }
        public string RemainingText { get; private set; }

        public ParsedType(Type type, string remainingText)
        {
            TypesParsed = new Collection<Type>() { type };
            RemainingText = remainingText;
        }

        public ParsedType(Type[] types, string remainingText)
        {
            TypesParsed = new Collection<Type>(types);
            RemainingText = remainingText;
        }
    }

    public class ParsedTypesAndObjects
    {
        public ParsedType ParsedType { get; private set; }
        public Dictionary<string, object> Objects { get; private set; }

        public ParsedTypesAndObjects(ParsedType parsedType, Dictionary<string, object> parsedObjects)
        {
            ParsedType = parsedType;
            Objects = parsedObjects;
        }
    }

    public static class AbilityTypeFactory
    {
        #region const
        private const string SurvivorText = "Survivor (Each of your Survivors has this creature's Survivor ability.) : ";
        private const string TurboRushText = "Turbo rush (If any of your other creatures broke any shields this turn, this creature gets its Turbo rush ability until the end of the turn.) : ";
        private const string WaveStrikerText = "Wave striker (While 2 or more other creatures in the battle zone have \"wave striker,\" this creature has its Wave striker ability.) : ";
        private const string WaveStrikerMultipleText = "Wave striker (While 2 or more other creatures in the battle zone have \"wave striker,\" this creature has its Wave striker abilities.) : ";
        #endregion const

        #region Methods
        #region Public
        #region string
        /// <summary>
        /// Returns a civilization as a string lowercased.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string GetCivilizationAsStringLowercase(Civilization civilization)
        {
            return civilization.ToString().ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns races as a string.
        /// </summary>
        public static string GetRacesAsString(string race)
        {
            return GetRacesAsString(null, race, " / ");
        }

        public static string GetRacesAsString(ReadOnlyCollection<string> races)
        {
            return GetRacesAsString(races, null, " / ");
        }

        public static string GetRacesAsString(ReadOnlyCollection<string> races, string separator)
        {
            return GetRacesAsString(races, null, separator);
        }
        #endregion string

        #region string[]
        /// <summary>
        /// Returns races from a text.
        /// </summary>
        /*public static string[] GetRaces(string text)
        {
            throw new NotImplementedException();
            
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else
            {
                Collection<string> races = new Collection<string>();
                string[] raceSplits = text.Split('/');
                foreach (string raceText in raceSplits)
                {
                    foreach (var race in RaceManager.Races)
                    {
                        if (raceText == race)
                        {
                            races.Add(race);
                            break;
                        }
                    }
                }
                if (races.Count != raceSplits.Count())
                {
                    throw new ArgumentException("Couldn't find races from text: " + text);
                }
                else
                {
                    return races.ToArray();
                }
            }
        }*/

        /// <summary>
        /// Checks if any given race belongs to a target group of races.
        /// </summary>
        /*
        public static string[] GetRaceGroups(ReadOnlyCollection<string> races)
        {
            throw new NotImplementedException();

            if (races == null)
            {
                throw new ArgumentNullException("races");
            }
            else
            {
                Collection<string> raceGroups = new Collection<string>();
                foreach (string race in races)
                {
                    foreach (var raceGroupPair in RaceManager.RaceGroups)
                    {
                        if (raceGroupPair.Value.Contains(race))
                        {
                            raceGroups.Add(raceGroupPair.Key);
                        }
                    }
                }
                return raceGroups.GroupBy(x => x).Select(x => x.First()).ToArray();
            }
        }*/
        #endregion string[]

        #region object[]
        /*
        public static object[] GetInstanceParameters(INonStaticAbility nonStaticAbility, Collection<object> parsedObjects)
        {
            var parameters = new List<object>() { nonStaticAbility };
            parameters.AddRange(parsedObjects);
            return parameters.ToArray();
        }*/

        public static object[] GetInstanceParameters(GameCreature creature, Collection<object> parsedObjects)
        {
            List<object> parameters = new List<object>() { creature };
            parameters.AddRange(parsedObjects);
            return parameters.ToArray();
        }

        public static object[] GetInstanceParameters(GameSpell spell, Collection<object> parsedObjects)
        {
            List<object> parameters = new List<object>() { spell };
            parameters.AddRange(parsedObjects);
            return parameters.ToArray();
        }

        public static object[] GetInstanceParameters(Player owner, Collection<object> parsedObjects)
        {
            List<object> parameters = new List<object>() { owner };
            parameters.AddRange(parsedObjects);
            return parameters.ToArray();
        }

        public static object[] GetInstanceParameters(Player owner, Creature creature, Collection<object> parsedObjects)
        {
            List<object> parameters = new List<object>() { owner, creature };
            parameters.AddRange(parsedObjects);
            return parameters.ToArray();
        }
        #endregion object[]

        /*
        public static ParsedAbilityGroup ParseAbilityGroup(string abilityGroupText)
        {
            if (abilityGroupText == null)
            {
                throw new ArgumentNullException("abilityGroupText");
            }
            else
            {
                if (abilityGroupText.StartsWith(SurvivorText, StringComparison.Ordinal))
                {
                    return new ParsedAbilityGroup(AbilityGroup.Survivor, abilityGroupText.Remove(0, SurvivorText.Length));
                }
                else if (abilityGroupText.StartsWith(TurboRushText, StringComparison.Ordinal))
                {
                    return new ParsedAbilityGroup(AbilityGroup.TurboRush, abilityGroupText.Remove(0, TurboRushText.Length));
                }
                else if (abilityGroupText.StartsWith(WaveStrikerText, StringComparison.Ordinal))
                {
                    return new ParsedAbilityGroup(AbilityGroup.WaveStriker, abilityGroupText.Remove(0, WaveStrikerText.Length));
                }
                else if (abilityGroupText.StartsWith(WaveStrikerMultipleText, StringComparison.Ordinal))
                {
                    return new ParsedAbilityGroup(AbilityGroup.WaveStrikerMultiple, abilityGroupText.Remove(0, WaveStrikerMultipleText.Length));
                }
                else
                {
                    return new ParsedAbilityGroup(AbilityGroup.None, abilityGroupText);
                }
            }
        }
        */

        /*
        public static void CreateFileForDictionaryKeys()
        {
            string path = @"c:\Duel Masters Arena\DictionaryKeys.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine("Activated abilities:");
                    foreach (var key in ActivatedAbilityManager.ActivatedAbilityTexts)
                    {
                        writer.WriteLine(key);
                    }
                    writer.WriteLine("\r\nStatic abilities for creatures:");
                    foreach (var key in StaticAbilityParserForCreaturesManager.StaticAbilityTexts)
                    {
                        writer.WriteLine(key);
                    }
                    writer.WriteLine("\r\nStatic abilities for spells:");
                    foreach (var key in Spell.StaticAbilityTexts)
                    {
                        writer.WriteLine(key);
                    }
                    writer.WriteLine("\r\nTrigger abilities:");
                    foreach (var key in TriggerAbilityParserManager.TriggerAbilityTexts)
                    {
                        writer.WriteLine(key);
                    }
                    writer.WriteLine("\r\nEffects for both creatures and spells:");
                    foreach (var key in NonStaticAbilityManager.NonStaticAbilityTexts)
                    {
                        writer.WriteLine(key);
                    }
                    writer.WriteLine("\r\nEffects for creatures only:");
                    foreach (var key in NonStaticAbilityManager.NonStaticAbilityForCreatureTexts)
                    {
                        writer.WriteLine(key);
                    }
                    writer.WriteLine("\r\nEffects for spells only:");
                    foreach (var key in Spell.EffectForSpellsTexts)
                    {
                        writer.WriteLine(key);
                    }
                }
            }
        }*/
        #endregion Public

        private static string GetRacesAsString(ReadOnlyCollection<string> races, string race, string separator)
        {
            if (races == null)
            {
                races = new ReadOnlyCollection<string>(new List<string>() { race });
            }
            List<string> stringRaces = new List<string>();
            foreach (string raceItem in races)
            {
                stringRaces.Add(Regex.Replace(raceItem.ToString(), "([a-z])([A-Z])", "$1 $2"));
            }
            return string.Join(separator, stringRaces.ToArray());
        }
        #endregion Methods

        /// <summary>
        /// Find a key from dictionary which matches the text.
        /// </summary>
        public static ParsedTypesAndObjects GetTypeFromDictionary<T>(string inputText, ReadOnlyDictionary<string, T> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (inputText == null)
            {
                throw new ArgumentNullException("inputText");
            }
            else
            {
                Dictionary<string, object> parsedObjects = new Dictionary<string, object>();
                List<string> possibleKeys = dictionary.Keys.ToList();
                string[] inputWords = inputText.Split(' ');
                int inputWordIndex = 0;
                int addIndex = 0;
                int updatedAddIndex = 0;
                bool keyFound = false;
                while (!keyFound && possibleKeys.Count > 0 && inputWordIndex < inputWords.Count())
                {
                    if (updatedAddIndex != addIndex)
                    {
                        updatedAddIndex = addIndex;
                    }
                    if (inputWordIndex < inputWords.Count())
                    {
                        int keyIndex = 0;
                        while (keyIndex < possibleKeys.Count)
                        {
                            string[] keyWordSplits = possibleKeys[keyIndex].Split(' ');
                            if (inputWordIndex < keyWordSplits.Count())
                            {
                                string keyWord = keyWordSplits[inputWordIndex];
                                string inputWord = inputWords[inputWordIndex + addIndex];
                                if (KeyWordMatchesInputWord(keyWord, inputWord, parsedObjects, inputWordIndex, inputWords, ref addIndex, updatedAddIndex))
                                {
                                    ++keyIndex;
                                }
                                else
                                {
                                    possibleKeys.RemoveAt(keyIndex);
                                }
                            }
                            else if (possibleKeys.Count == 1)
                            {
                                keyFound = true;
                                break;
                            }
                            else
                            {
                                possibleKeys.RemoveAt(keyIndex);
                            }
                        }
                        ++inputWordIndex;
                    }
                    else
                    {
                        possibleKeys = new List<string>() { };
                    }
                }
                if (possibleKeys.Count == 0)
                {
                    return null;
                }
                else if (possibleKeys.Count == 1)
                {
                    string matchingKey = possibleKeys[0];
                    ManageParsedObjects(matchingKey, ref parsedObjects);
                    List<string> wordList = inputWords.ToList();
                    wordList.RemoveRange(0, matchingKey.Split(' ').Count() + updatedAddIndex);
                    inputText = string.Join(" ", wordList);
                    if (inputText.Length > 0)
                    {
                        inputText = UppercaseFirstLetter(inputText);
                    }
                    return new ParsedTypesAndObjects(GetParsedTypes(inputText, dictionary, matchingKey), parsedObjects);
                }
                else
                {
                    string matchingKey = possibleKeys.OrderBy(key => key.Length).First();
                    ManageParsedObjects(matchingKey, ref parsedObjects);
                    List<string> wordList = inputWords.ToList();
                    wordList.RemoveRange(0, matchingKey.Split(' ').Count() + updatedAddIndex);
                    inputText = string.Join(" ", wordList);
                    if (inputText.Length > 0)
                    {
                        inputText = UppercaseFirstLetter(inputText);
                    }
                    return new ParsedTypesAndObjects(GetParsedTypes(inputText, dictionary, matchingKey), parsedObjects);
                }
            }
        }

        private static ParsedType GetParsedTypes<T>(string inputText, ReadOnlyDictionary<string, T> dictionary, string matchingKey)
        {
            if (typeof(T) == typeof(Type))
            {
                return new ParsedType(dictionary[matchingKey] as Type, inputText);
            }
            else if (typeof(T) == typeof(Type[]))
            {
                return new ParsedType(dictionary[matchingKey] as Type[], inputText);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Find a key from dictionary which matches the text.
        /// </summary>
        /*public static ParsedType GetTypeFromDictionary(string inputText, ReadOnlyDictionary<string, Type> dictionary, out Dictionary<string, object> parsedObjects)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (inputText == null)
            {
                throw new ArgumentNullException("inputText");
            }
            else
            {
                parsedObjects = new Dictionary<string, object>();
                List<string> possibleKeys = dictionary.Keys.ToList();
                string[] inputWords = inputText.Split(' ');
                int inputWordIndex = 0;
                int addIndex = 0;
                int updatedAddIndex = 0;
                bool keyFound = false;
                while (!keyFound && possibleKeys.Count > 0 && inputWordIndex < inputWords.Count())
                {
                    if (updatedAddIndex != addIndex)
                    {
                        updatedAddIndex = addIndex;
                    }
                    if (inputWordIndex < inputWords.Count())
                    {
                        int keyIndex = 0;
                        while (keyIndex < possibleKeys.Count)
                        {
                            string[] keyWordSplits = possibleKeys[keyIndex].Split(' ');
                            if (inputWordIndex < keyWordSplits.Count())
                            {
                                string keyWord = keyWordSplits[inputWordIndex];
                                string inputWord = inputWords[inputWordIndex + addIndex];
                                if (KeyWordMatchesInputWord(keyWord, inputWord, parsedObjects, inputWordIndex, inputWords, ref addIndex, updatedAddIndex))
                                {
                                    ++keyIndex;
                                }
                                else
                                {
                                    possibleKeys.RemoveAt(keyIndex);
                                }
                            }
                            else if (possibleKeys.Count == 1)
                            {
                                keyFound = true;
                                break;
                            }
                            else
                            {
                                possibleKeys.RemoveAt(keyIndex);
                            }
                        }
                        ++inputWordIndex;
                    }
                    else
                    {
                        possibleKeys = new List<string>() { };
                    }
                }
                if (possibleKeys.Count == 0)
                {
                    return null;
                }
                else if (possibleKeys.Count == 1)
                {
                    string matchingKey = possibleKeys[0];
                    ManageParsedObjects(matchingKey, ref parsedObjects);
                    List<string> wordList = inputWords.ToList();
                    wordList.RemoveRange(0, matchingKey.Split(' ').Count() + updatedAddIndex);
                    inputText = string.Join(" ", wordList);
                    if (inputText.Length > 0)
                    {
                        inputText = UppercaseFirstLetter(inputText);
                    }
                    return new ParsedType(dictionary[matchingKey], inputText);
                }
                else
                {
                    string matchingKey = possibleKeys.OrderBy(key => key.Length).First();
                    ManageParsedObjects(matchingKey, ref parsedObjects);
                    List<string> wordList = inputWords.ToList();
                    wordList.RemoveRange(0, matchingKey.Split(' ').Count() + updatedAddIndex);
                    inputText = string.Join(" ", wordList);
                    if (inputText.Length > 0)
                    {
                        inputText = UppercaseFirstLetter(inputText);
                    }
                    return new ParsedType(dictionary[matchingKey], inputText);
                }
            }
        }*/

        #region Private
        #region bool
        private static bool IsCivilization(string text, Dictionary<string, object> parsedObjects, string variableWord)
        {
            foreach (Civilization civilization in Enum.GetValues(typeof(Civilization)))
            {
                if (string.Compare(text, civilization.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    UpdateParsedObjects(parsedObjects, variableWord, civilization);
                    return true;
                }
            }
            return false;
        }

        private static bool IsCardName(int inputWordIndex, List<string> inputWords, Dictionary<string, object> parsedObjects, ref int addIndex)
        {
            throw new NotImplementedException();
            /*
            inputWords.RemoveRange(0, inputWordIndex);
            string text = string.Join(" ", inputWords);
            foreach (var cardName in JsonManager.CardNames)
            {
                if (text.StartsWith(cardName, StringComparison.Ordinal))
                {
                    addIndex += cardName.Split(' ').Count() - 1;
                    UpdateParsedObjects(parsedObjects, CreatureNameText, cardName);
                    return true;
                }
            }
            return false;
            */
        }

        private static bool IsInteger(string text, Dictionary<string, object> parsedObjects, string variableWord)
        {
            if (Regex.IsMatch(text, @"^\d+$"))
            {
                UpdateParsedObjects(parsedObjects, variableWord, int.Parse(text, CultureInfo.InvariantCulture));
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsNonCivilization(string text, Dictionary<string, object> parsedObjects)
        {
            foreach (Civilization civilization in Enum.GetValues(typeof(Civilization)))
            {
                if (string.Compare(text, "non-" + civilization.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    UpdateParsedObjects(parsedObjects, NonCivilizationText, civilization);
                    return true;
                }
            }
            return false;
        }

        private static bool IsPlusInteger(string text, Dictionary<string, object> parsedObjects)
        {
            if (Regex.IsMatch(text, @"\+\d+"))
            {
                UpdateParsedObjects(parsedObjects, PlusIntegerText, int.Parse(text.Substring(1), CultureInfo.InvariantCulture));
                return true;
            }
            else
            {
                return false;
            }
        }

        /*private static bool IsRace(int inputWordIndex, List<string> inputWords, Dictionary<string, object> parsedObjects, ref int addIndex, string variableWord, int updatedAddIndex)
        {
            throw new NotImplementedException();
            
            inputWords.RemoveRange(0, inputWordIndex + updatedAddIndex);
            string text = stringJoin(" ", inputWords);
            string possibleRace = null;
            foreach (var race in RaceManager.Races)
            {
                var raceText = race;
                if (text.StartsWith(raceText, StringComparison.Ordinal) &&
                    (stringIsNullOrEmpty(possibleRace) ||
                    race.ToString().Length > possibleRace.ToString().Length))
                {
                    possibleRace = race;
                }
            }
            if (!stringIsNullOrEmpty(possibleRace))
            {
                if (!parsedObjects.Values.Contains(RaceManager.GetRaceInPluralForm(possibleRace)))
                {
                    if (UpdateParsedObjects(parsedObjects, variableWord, possibleRace) || addIndex == updatedAddIndex)
                    {
                        addIndex += possibleRace.Split(' ').Count() - 1;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }*/

        /*private static bool IsRaces(int inputWordIndex, List<string> inputWords, Dictionary<string, object> parsedObjects, ref int addIndex, string variableWord, int updatedAddIndex)
        {
            throw new NotImplementedException();

            inputWords.RemoveRange(0, inputWordIndex + updatedAddIndex);
            string text = string.Join(" ", inputWords);
            string possibleRace = null;
            foreach (var race in RaceManager.RacesPlural)
            {
                var raceText = race;
                if (text.StartsWith(raceText, StringComparison.Ordinal) &&
                    (string.IsNullOrEmpty(possibleRace) ||
                    race.ToString().Length > possibleRace.ToString().Length))
                {
                    possibleRace = race;
                }
            }
            if (!string.IsNullOrEmpty(possibleRace))
            {
                if (UpdateParsedObjects(parsedObjects, variableWord, possibleRace) || addIndex == updatedAddIndex)
                {
                    addIndex += possibleRace.Split(' ').Count() - 1;
                }
                return true;
            }
            else
            {
                return false;
            }
        }*/

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "racegroup")]
        private static bool IsRaceGroup(string text, Dictionary<string, object> parsedObjects)
        {
            throw new NotImplementedException();

            foreach (string raceGroup in RaceManager.RaceGroups.Keys)
            {
                if (string.Compare(text, raceGroup.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    UpdateParsedObjects(parsedObjects, RaceGroupVariableText, raceGroup);
                    return true;
                }
            }
            return false;

        }*/

        private static bool IsKeyWordVariable(string keyWord, string variableText)
        {
            return GetProperInputText(keyWord) == variableText;
        }

        private static bool KeyWordMatchesInputWord(
            string keyWord, string inputWord, Dictionary<string, object> parsedObjects,
            int inputWordIndex, string[] inputWords, ref int addIndex, int updatedAddIndex)
        {
            return 
                keyWord == inputWord ||
                (IsKeyWordVariable(keyWord, "$integer") && IsInteger(GetProperInputText(inputWord), parsedObjects, "$integer")) ||
                (IsKeyWordVariable(keyWord, "$integer2") && IsInteger(GetProperInputText(inputWord), parsedObjects, "$integer2")) ||
                (IsKeyWordVariable(keyWord, "$plusinteger") && IsPlusInteger(GetProperInputText(inputWord), parsedObjects)) ||
                (IsKeyWordVariable(keyWord, "$civilization") && IsCivilization(GetProperInputText(inputWord), parsedObjects, "$civilization")) ||
                (IsKeyWordVariable(keyWord, "$civilization2") && IsCivilization(GetProperInputText(inputWord), parsedObjects, "$civilization2")) ||
                (IsKeyWordVariable(keyWord, "$creaturename") && IsCardName(inputWordIndex, inputWords.ToList(), parsedObjects, ref addIndex)) ||
                (IsKeyWordVariable(keyWord, "$noncivilization") && IsNonCivilization(GetProperInputText(inputWord), parsedObjects)/*) ||
                (IsKeyWordVariable(keyWord, "$race") && IsRace(inputWordIndex, inputWords.ToList(), parsedObjects, ref addIndex, "$race", updatedAddIndex)) ||
                (IsKeyWordVariable(keyWord, "$race2") && IsRace(inputWordIndex, inputWords.ToList(), parsedObjects, ref addIndex, "$race2", updatedAddIndex)) ||*/
                /*(IsKeyWordVariable(keyWord, "$races") && IsRaces(inputWordIndex, inputWords.ToList(), parsedObjects, ref addIndex, "$races", updatedAddIndex)) ||
                (IsKeyWordVariable(keyWord, "$races2") && IsRaces(inputWordIndex, inputWords.ToList(), parsedObjects, ref addIndex, "$races2", updatedAddIndex)) ||
                (IsKeyWordVariable(keyWord, "$racegroup") && IsRaceGroup(GetProperInputText(inputWord), parsedObjects)*/);
        }

        private static bool UpdateParsedObjects(Dictionary<string, object> parsedObjects, string text, object parsedObject)
        {
            if (!parsedObjects.ContainsKey(text))
            {
                parsedObjects.Add(text, parsedObject);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion bool

        #region string
        private static string GetProperInputText(string text)
        {
            return text.Replace("\"", "").Replace(".", "");
        }
        #endregion string

        private static void ManageParsedObjects(string text, ref Dictionary<string, object> parsedObjects)
        {
            if (!text.Contains("$"))
            {
                parsedObjects.Clear();
            }
        }

        /// <summary>
        /// Upper cases first letter.
        /// </summary>
        /// <returns></returns>
        private static string UppercaseFirstLetter(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else
            {
                char firstLetter = text[0];
                firstLetter = char.ToUpper(firstLetter, CultureInfo.InvariantCulture);
                return firstLetter + text.Substring(1);
            }
        }
        #endregion Private

        private const string EffectErrorText = "Failed to parse text for an effect:\r\n            {0}";
        private const string RaceGroupVariableText = "$racegroup";
        private const string CreatureNameText = "$creaturename";
        private const string NonCivilizationText = "$noncivilization";
        private const string PlusIntegerText = "$plusinteger";
        private const string RaceGroupText = "$racegroup";
    }
}
