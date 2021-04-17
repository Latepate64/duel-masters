using DuelMastersModels.Abilities.StaticAbilities;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Zones;
using System.Collections.Generic;

namespace DuelMastersModels
{
    /// <summary>
    /// Interface for people playing Duel Masters.
    /// </summary>
    public interface IPlayer
    {
        int ID { get; }

        /// <summary>
        /// The name of the player.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// When a game begins, each player’s deck becomes their deck.
        /// </summary>
        IDeck Deck { get; set; }

        /// <summary>
        /// A player’s graveyard is their discard pile. Discarded cards, destroyed creatures and spells cast are put in their owner's graveyard.
        /// </summary>
        IGraveyard Graveyard { get; }

        /// <summary>
        /// The hand is where a player holds cards that have been drawn. Cards can be put into a player’s hand by other effects as well. At the beginning of the game, each player draws five cards.
        /// </summary>
        IHand Hand { get; }

        /// <summary>
        /// The mana zone is where cards are put in order to produce mana for using other cards. All cards are put into the mana zone upside down. However, multicolored cards are put into the mana zone tapped.
        /// </summary>
        IManaZone ManaZone { get; }

        /// <summary>
        /// At the beginning of the game, each player puts five shields into their shield zone. Castles are put into the shield zone to fortify a shield.
        /// </summary>
        IShieldZone ShieldZone { get; }

        IEnumerable<ICard> CardsInNonsharedZones { get; }

        IEnumerable<IHandCard> ShieldTriggersToUse { get; }

        IPlayer Opponent { get; set; }

        void AddShieldTriggerToUse(IHandCard card);
        void DrawCards(int amount);
        ReadOnlyContinuousEffectCollection GetContinuousEffectsGeneratedByStaticAbility(ICard card, IStaticAbility staticAbility, IBattleZone battleZone);
        void PutFromBattleZoneIntoGraveyard(IBattleZoneCard card, IBattleZone battleZone);
        void PutFromHandIntoManaZone(IHandCard card);
        void PutFromTopOfDeckIntoShieldZone(int amount);
        void RemoveShieldTriggerToUse(IHandCard card);
        ICard RemoveTopCardOfDeck();
        void ShuffleDeck();
        IChoice UntapCardsInBattleZoneAndManaZone(IBattleZone battleZone);
        IChoice Use(IHandCard card, IEnumerable<IManaZoneCard> manaCards);
    }
}
