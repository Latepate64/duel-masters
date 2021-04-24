using DuelMastersModels.Abilities.StaticAbilities;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Zones;
using System.Collections.Generic;
using DuelMastersModels.Managers;
using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels
{
    /// <summary>
    /// Interface for people playing Duel Masters.
    /// </summary>
    public interface IPlayer
    {
        int ID { get; set; }

        /// <summary>
        /// The name of the player.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// When a game begins, each player’s deck becomes their deck.
        /// </summary>
        Deck Deck { get; set; }

        /// <summary>
        /// A player’s graveyard is their discard pile. Discarded cards, destroyed creatures and spells cast are put in their owner's graveyard.
        /// </summary>
        Graveyard Graveyard { get; }

        /// <summary>
        /// The hand is where a player holds cards that have been drawn. Cards can be put into a player’s hand by other effects as well. At the beginning of the game, each player draws five cards.
        /// </summary>
        Hand Hand { get; }

        /// <summary>
        /// The mana zone is where cards are put in order to produce mana for using other cards. All cards are put into the mana zone upside down. However, multicolored cards are put into the mana zone tapped.
        /// </summary>
        ManaZone ManaZone { get; }

        /// <summary>
        /// At the beginning of the game, each player puts five shields into their shield zone. Castles are put into the shield zone to fortify a shield.
        /// </summary>
        ShieldZone ShieldZone { get; }

        IEnumerable<ICard> CardsInNonsharedZones { get; }

        IEnumerable<ICard> ShieldTriggersToUse { get; }

        IPlayer Opponent { get; set; }

        EventManager EventManager { get; set; }

        void AddShieldTriggerToUse(ICard card);
        void DrawCards(int amount);
        ReadOnlyContinuousEffectCollection GetContinuousEffectsGeneratedByStaticAbility(ICard card, IStaticAbility staticAbility, BattleZone battleZone);
        void PutFromBattleZoneIntoGraveyard(ICard card, BattleZone battleZone);
        void PutFromHandIntoManaZone(ICard card);
        void PutFromTopOfDeckIntoShieldZone(int amount);
        void RemoveShieldTriggerToUse(ICard card);
        ICard RemoveTopCardOfDeck();
        void ShuffleDeck();
        IChoice UntapCardsInBattleZoneAndManaZone(BattleZone battleZone);
        IChoice Use(ICard card, IEnumerable<ICard> manaCards);
    }
}
