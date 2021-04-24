﻿using DuelMastersInterfaceModels.Choices;
using DuelMastersInterfaceModels.Events;
using DuelMastersModels.Managers;
using DuelMastersModels.Steps;
using DuelMastersModels.Zones;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels
{
    public class Turn
    {
        #region Properties
        /// <summary>
        /// The player whose turn it is.
        /// </summary>
        public Player ActivePlayer { get; }

        /// <summary>
        /// The opponent of the active player.
        /// </summary>
        public Player NonActivePlayer { get; }

        /// <summary>
        /// The step that is currently being processed.
        /// </summary>
        public Step CurrentStep => Steps.Last();

        /// <summary>
        /// All the steps in the turn that have been or are processed, in order.
        /// </summary>
        internal ICollection<Step> Steps { get; } = new Collection<Step>();

        /// <summary>
        /// The number of the turn.
        /// </summary>
        internal int Number { get; private set; }
        #endregion Properties

        public Turn(Player activePlayer, int number)
        {
            ActivePlayer = activePlayer;
            NonActivePlayer = activePlayer.Opponent;
            Number = number;
        }

        public IChoice Start(BattleZone battleZone, EventManager eventManager)
        {
            if (!Steps.Any())
            {
                Steps.Add(new StartOfTurnStep(ActivePlayer, Number == 1, battleZone));
                eventManager?.Raise(new TurnStartEvent { PlayerID = ActivePlayer.ID, Number = Number });
                return StartCurrentStep();
            }
            else
            {
                throw new System.InvalidOperationException();
            }
        }

        public IChoice ChangeAndStartStep()
        {
            Step nextStep = CurrentStep.GetNextStep();
            if (nextStep != null)
            {
                Steps.Add(nextStep);
                return StartCurrentStep();
            }
            else
            {
                return null; // Turn is over
            }
        }

        private IChoice StartCurrentStep()
        {
            IChoice choice = CurrentStep.Start();
            if (choice != null)
            {
                return choice;
            }
            else
            {
                return ChangeAndStartStep();
            }
        }
    }
}
