using DuelMastersModels.Cards;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.CardSelections;
using DuelMastersModels.PlayerActions.CreatureSelections;
using DuelMastersModels.PlayerActions.OptionalActions;
using System;
using System.Collections.Generic;
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
                PlayerAction newAction;
                if (creatureSelection is OptionalCreatureSelection optionalCreatureSelection)
                {
                    GameCreature creature = null;
                    if (optionalCreatureSelection is DeclareTargetOfAttack declareTargetOfAttack)
                    {
                        List<int> listOfPoints = new List<int>();
                        GameCreature attacker = (duel.CurrentTurn.CurrentStep as Steps.AttackDeclarationStep).AttackingCreature;
                        foreach (GameCreature targetOfAttack in declareTargetOfAttack.Creatures)
                        {
                            int points = 0;
                            int attackerPower = duel.GetPower(attacker);
                            int targetOfAttackPower = duel.GetPower(targetOfAttack);
                            if (attackerPower == targetOfAttackPower)
                            {
                                points = 1;
                            }
                            else if (attackerPower > targetOfAttackPower)
                            {
                                points = duel.GetPower(targetOfAttack);
                            }
                            listOfPoints.Add(points);
                        }
                        int maxPoints = listOfPoints.Max();
                        if (maxPoints > 0)
                        {
                            creature = declareTargetOfAttack.Creatures[listOfPoints.IndexOf(maxPoints)];
                        }
                    }
                    else
                    {
                        if (optionalCreatureSelection.Creatures.Count > 0)
                        {
                            creature = optionalCreatureSelection.Creatures.First();
                        }
                    }
                    optionalCreatureSelection.SelectedCreature = creature;
                    newAction = optionalCreatureSelection.Perform(duel, creature);
                    duel.CurrentTurn.CurrentStep.PlayerActions.Add(optionalCreatureSelection);
                }
                else if (creatureSelection is MandatoryCreatureSelection mandatoryCreatureSelection)
                {
                    GameCreature creature = mandatoryCreatureSelection.Creatures.First();
                    mandatoryCreatureSelection.SelectedCreature = creature;
                    newAction = mandatoryCreatureSelection.Perform(duel, creature);
                    duel.CurrentTurn.CurrentStep.PlayerActions.Add(mandatoryCreatureSelection);
                }
                else
                {
                    throw new InvalidOperationException();
                }
                return newAction;
            }
            else if (playerAction is OptionalAction optionalAction)
            {
                return optionalAction.Perform(duel, true);
            }
            else if (playerAction is SelectAbilityToResolve selectAbilityToResolve)
            {
                selectAbilityToResolve.SelectedAbility = selectAbilityToResolve.Abilities.First();
                SelectAbilityToResolve.Perform(duel, selectAbilityToResolve.SelectedAbility);
                duel.CurrentTurn.CurrentStep.PlayerActions.Add(selectAbilityToResolve);
                return null;
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
                    GameCard card = null;
                    if (Hand.Cards.Sum(c => c.Cost) > ManaZone.UntappedCards.Count)
                    {
                        card = chargeMana.Cards.First();
                    }
                    chargeMana.SelectedCard = card;
                    newAction = chargeMana.Perform(duel, card);
                }
                else
                {
                    GameCard card = null;
                    if (optionalCardSelection.Cards.Count > 0)
                    {
                        card = optionalCardSelection.Cards.First();
                    }
                    optionalCardSelection.SelectedCard = card;
                    newAction = optionalCardSelection.Perform(duel, card);
                }
            }
            else if (cardSelection is MandatoryMultipleCardSelection mandatoryMultipleCardSelection)
            {
                if (cardSelection is PayCost payCost)
                {
                    GameCard civCard = payCost.Player.ManaZone.Cards.First(c => !c.Tapped && c.Civilizations.Intersect((duel.CurrentTurn.CurrentStep as Steps.MainStep).CardToBeUsed.Civilizations).Any());
                    List<GameCard> manaCards = payCost.Player.ManaZone.Cards.Where(c => !c.Tapped && c != civCard).Take(payCost.Cost - 1).ToList();
                    manaCards.Add(civCard);
                    newAction = payCost.Perform(duel, new ReadOnlyCardCollection(manaCards));
                }
                else
                {
                    mandatoryMultipleCardSelection.Perform(duel, new ReadOnlyCardCollection(mandatoryMultipleCardSelection.Cards.ToList().GetRange(0, mandatoryMultipleCardSelection.MinimumSelection)));
                }
            }
            else if (cardSelection is MultipleCardSelection multipleCardSelection)
            {
                foreach (GameCard card in multipleCardSelection.Cards)
                {
                    multipleCardSelection.SelectedCards.Add(card);
                }
                newAction = multipleCardSelection.Perform(duel, multipleCardSelection.Cards);
            }
            else if (cardSelection is MandatoryCardSelection mandatoryCardSelection)
            {
                GameCard card = mandatoryCardSelection.Cards.First();
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
