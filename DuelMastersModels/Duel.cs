﻿using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Abilities.Trigger;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.GameActions.StateBasedActions;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.AutomaticActions;
using DuelMastersModels.PlayerActions.CardSelections;
using DuelMastersModels.PlayerActions.CreatureSelections;
using DuelMastersModels.PlayerActions.OptionalActions;
using DuelMastersModels.PlayerActionResponses;
using DuelMastersModels.Steps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels
{
    /// <summary>
    /// Specifies the player who goes first in the duel.
    /// </summary>
    public enum StartingPlayer
    {
        /// <summary>
        /// Player 1 goes first.
        /// </summary>
        Player1 = 0,
        
        /// <summary>
        /// Player 2 goes first.
        /// </summary>
        Player2 = 1,

        /// <summary>
        /// Player who goes first is randomized.
        /// </summary>
        Random = 2,
    }

    /// <summary>
    /// Represents a duel that is played between two players.
    /// </summary>
    public class Duel : System.ComponentModel.INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// A player that participates in duel against player 2.
        /// </summary>
        public Player Player1 { get; set; }

        /// <summary>
        /// A player that participates in duel against player 1.
        /// </summary>
        public Player Player2 { get; set; }

        /// <summary>
        /// Player who won the duel.
        /// </summary>
        public Player Winner { get; private set; }

        /// <summary>
        /// Players who lost the duel.
        /// </summary>
        public Collection<Player> Losers { get; } = new Collection<Player>();

        /// <summary>
        /// Determines whether duel has ended or not.
        /// </summary>
        public bool Ended { get; private set; }

        /// <summary>
        /// All the turns of the duel that have been or are processed, in order.
        /// </summary>
        public ObservableCollection<Turn> Turns { get; } = new ObservableCollection<Turn>();

        /// <summary>
        /// The turn that is currently being processed.
        /// </summary>
        public Turn CurrentTurn => Turns.Last();

        /// <summary>
        /// The number of shields each player has at the start of a duel. 
        /// </summary>
        public int InitialNumberOfShields { get; set; } = 5;

        /// <summary>
        /// The number of cards each player draw at the start of a duel.
        /// </summary>
        public int InitialNumberOfHandCards { get; set; } = 5;

        /// <summary>
        /// An event that occurs if a property of duel changes.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// An action a player is currently performing.
        /// </summary>
        public PlayerAction CurrentPlayerAction
        {
            get => _currentPlayerAction;
            set
            {
                if (value != _currentPlayerAction)
                {
                    _currentPlayerAction = value;
                    NotifyPropertyChanged(); //TODO: Remove
                }
            }
        }
        private PlayerAction _currentPlayerAction;

        /// <summary>
        /// Spells that are being resolved.
        /// </summary>
        public SpellCollection SpellsBeingResolved { get; } = new SpellCollection();


        /// <summary>
        /// Abilities that could not be parsed.
        /// </summary>
        //todo: Not parsed abilities should be thrown as an exception, not to be saved to any collection like this.
        public static Collection<string> NotParsedAbilities { get; } = new Collection<string>();

        /// <summary>
        /// All creatures that are in the battle zone.
        /// </summary>
        public ReadOnlyCreatureCollection CreaturesInTheBattleZone
        {
            get
            {
                List<Creature> creatures = Player1.BattleZone.Creatures.ToList();
                creatures.AddRange(Player2.BattleZone.Creatures);
                return new ReadOnlyCreatureCollection(creatures);
            }
        }

        /// <summary>
        /// A non-static ability that is currently being resolved.
        /// </summary>
        public NonStaticAbility AbilityBeingResolved { get; set; }

        /// <summary>
        /// Non-static abilities that are waiting to be resolved.
        /// </summary>
        public Collection<NonStaticAbility> PendingAbilities { get; } = new Collection<NonStaticAbility>();

        /// <summary>
        /// Non-static abilities controlled by active player that are waiting to be resolved.
        /// </summary>
        public ObservableCollection<NonStaticAbility> PendingAbilitiesForActivePlayer => new ObservableCollection<NonStaticAbility>(PendingAbilities.Where(a => a.Controller == CurrentTurn.ActivePlayer).ToList());

        /// <summary>
        /// Non-static abilities controlled by non-active player that are waiting to be resolved.
        /// </summary>
        public ObservableCollection<NonStaticAbility> PendingAbilitiesForNonActivePlayer => new ObservableCollection<NonStaticAbility>(PendingAbilities.Where(a => a.Controller == CurrentTurn.NonActivePlayer).ToList());

        /// <summary>
        /// Continuous effects that are generated by non-static abilities. Use method GetContinuousEffects() to obtain all continuous effects generated by non-static and static abilities.
        /// </summary>
        public ObservableContinuousEffectCollection ContinuousEffects { get; } = new ObservableContinuousEffectCollection();

        /// <summary>
        /// Cards the player drew at the start of duel.
        /// </summary>
        public ObservableCollection<Card> CardsDrawnAtTheStartOfDuel { get; } = new ObservableCollection<Card>();
        #endregion Properties

        #region Public methods
        #region Player
        /// <summary>
        /// Returns the opponent of a player.
        /// </summary>
        public Player GetOpponent(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }
            else if (player == Player1)
            {
                return Player2;
            }
            else if (player == Player2)
            {
                return Player1;
            }
            else
            {
                throw new ArgumentOutOfRangeException("player");
            }
        }

        /// <summary>
        /// Returns player with target identifier.
        /// </summary>
        /// <param name="id">Identifier the player should have.</param>
        /// <returns>Player with target identifier.</returns>
        public Player GetPlayer(int id)
        {
            if (Player1.Id == id)
            {
                return Player1;
            }
            else if (Player2.Id == id)
            {
                return Player2;
            }
            else
            {
                throw new ArgumentOutOfRangeException("id");
            }
        }

        /// <summary>
        /// Returns the player who owns target card.
        /// </summary>
        /// <param name="card">Card whose owner is queried.</param>
        /// <returns>Player who owns the card.</returns>
        public Player GetOwner(Card card)
        {
            if (Player1.DeckBeforeDuel.Select(c => c.GameId).Contains(card.GameId))
            {
                return Player1;
            }
            else if (Player2.DeckBeforeDuel.Select(c => c.GameId).Contains(card.GameId))
            {
                return Player2;
            }
            else
            {
                throw new ArgumentOutOfRangeException("card");
            }
        }
        #endregion Player

        #region void
        /// <summary>
        /// Ends the duel.
        /// </summary>
        public void End(Player winner)
        {
            Winner = winner;
            Losers.Add(GetOpponent(winner));
            Ended = true;
        }

        /// <summary>
        /// Ends duel in a draw.
        /// </summary>
        public void EndDuelInDraw()
        {
            Losers.Add(Player1);
            Losers.Add(Player2);
            Ended = true;
        }

        /// <summary>
        /// Target player puts target card from their hand into their mana zone.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        public void PutFromHandIntoManaZone(Player player, Card card)
        {
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }
            player.Hand.Remove(card, this);
            player.ManaZone.Add(card, this);
        }

        /// <summary>
        /// Manages a battle between two creatures.
        /// </summary>
        /// <param name="attackingCreature">Creature which initiated the attack.</param>
        /// <param name="defendingCreature">Creature which the attack was directed at.</param>
        /// <param name="attackingPlayer"></param>
        /// <param name="defendingPlayer"></param>
        public void Battle(Creature attackingCreature, Creature defendingCreature, Player attackingPlayer, Player defendingPlayer)
        {
            if (attackingCreature == null)
            {
                throw new ArgumentNullException("attackingCreature");
            }
            else if (defendingCreature == null)
            {
                throw new ArgumentNullException("defendingCreature");
            }
            else if (attackingPlayer == null)
            {
                throw new ArgumentNullException("attackingPlayer");
            }
            else if (defendingPlayer == null)
            {
                throw new ArgumentNullException("defendingPlayer");
            }
            int attackingCreaturePower = GetPower(attackingCreature);
            int defendingCreaturePower = GetPower(defendingCreature);
            //TODO: Handle destruction as a state-based action. 703.4d
            if (attackingCreaturePower > defendingCreaturePower)
            {
                PutFromBattleZoneIntoGraveyard(defendingPlayer, defendingCreature);
            }
            else if (attackingCreaturePower < defendingCreaturePower)
            {
                PutFromBattleZoneIntoGraveyard(attackingPlayer, attackingCreature);
            }
            else
            {
                PutFromBattleZoneIntoGraveyard(attackingPlayer, attackingCreature);
                PutFromBattleZoneIntoGraveyard(defendingPlayer, defendingCreature);
            }
        }

        /// <summary>
        /// Card is used based on its type: A creature is put into the battle zone; A spell is put into your graveyard.
        /// </summary>
        /// <param name="card"></param>
        public void UseCard(Card card)
        {
            if (card is Creature creature)
            {
                GetOwner(creature).BattleZone.Add(creature, this);
            }
            else if (card is Spell spell)
            {
                SpellsBeingResolved.Add(spell);

                foreach (Creature battleZoneCreature in CreaturesInTheBattleZone)
                {
                    foreach (TriggerAbility ability in battleZoneCreature.TriggerAbilities.Where(ability => ability.TriggerCondition is WheneverAPlayerCastsASpell))
                    {
                        TriggerTriggerAbility(ability, ability.Controller);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("mainStep.CardToBeUsed");
            }
        }

        /// <summary>
        /// Once an ability has triggered, its controller puts it on the stack as an object that’s not a card the next time a player would receive priority.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="controller"></param>
        public void TriggerTriggerAbility(TriggerAbility ability, Player controller)
        {
            PendingAbilities.Add(ability.CreatePendingTriggerAbility(controller));
        }

        /// <summary>
        /// Player adds a card from their hand to their shields face down.
        /// </summary>
        /// <param name="card"></param>
        public void AddFromYourHandToYourShieldsFaceDown(Card card)
        {
            Player owner = GetOwner(card);
            owner.Hand.Cards.Remove(card);
            owner.ShieldZone.Cards.Add(card);
        }
        #endregion void

        #region bool
        /// <summary>
        /// Checks if a card can be used.
        /// </summary>
        public static bool CanBeUsed(Card card, ReadOnlyCardCollection manaCards)
        {
            //System.Collections.Generic.IEnumerable<Civilization> manaCivilizations = manaCards.SelectMany(manaCard => manaCard.Civilizations).Distinct();
            //return card.Cost <= manaCards.Count && card.Civilizations.Intersect(manaCivilizations).Count() == 1; //TODO: Add support for multicolored cards.
            return card.Cost <= manaCards.Count && HasCivilizations(manaCards, card.Civilizations);
        }

        public bool CanAttackOpponent(Creature creature)
        {
            IEnumerable<Creature> creaturesThatCannotAttackPlayers = GetContinuousEffects<CannotAttackPlayersEffect>().SelectMany(e => e.CreatureFilter.FilteredCreatures).Distinct();
            return !AffectedBySummoningSickness(creature) && !creaturesThatCannotAttackPlayers.Contains(creature);
        }

        public bool HasShieldTrigger(Card card)
        {
            if (card is Creature creature)
            {
                return HasShieldTrigger(creature);
            }
            else if (card is Spell spell)
            {
                return HasShieldTrigger(spell);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public bool AttacksIfAble(Creature creature)
        {
            return GetContinuousEffects<AttacksIfAbleEffect>().Any(e => e.CreatureFilter.FilteredCreatures.Contains(creature));
        }

        public bool HasSpeedAttacker(Creature creature)
        {
            return GetContinuousEffects<SpeedAttackerEffect>().Any(e => e.CreatureFilter.FilteredCreatures.Contains(creature));
        }

        public bool AffectedBySummoningSickness(Creature creature)
        {
            return creature.SummoningSickness && !HasSpeedAttacker(creature);
        }
        #endregion bool

        #region ReadOnlyCreatureCollection
        public ReadOnlyCreatureCollection GetCreaturesThatCanBlock(Creature attackingCreature)
        {
            return new ReadOnlyCreatureCollection(GetAllBlockersPlayerHasInTheBattleZone(GetOpponent(GetOwner(attackingCreature))).Where(c => !c.Tapped).ToList());
            //TODO: consider situations where abilities of attacking creature matter etc.
        }

        public ReadOnlyCreatureCollection GetCreaturesThatCanAttack(Player player)
        {
            List<Creature> creatures = player.BattleZone.UntappedCreatures.Where(creature => !AffectedBySummoningSickness(creature)).ToList();

            IEnumerable<Creature> creaturesThatCannotAttackPlayers = GetContinuousEffects<CannotAttackPlayersEffect>().SelectMany(e => e.CreatureFilter.FilteredCreatures).Distinct();
            IEnumerable<Creature> creaturesThatCannotAttack = creaturesThatCannotAttackPlayers.Where(c => GetCreaturesThatCanBeAttacked(player, c).Count == 0);
            /*
            foreach (CreatureContinuousEffect creatureContinuousEffect in cannotAttackPlayersEffects)
            {
                ReadOnlyCreatureCollection creaturesThatCannotAttackPlayers = creatureContinuousEffect.CreatureFilter.FilteredCreatures;
                creatures.RemoveAll(c => creaturesThatCannotAttackPlayers.Contains(c));
            }*/
            creatures.RemoveAll(c => creaturesThatCannotAttack.Contains(c));
            return new ReadOnlyCreatureCollection(creatures);
        }

        public ReadOnlyCreatureCollection GetCreaturesThatCanBeAttacked(Player player, Creature attackingCreature)
        {
            Player opponent = GetOpponent(player);
            return opponent.BattleZone.TappedCreatures;
            //TODO: improve
        }
        #endregion ReadOnlyCreatureCollection

        #region PlayerAction
        /// <summary>
        /// Starts a duel.
        /// </summary>
        public PlayerAction StartDuel(StartingPlayer startingPlayer = StartingPlayer.Player1)
        {
            Player activePlayer = Player1;
            Player nonActivePlayer = Player2;

            if (startingPlayer == StartingPlayer.Random)
            {
                const int RandomMax = 100;
                int randomNumber = new Random().Next(0, RandomMax);
                startingPlayer = (randomNumber % 2 == 0) ? StartingPlayer.Player1 : StartingPlayer.Player2;
            }

            if (startingPlayer == StartingPlayer.Player2)
            {
                activePlayer = Player2;
                nonActivePlayer = Player1;
            }
            else if (startingPlayer != StartingPlayer.Player1)
            {
                throw new InvalidOperationException();
            }

            activePlayer.ShuffleDeck();
            nonActivePlayer.ShuffleDeck();
            PutFromTheTopOfDeckIntoShieldZone(activePlayer, InitialNumberOfShields);
            PutFromTheTopOfDeckIntoShieldZone(nonActivePlayer, InitialNumberOfShields);

            //TODO: Animation
            DrawCards(activePlayer, InitialNumberOfHandCards);
            DrawCards(nonActivePlayer, InitialNumberOfHandCards);

            return SetCurrentPlayerAction(StartNewTurn(activePlayer, nonActivePlayer));
        }

        /// <summary>
        /// Player draws a card.
        /// </summary>
        public PlayerAction DrawCard(Player player)
        {
            DrawCards(player, 1);
            return null;
            //TODO: remove
            /*
            if (cards.Count == 1)
            {
                drawnCard = cards.First();
                return null;
            }
            else
            {
                throw new InvalidOperationException("drawnCard");
            }*/
        }

        /// <summary>
        /// Tries to progress in the duel based on the latest player action, and returns new player action for a player to take.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public PlayerAction Progress(PlayerActionResponse response)
        {
            PlayerAction playerAction = null;
            if (response is CardSelectionResponse cardSelectionResponse)
            {
                if (CurrentPlayerAction is OptionalCardSelection optionalCardSelection)
                {
                    Card card = null;
                    if (cardSelectionResponse.SelectedCards.Count == 1)
                    {
                        card = cardSelectionResponse.SelectedCards.First();
                    }
                    if (optionalCardSelection.Validate(card))
                    {
                        optionalCardSelection.SelectedCard = card;
                        playerAction = optionalCardSelection.Perform(this, card);
                        CurrentTurn.CurrentStep.PlayerActions.Add(optionalCardSelection);
                    }
                    else
                    {
                        return CurrentPlayerAction;
                    }
                }
                else if (CurrentPlayerAction is MandatoryMultipleCardSelection mandatoryMultipleCardSelection)
                {
                    if (CurrentPlayerAction is PayCost payCost)
                    {
                        if (payCost.Validate(cardSelectionResponse.SelectedCards) && PayCost.Validate(cardSelectionResponse.SelectedCards, (CurrentTurn.CurrentStep as MainStep).CardToBeUsed))
                        {
                            playerAction = payCost.Perform(this, cardSelectionResponse.SelectedCards);
                            CurrentTurn.CurrentStep.PlayerActions.Add(payCost);
                        }
                        else
                        {
                            return CurrentPlayerAction;
                        }
                    }
                    else
                    {
                        if (mandatoryMultipleCardSelection.Validate(cardSelectionResponse.SelectedCards))
                        {
                            playerAction = mandatoryMultipleCardSelection.Perform(this, cardSelectionResponse.SelectedCards);
                            CurrentTurn.CurrentStep.PlayerActions.Add(mandatoryMultipleCardSelection);
                        }
                        else
                        {
                            return CurrentPlayerAction;
                        }
                    }
                }
                else if (CurrentPlayerAction is MultipleCardSelection multipleCardSelection)
                {
                    if (multipleCardSelection.Validate(cardSelectionResponse.SelectedCards))
                    {
                        foreach (Card card in cardSelectionResponse.SelectedCards)
                        {
                            multipleCardSelection.SelectedCards.Add(card);
                        }
                        playerAction = multipleCardSelection.Perform(this, cardSelectionResponse.SelectedCards);
                        CurrentTurn.CurrentStep.PlayerActions.Add(multipleCardSelection);
                    }
                }
                else if (CurrentPlayerAction is MandatoryCardSelection mandatoryCardSelection)
                {
                    if (cardSelectionResponse.SelectedCards.Count == 1)
                    {
                        Card card = cardSelectionResponse.SelectedCards.First();
                        if (mandatoryCardSelection.Validate(card))
                        {
                            mandatoryCardSelection.SelectedCard = card;
                            playerAction = mandatoryCardSelection.Perform(this, card);
                            CurrentTurn.CurrentStep.PlayerActions.Add(mandatoryCardSelection);
                        }
                        else
                        {
                            throw new InvalidOperationException("mandatoryCardSelection");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("mandatoryCardSelection");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Could not identify current player action.");
                }
            }
            else if (response is CreatureSelectionResponse creatureSelectionResponse)
            {
                if (CurrentPlayerAction is OptionalCreatureSelection optionalCreatureSelection)
                {
                    Creature creature = null;
                    if (creatureSelectionResponse.SelectedCreatures.Count == 1)
                    {
                        creature = creatureSelectionResponse.SelectedCreatures.First();
                    }
                    if (optionalCreatureSelection.Validate(creature))
                    {
                        optionalCreatureSelection.SelectedCreature = creature;
                        playerAction = optionalCreatureSelection.Perform(this, creature);
                        CurrentTurn.CurrentStep.PlayerActions.Add(optionalCreatureSelection);
                    }
                    else
                    {
                        return CurrentPlayerAction;
                    }
                }
                else if (CurrentPlayerAction is MandatoryCreatureSelection mandatoryCreatureSelection)
                {
                    if (creatureSelectionResponse.SelectedCreatures.Count == 1)
                    {
                        Creature creature = creatureSelectionResponse.SelectedCreatures.First();
                        if (mandatoryCreatureSelection.Validate(creature))
                        {
                            mandatoryCreatureSelection.SelectedCreature = creature;
                            playerAction = mandatoryCreatureSelection.Perform(this, creature);
                            CurrentTurn.CurrentStep.PlayerActions.Add(mandatoryCreatureSelection);
                        }
                        else
                        {
                            throw new InvalidOperationException("mandatoryCreatureSelection");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("mandatoryCreatureSelection");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Could not identify current player action.");
                }
            }
            else if (response is OptionalActionResponse optionalActionResponse)
            {
                if (CurrentPlayerAction is OptionalAction optionalAction)
                {
                    playerAction = optionalAction.Perform(this, optionalActionResponse.TakeAction);
                    CurrentTurn.CurrentStep.PlayerActions.Add(optionalAction);
                }
                else
                {
                    throw new InvalidOperationException("optionalActionResponse");
                }
            }
            else if (response is SelectAbilityToResolveResponse selectAbilityToResolveResponse)
            {
                if (CurrentPlayerAction is SelectAbilityToResolve selectAbilityToResolve)
                {
                    selectAbilityToResolve.SelectedAbility = selectAbilityToResolveResponse.Ability;
                    SelectAbilityToResolve.Perform(this, selectAbilityToResolveResponse.Ability);
                    CurrentTurn.CurrentStep.PlayerActions.Add(selectAbilityToResolve);
                }
                else
                {
                    throw new InvalidOperationException("optionalActionResponse");
                }
            }
            //TODO SelectAbilityToResolveResponse
            //else if (response if )
            else
            {
                throw new ArgumentOutOfRangeException("response");
            }
            return playerAction == null ? SetCurrentPlayerAction(Progress()) : SetCurrentPlayerAction(TryToPerformAutomatically(playerAction));
        }

        public PlayerAction PutFromShieldZoneToHand(Player player, Card card, bool canUseShieldTrigger)
        {
            return PutFromShieldZoneToHand(player, new ReadOnlyCardCollection(card), canUseShieldTrigger);
        }

        public PlayerAction PutFromShieldZoneToHand(Player player, ReadOnlyCardCollection cards, bool canUseShieldTrigger)
        {
            CardCollection shieldTriggerCards = new CardCollection();
            for (int i = 0; i < cards.Count; ++i)
            {
                Card card = cards[i];
                PutFromShieldZoneToHand(player, card);
                if (canUseShieldTrigger && HasShieldTrigger(card))
                {
                    shieldTriggerCards.Add(card);
                }
            }
            return shieldTriggerCards.Count > 0 ? new DeclareShieldTriggers(player, new ReadOnlyCardCollection(shieldTriggerCards)) : null;
        }

        public PlayerAction PutTheTopCardOfYourDeckIntoYourManaZone(Player player)
        {
            player.ManaZone.Add(RemoveTheTopCardOfDeck(player), this);
            return null;
        }

        public PlayerAction PutTheTopCardOfYourDeckIntoYourManaZoneThenAddTheTopCardOfYourDeckToYourShieldsFaceDown(Player player)
        {
            player.ManaZone.Add(RemoveTheTopCardOfDeck(player), this);
            return new AddTheTopCardOfYourDeckToYourShieldsFaceDown(player);
        }

        public PlayerAction ReturnFromBattleZoneToHand(Creature creature)
        {
            Player owner = GetOwner(creature);
            owner.BattleZone.Remove(creature, this);
            owner.Hand.Add(creature, this);
            return null;
        }

        public PlayerAction PutFromBattleZoneIntoOwnersManazone(Creature creature)
        {
            Player owner = GetOwner(creature);
            owner.BattleZone.Remove(creature, this);
            owner.ManaZone.Add(creature, this);
            return null;
        }

        public PlayerAction PutFromManaZoneIntoTheBattleZone(Creature creature)
        {
            Player owner = GetOwner(creature);
            owner.ManaZone.Remove(creature, this);
            owner.BattleZone.Add(creature, this);
            return null;
        }
        #endregion PlayerAction

        public int GetPower(Creature creature)
        {
            //return creature.Power + GetContinuousEffects().Where(e => e is PowerEffect).Cast<PowerEffect>().Where(e => e.CreatureFilter.FilteredCreatures.Contains(creature)).Sum(e => e.Power);
            return creature.Power + GetContinuousEffects<PowerEffect>().Where(e => e.CreatureFilter.FilteredCreatures.Contains(creature)).Sum(e => e.Power);
        }

        public Card GetCard(int gameId)
        {
            return GetAllCards().First(c => c.GameId == gameId);
        }
        #endregion Public methods

        #region Internal methods
        internal void EndContinuousEffects(Type type)
        {
            List<ContinuousEffect> suitableContinuousEffects = ContinuousEffects.Where(c => c.Period.GetType() == type).ToList();
            while (suitableContinuousEffects.Count() > 0)
            {
                ContinuousEffects.Remove(suitableContinuousEffects.First());
                suitableContinuousEffects.Remove(suitableContinuousEffects.First());
            }
        }

        internal void AddContinuousEffect(ContinuousEffect continuousEffect)
        {
            ContinuousEffects.Add(continuousEffect);
        }

        internal PlayerAction AddTheTopCardOfYourDeckToYourShieldsFaceDown(Player player)
        {
            PutFromTheTopOfDeckIntoShieldZone(player, 1);
            return null;
        }
        #endregion Internal methods

        #region Private methods
        #region void
        ///<summary>
        /// Removes the top cards from a player's deck and puts them into their shield zone.
        ///</summary>
        private void PutFromTheTopOfDeckIntoShieldZone(Player player, int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                player.ShieldZone.Add(RemoveTheTopCardOfDeck(player), this);
            }
        }

        /// <summary>
        /// A player draws a number of cards.
        /// </summary>
        private void DrawCards(Player player, int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                Card drawnCard = RemoveTheTopCardOfDeck(player);
                player.Hand.Add(drawnCard, this);
                if (Turns.Count > 0)
                {
                    CurrentTurn.CurrentStep.DrawnCards.Add(drawnCard);
                }
                else
                {
                    CardsDrawnAtTheStartOfDuel.Add(drawnCard);
                }
                //TODO: think how to animate draw at start of game
            }
        }

        /// <summary>
        /// 703.3. The game always checks for any of the listed conditions for state-based actions, then performs all applicable state-based actions simultaneously as a single event. If any state-based actions are performed as a result of a check, the check is repeated.
        /// </summary>
        private void CheckStateBasedActions()
        {
            CheckDeckHasCards checkDeckHasCards = new CheckDeckHasCards();
            checkDeckHasCards.Perform(this);
        }

        private void PutFromBattleZoneIntoGraveyard(Player player, Creature creature)
        {
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }
            if (creature == null)
            {
                throw new ArgumentNullException("creature");
            }
            player.BattleZone.Remove(creature, this);
            player.Graveyard.Add(creature, this);
        }

        private void PutFromShieldZoneToHand(Player player, Card card)
        {
            player.ShieldZone.Remove(card, this);
            player.Hand.Add(card, this);
        }

        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion void

        #region PlayerAction
        private PlayerAction TryToPerformAutomatically(PlayerAction playerAction)
        {
            PlayerAction newPlayerAction = playerAction.TryToPerformAutomatically(this);
            if (playerAction == newPlayerAction)
            {
                //Player action was not performed automatically.
                if (playerAction.Player is AIPlayer aiPlayer)
                {
                    PlayerAction aiAction = aiPlayer.PerformPlayerAction(this, newPlayerAction);
                    return aiAction != null ? TryToPerformAutomatically(aiAction) : Progress();
                }
                else
                {
                    return playerAction;
                }
            }
            else
            {
                return newPlayerAction != null ? TryToPerformAutomatically(newPlayerAction) : Progress();
            }
        }

        /// <summary>
        /// Progresses in the duel.
        /// </summary>
        /// <returns>A player request for a player to perform an action. Returns null if there is nothing left to do in the duel.</returns>
        private PlayerAction Progress()
        {
            if (!Ended)
            {
                CheckStateBasedActions();
                if (!Ended)
                {
                    foreach (Player player in new List<Player>() { CurrentTurn.ActivePlayer, CurrentTurn.NonActivePlayer })
                    {
                        if (player.ShieldTriggersToUse.Count > 0)
                        {
                            return TryToPerformAutomatically(new UseShieldTrigger(player, new ReadOnlyCardCollection(player.ShieldTriggersToUse)));
                        }
                    }

                    if (AbilityBeingResolved != null)
                    {
                        //PlayerAction playerActionFromNonStaticAbility = AbilityBeingResolved.ContinueResolution(this);
                        PlayerActionWithEndInformation action = AbilityBeingResolved.ContinueResolution(this);
                        if (!action.End)
                        {
                            //TODO: test
                            return action.PlayerAction != null ? TryToPerformAutomatically(action.PlayerAction) : Progress();
                        }
                        else
                        {
                            if (AbilityBeingResolved is SpellAbility)
                            {
                                Spell spell = SpellsBeingResolved.Last();
                                SpellsBeingResolved.Remove(spell);
                                GetOwner(spell).Graveyard.Add(spell, this);
                            }

                            AbilityBeingResolved = null;
                        }
                    }

                    if (SpellsBeingResolved.Count > 0)
                    {
                        Spell spell = SpellsBeingResolved.Last();
                        if (spell.SpellAbilities.Count > 0)
                        {
                            //TODO: spell may have more than one spell ability.
                            SpellAbility spellAbility = spell.SpellAbilities.First();
                            AbilityBeingResolved = spellAbility;
                            return Progress();
                        }
                        else
                        {
                            SpellsBeingResolved.Remove(spell);
                            GetOwner(spell).Graveyard.Add(spell, this);
                        }
                    }

                    if (PendingAbilitiesForActivePlayer.Count > 0)
                    {
                        return TryToPerformAutomatically(new SelectAbilityToResolve(CurrentTurn.ActivePlayer, PendingAbilitiesForActivePlayer));
                    }

                    if (PendingAbilitiesForNonActivePlayer.Count > 0)
                    {
                        return TryToPerformAutomatically(new SelectAbilityToResolve(CurrentTurn.NonActivePlayer, PendingAbilitiesForNonActivePlayer));
                    }

                    /*foreach (Collection<NonStaticAbility> pendingAbilities in new List<Collection<NonStaticAbility>>() { PendingAbilitiesForActivePlayer, PendingAbilitiesForNonActivePlayer })
                    {
                        SelectAbilityToResolve selectAbilityToResolve = new SelectAbilityToResolve(pendingAbilities.First().Controller, pendingAbilities);
                        return TryToPerformAutomatically(selectAbilityToResolve);
                    }*/

                    PlayerAction playerAction = CurrentTurn.CurrentStep.PlayerActionRequired(this);
                    if (playerAction != null)
                    {
                        return TryToPerformAutomatically(playerAction);
                    }
                    else
                    {
                        if (CurrentTurn.ChangeStep())
                        {
                            return StartNewTurn(CurrentTurn.NonActivePlayer, CurrentTurn.ActivePlayer);
                        }
                        else
                        {
                            PlayerAction action = CurrentTurn.CurrentStep.ProcessTurnBasedActions(this);
                            return action != null ? TryToPerformAutomatically(action) : Progress();
                        }
                    }
                }
            }
            return null;
        }

        private PlayerAction SetCurrentPlayerAction(PlayerAction playerAction)
        {
            CurrentPlayerAction = playerAction;
            return playerAction;
        }

        /// <summary>
        /// Creates a new turn and starts it.
        /// </summary>
        private PlayerAction StartNewTurn(Player activePlayer, Player nonActivePlayer)
        {
            Turn turn = new Turn(activePlayer, nonActivePlayer, Turns.Count + 1);
            Turns.Add(turn);
            PlayerAction playerAction = turn.Start(this);
            return playerAction != null ? TryToPerformAutomatically(playerAction) : Progress();
        }
        #endregion PlayerAction

        #region bool
        /// <summary>
        /// Checks if selected mana cards have the required civilizations.
        /// </summary>
        private static bool HasCivilizations(ReadOnlyCardCollection paymentCards, ReadOnlyCivilizationCollection requiredCivilizations)
        {
            if (paymentCards == null)
            {
                throw new ArgumentNullException("paymentCards");
            }
            else if (requiredCivilizations == null)
            {
                throw new ArgumentNullException("requiredCivilizations");
            }
            else
            {
                List<List<Civilization>> civilizationGroups = new List<List<Civilization>>();
                foreach (Card card in paymentCards)
                {
                    civilizationGroups.Add(card.Civilizations.ToList());
                }
                List<List<Civilization>> testi = GetCivilizationCombinations(civilizationGroups, new List<Civilization>());
                IEnumerable<IEnumerable<Civilization>> combinations = testi.Select(combination => combination.Distinct());
                foreach (IEnumerable<Civilization> combination in combinations)
                {
                    for (int i = 0; i < requiredCivilizations.Count; ++i)
                    {
                        if (!combination.Contains(requiredCivilizations[i]))
                        {
                            break;
                        }
                        else if (requiredCivilizations.Count - 1 == i)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private bool HasShieldTrigger(Creature creature)
        {
            foreach (CreatureContinuousEffect creatureContinuousEffect in GetContinuousEffects().Where(e => e is CreatureShieldTriggerEffect).Cast<CreatureShieldTriggerEffect>())
            {
                if (creatureContinuousEffect.CreatureFilter.FilteredCreatures.Contains(creature))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasShieldTrigger(Spell spell)
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
        #endregion bool

        /// <summary>
        /// Removes the top card from a player's deck and returns it.
        /// </summary>
        private Card RemoveTheTopCardOfDeck(Player player)
        {
            return player.Deck.RemoveAndGetTopCard(this);
        }

        private static List<List<Civilization>> GetCivilizationCombinations(List<List<Civilization>> civilizationGroups, List<Civilization> knownCivilizations)
        {
            if (civilizationGroups.Count > 0)
            {
                List<Civilization> civilizations = civilizationGroups.First();
                List<List<Civilization>> newCivilizationGroups = new List<List<Civilization>>(civilizationGroups);
                newCivilizationGroups.Remove(civilizations);
                List<List<Civilization>> output = new List<List<Civilization>>();
                foreach (Civilization civilization in civilizations)
                {
                    List<Civilization> newKnownCivilizations = new List<Civilization>(knownCivilizations) { civilization };

                    output.AddRange(
                        GetCivilizationCombinations(
                            newCivilizationGroups,
                            newKnownCivilizations));
                }
                return output;
            }
            else
            {
                List<Civilization> copyOfKnownCivilizations = new List<Civilization>(knownCivilizations);
                return new List<List<Civilization>> { copyOfKnownCivilizations };
            }
        }

        private ReadOnlyCardCollection GetAllCards()
        {
            List<Card> cards = Player1.DeckBeforeDuel.ToList();
            cards.AddRange(Player2.DeckBeforeDuel);
            return new ReadOnlyCardCollection(cards);
        }

        private ReadOnlyContinuousEffectCollection GetContinuousEffects()
        {
            List<ContinuousEffect> continuousEffects = ContinuousEffects.ToList();
            foreach (Card card in GetAllCards())
            {
                continuousEffects.AddRange(GetContinuousEffectsGeneratedByCard(card));
            }
            return new ReadOnlyContinuousEffectCollection(continuousEffects);
        }

        private IEnumerable<T> GetContinuousEffects<T>()
        {
            return GetContinuousEffects().Where(e => e is T).Cast<T>();
        }

        private List<ContinuousEffect> GetContinuousEffectsGeneratedByCard(Card card)
        {
            List<ContinuousEffect> continuousEffects = new List<ContinuousEffect>();
            foreach (StaticAbility staticAbility in card.StaticAbilities)
            {
                continuousEffects.AddRange(GetContinuousEffectsGeneratedByStaticAbility(card, staticAbility));
            }
            return continuousEffects;
        }

        private ReadOnlyContinuousEffectCollection GetContinuousEffectsGeneratedByStaticAbility(Card card, StaticAbility staticAbility)
        {
            if (staticAbility is StaticAbilityForCreature staticAbilityForCreature)
            {
                if (staticAbilityForCreature.EffectActivityCondition == EffectActivityConditionForCreature.Anywhere ||
                    (staticAbilityForCreature.EffectActivityCondition == EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone && GetOwner(card).BattleZone.Cards.Contains(card)) ||
                    (staticAbilityForCreature.EffectActivityCondition == EffectActivityConditionForCreature.WhileThisCreatureIsInYourHand && GetOwner(card).Hand.Cards.Contains(card)))
                {
                    return staticAbilityForCreature.ContinuousEffects;
                }
                else
                {
                    return new ReadOnlyContinuousEffectCollection();
                }
            }
            else if (staticAbility is StaticAbilityForSpell staticAbilityForSpell)
            {
                if (staticAbilityForSpell.EffectActivityCondition == StaticAbilityForSpellActivityCondition.WhileThisSpellIsInYourHand && GetOwner(card).Hand.Cards.Contains(card))
                {
                    return staticAbilityForSpell.ContinuousEffects;
                }
                else
                {
                    return new ReadOnlyContinuousEffectCollection();
                }
            }
            else
            {
                throw new InvalidOperationException("staticAbility");
            }
        }

        private ReadOnlyCreatureCollection GetAllBlockersPlayerHasInTheBattleZone(Player player)
        {
            List<Creature> blockers = new List<Creature>();
            IEnumerable<BlockerEffect> blockerEffects = GetContinuousEffects().Where(e => e is BlockerEffect).Cast<BlockerEffect>();
            /*foreach (CreatureContinuousEffect creatureContinuousEffect in blockerEffects)
            {

                blockers.AddRange(player.BattleZone.Creatures.Where( creatureContinuousEffect.CreatureFilter.FilteredCreatures);
            }*/
            foreach (Creature creature in player.BattleZone.Creatures)
            {
                foreach (CreatureContinuousEffect creatureContinuousEffect in blockerEffects)
                {
                    if (creatureContinuousEffect.CreatureFilter.FilteredCreatures.Contains(creature))
                    {
                        blockers.Add(creature);
                    }
                }
            }
            return new ReadOnlyCreatureCollection(blockers);
        }
        #endregion Private methods
    }
}
