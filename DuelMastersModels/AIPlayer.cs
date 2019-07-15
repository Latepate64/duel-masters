﻿using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.CardSelections;
using DuelMastersModels.PlayerActions.CreatureSelections;
using DuelMastersModels.PlayerActions.OptionalActions;
using System;
using System.Linq;

namespace DuelMastersModels
{
    public class AIPlayer : Player
    {
        public PlayerAction PerformPlayerAction(Duel duel, PlayerAction playerAction)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            else if (playerAction is CardSelection cardSelection)
            {
                return SelectCard(duel, cardSelection);
            }
            else if (playerAction is CreatureSelection creatureSelection)
            {
                if (creatureSelection is OptionalCreatureSelection optionalCreatureSelection)
                {
                    Creature creature = null;
                    if (optionalCreatureSelection.Creatures.Count > 0)
                    {
                        creature = optionalCreatureSelection.Creatures.First();
                    }
                    optionalCreatureSelection.SelectedCreature = creature;
                    PlayerAction newAction = optionalCreatureSelection.Perform(duel, creature);
                    duel.CurrentTurn.CurrentStep.PlayerActions.Add(optionalCreatureSelection);
                    return newAction;
                }
                else
                {
                    throw new InvalidOperationException();
                }   
            }
            else if (playerAction is OptionalAction optionalAction)
            {
                return optionalAction.Perform(duel, true);
            }
            else
            {
                throw new ArgumentOutOfRangeException("playerAction");
            }
        }

        private PlayerAction SelectCard(Duel duel, CardSelection cardSelection)
        {
            PlayerAction newAction = null;
            if (cardSelection is OptionalCardSelection optionalCardSelection)
            {
                if (optionalCardSelection is ChargeMana chargeMana)
                {
                    Card card = null;
                    if (Hand.Cards.Sum(c => c.Cost) > ManaZone.UntappedCards.Count)
                    {
                        card = chargeMana.Cards.First();
                    }
                    chargeMana.SelectedCard = card;
                    newAction = chargeMana.Perform(duel, card);
                }
                else
                {
                    Card card = null;
                    if (optionalCardSelection.Cards.Count > 0)
                    {
                        card = optionalCardSelection.Cards.First();
                    }
                    optionalCardSelection.SelectedCard = card;
                    newAction = optionalCardSelection.Perform(duel, card);
                }
            }
            else if (cardSelection is PayCost payCost)
            {
                Card civCard = payCost.Player.ManaZone.Cards.First(c => !c.Tapped && c.Civilizations.Intersect((duel.CurrentTurn.CurrentStep as Steps.MainStep).CardToBeUsed.Civilizations).Any());
                System.Collections.Generic.List<Card> manaCards = payCost.Player.ManaZone.Cards.Where(c => !c.Tapped && c != civCard).Take(payCost.Cost - 1).ToList();
                manaCards.Add(civCard);
                newAction = payCost.Perform(duel, new System.Collections.ObjectModel.Collection<Card>(manaCards));
            }
            else if (cardSelection is MultipleCardSelection multipleCardSelection)
            {
                foreach (Card card in multipleCardSelection.Cards)
                {
                    multipleCardSelection.SelectedCards.Add(card);
                }
                newAction = multipleCardSelection.Perform(duel, multipleCardSelection.Cards);
            }
            else if (cardSelection is MandatoryCardSelection mandatoryCardSelection)
            {
                Card card = mandatoryCardSelection.Cards.First();
                mandatoryCardSelection.SelectedCard = card;
                newAction = mandatoryCardSelection.Perform(duel, card);
            }
            else
            {
                throw new ArgumentOutOfRangeException("cardSelection");
            }
            duel.CurrentTurn.CurrentStep.PlayerActions.Add(cardSelection);
            return newAction;
        }
    }
}
