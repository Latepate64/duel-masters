using DuelMastersModels.Abilities.StaticAbilities;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.Factories;
using DuelMastersModels.Managers;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using DuelMastersInterfaceModels.Events;

namespace DuelMastersModels
{
    /// <summary>
    /// Players are the two people that are participating in the duel. The player during the current turn is known as the "active player" and the other player is known as the "non-active player".
    /// </summary>
    public class Player : IPlayer
    {
        public int ID { get; }

        /// <summary>
        /// The name of the player.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// When a game begins, each player’s deck becomes their deck.
        /// </summary>
        public IDeck Deck { get; set; }

        /// <summary>
        /// A player’s graveyard is their discard pile. Discarded cards, destroyed creatures and spells cast are put in their owner's graveyard.
        /// </summary>
        public IGraveyard Graveyard { get; private set; } = new Graveyard();

        /// <summary>
        /// The hand is where a player holds cards that have been drawn. Cards can be put into a player’s hand by other effects as well. At the beginning of the game, each player draws five cards.
        /// </summary>
        public IHand Hand { get; private set; } = new Hand();

        /// <summary>
        /// The mana zone is where cards are put in order to produce mana for using other cards. All cards are put into the mana zone upside down. However, multicolored cards are put into the mana zone tapped.
        /// </summary>
        public IManaZone ManaZone { get; private set; } = new ManaZone();

        /// <summary>
        /// At the beginning of the game, each player puts five shields into their shield zone. Castles are put into the shield zone to fortify a shield.
        /// </summary>
        public IShieldZone ShieldZone { get; private set; } = new ShieldZone();

        public IEnumerable<IHandCard> ShieldTriggersToUse => _shieldTriggerManager.ShieldTriggersToUse;

        public IEnumerable<ICard> CardsInNonsharedZones
        {
            get
            {
                List<ICard> cards = new List<ICard>();
                cards.AddRange(Deck.Cards);
                cards.AddRange(Graveyard.Cards);
                cards.AddRange(Hand.Cards);
                cards.AddRange(ManaZone.Cards);
                cards.AddRange(ShieldZone.Cards);
                return cards;
            }
        }

        public IPlayer Opponent { get; set; }

        public EventManager EventManager { get; set; }

        /// <summary>
        /// Player shuffles their deck.
        /// </summary>
        public void ShuffleDeck()
        {
            Deck.Shuffle();
            EventManager?.Raise(new ShuffleDeckEvent(ID));
        }

        public void AddShieldTriggerToUse(IHandCard card)
        {
            _shieldTriggerManager.AddShieldTriggerToUse(card);
        }

        public IChoice Use(IHandCard card, IEnumerable<IManaZoneCard> manaCards)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            else if (manaCards == null)
            {
                throw new ArgumentNullException(nameof(manaCards));
            }
            //return Duel.Progress();
            throw new NotImplementedException("Consider mana payment");
        }

        public void RemoveShieldTriggerToUse(IHandCard card)
        {
            _shieldTriggerManager.RemoveShieldTriggerToUse(card);
        }

        public ReadOnlyContinuousEffectCollection GetContinuousEffectsGeneratedByStaticAbility(ICard card, IStaticAbility staticAbility, IBattleZone battleZone)
        {
            if (staticAbility is StaticAbilityForCreature staticAbilityForCreature)
            {
                return staticAbilityForCreature.EffectActivityCondition == EffectActivityConditionForCreature.Anywhere ||
                    (staticAbilityForCreature.EffectActivityCondition == EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone && card is IBattleZoneCard battleZoneCard && battleZone.Cards.Contains(battleZoneCard)) ||
                    (staticAbilityForCreature.EffectActivityCondition == EffectActivityConditionForCreature.WhileThisCreatureIsInYourHand && card is IHandCreature handCreature && Hand.Cards.Contains(handCreature))
                    ? staticAbilityForCreature.ContinuousEffects
                    : new ReadOnlyContinuousEffectCollection();
            }
            else if (staticAbility is StaticAbilityForSpell staticAbilityForSpell)
            {
                return staticAbilityForSpell.EffectActivityCondition == StaticAbilityForSpellActivityCondition.WhileThisSpellIsInYourHand && card is IHandSpell handSpell && Hand.Cards.Contains(handSpell)
                    ? staticAbilityForSpell.ContinuousEffects
                    : new ReadOnlyContinuousEffectCollection();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void PutFromBattleZoneIntoGraveyard(IBattleZoneCard card, IBattleZone battleZone)
        {
            battleZone.Remove(card);
            Graveyard.Add(CardFactory.GenerateGraveyardCard(card));
        }

        /// <summary>
        /// Player puts target card from their hand into their mana zone.
        /// </summary>
        /// <param name="card"></param>
        public void PutFromHandIntoManaZone(IHandCard card)
        {
            Hand.Remove(card);
            ManaZone.Add(CardFactory.GenerateManaZoneCard(card));
        }

        ///<summary>
        /// Removes the top cards from a player's deck and puts them into their shield zone.
        ///</summary>
        public void PutFromTopOfDeckIntoShieldZone(int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                ShieldZone.Add(CardFactory.GenerateShieldZoneCard(RemoveTopCardOfDeck(), false));
            }
        }

        /// <summary>
        /// Removes the top card from a player's deck and returns it.
        /// </summary>
        public ICard RemoveTopCardOfDeck()
        {
            return Deck.RemoveAndGetTopCard();
        }

        /// <summary>
        /// Player draws a number of cards.
        /// </summary>
        public void DrawCards(int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                ICard drawnCard = RemoveTopCardOfDeck();
                IHandCard handCard = CardFactory.GenerateHandCard(drawnCard);
                Hand.Add(handCard);

                //TODO: Uncomment
                //DuelEventOccurred?.Invoke(this, new DuelEventArgs(new DrawCardEvent(this, handCard)));
            }
        }

        public IChoice UntapCardsInBattleZoneAndManaZone(IBattleZone battleZone)
        {
            battleZone.UntapCards();
            ManaZone.UntapCards();
            return null; //TODO: Could require choice (eg. Silent Skill)
        }

        private readonly ShieldTriggerManager _shieldTriggerManager = new ShieldTriggerManager();
    }
}
