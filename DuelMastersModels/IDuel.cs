using DuelMastersModels.Abilities;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.Zones;
using System.Collections.Generic;
using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Cards;

namespace DuelMastersModels
{
    /// <summary>
    /// Interface for Duel Masters duels.
    /// </summary>
    public interface IDuel
    {
        /// <summary>
        /// A player that participates in duel against player 2.
        /// </summary>
        IPlayer Player1 { get; set; }

        /// <summary>
        /// A player that participates in duel against player 1.
        /// </summary>
        IPlayer Player2 { get; set; }

        /// <summary>
        /// 103.1. At the start of a game, the players determine which one of them will choose who takes the first turn. In the first game of a match (including a single-game match), the players may use any mutually agreeable method (flipping a coin, rolling dice, etc.) to do so. In a match of several games, the loser of the previous game chooses who takes the first turn. If the previous game was a draw, the player who made the choice in that game makes the choice in this game. The player chosen to take the first turn is the starting player. The game’s default turn order begins with the starting player and proceeds clockwise.
        /// </summary>
        IPlayer StartingPlayer { get; set; }

        /// <summary>
        /// Player who won the duel.
        /// </summary>
        IPlayer Winner { get; }

        /// <summary>
        /// Determines the state of the duel.
        /// </summary>
        DuelState State { get; set; }

        /// <summary>
        /// The number of shields each player has at the start of a duel. 
        /// </summary>
        int InitialNumberOfShields { get; }

        /// <summary>
        /// The number of cards each player draw at the start of a duel.
        /// </summary>
        int InitialNumberOfHandCards { get; }

        /// <summary>
        /// Determines which player goes first in the duel.
        /// </summary>
        StartingPlayerMethod StartingPlayerMethod { get; }

        /// <summary>
        /// The turn that is currently being processed.
        /// </summary>
        ITurn CurrentTurn { get; }

        /// <summary>
        /// Battle Zone is the main place of the game. Creatures, Cross Gears, Weapons, Fortresses, Beats and Fields are put into the battle zone, but no mana, shields, castles nor spells may be put into the battle zone.
        /// </summary>
        BattleZone BattleZone { get; }

        /// <summary>
        /// Starts the duel.
        /// </summary>
        /// <returns></returns>
        IChoice Start();

        int GetPower(ICreature creature);
        IChoice DrawCard(IPlayer player);
        IChoice PutFromShieldZoneToHand(IPlayer player, ICard card, bool canUseShieldTrigger);
        IChoice PutFromShieldZoneToHand(IPlayer player, IEnumerable<ICard> cards, bool canUseShieldTrigger);
        IChoice PutTheTopCardOfYourDeckIntoYourManaZone(IPlayer player);
        IChoice AddTheTopCardOfYourDeckToYourShieldsFaceDown(IPlayer player);
        void End(IPlayer winner);
        void EndDuelInDraw();
        void UseCard(IPlayer player, ICard card);
        void EndContinuousEffects<T>();
        void AddContinuousEffect(IContinuousEffect continuousEffect);
        IEnumerable<ICreature> GetCreaturesThatCanBlock(Creature attackingCreature);
        IEnumerable<ICreature> GetCreaturesThatCanAttack(IPlayer player);
        IEnumerable<ICreature> GetCreaturesThatCanBeAttacked(IPlayer player);
        bool CanAttackOpponent(ICreature creature);
        bool AttacksIfAble(ICreature creature);
        IEnumerable<ICard> GetAllCards();
        void SetPendingAbilityToBeResolved(INonStaticAbility ability);
        void TriggerWhenYouPutThisCreatureIntoTheBattleZoneAbilities(ICreature creature);
        void TriggerWheneverAnotherCreatureIsPutIntoTheBattleZoneAbilities(ICreature excludedCreature);
        IChoice StartNewTurn(IPlayer activePlayer);
    }
}