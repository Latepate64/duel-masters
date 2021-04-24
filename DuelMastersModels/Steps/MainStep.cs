using DuelMastersInterfaceModels.Choices;
using System.Collections.Generic;
using System.Linq;
using DuelMastersModels.Cards;

namespace DuelMastersModels.Steps
{
    internal enum MainStepState
    {
        Use,
        Pay,
        MustBeEnded,
    }

    /// <summary>
    /// 504.1. Normally, the active player can use cards only during their main step.
    /// </summary>
    public class MainStep : PriorityStep
    {
        internal Card CardToBeUsed { get; set; }

        public MainStep(Player player) : base(player)
        {
        }

        public override IChoice PerformPriorityAction()
        {
            State = StepState.PriorityAction;
            //TODO: Check if cards can be used
            bool cardsCanBeUsed = true;
            if (cardsCanBeUsed)
            {
                return new CardUsageChoice(ActivePlayer.ID);
            }
            else
            {
                return null;
            }
            //IEnumerable<Card> usableCards = MainStep.GetUsableCards(ActivePlayer.Hand.Cards, ActivePlayer.ManaZone.UntappedCards);
            //return new PriorityActionChoice(ActivePlayer, ActivePlayer.Hand.Cards, usableCards, duel.GetCreaturesThatCanAttack(ActivePlayer));
        }

        //TODO
        //public override IChoice PlayerActionRequired(Duel duel)
        //{
        //    //IEnumerable<Card> usableCards = GetUsableCards(ActivePlayer.Hand.Cards, ActivePlayer.ManaZone.UntappedCards);
        //    throw new System.NotImplementedException();
        //    //return State == MainStepState.Use && usableCards.Any()
        //    //    ? new UseCard(ActivePlayer, usableCards)
        //    //    : (Choice)(State == MainStepState.Pay ? new PayCost(ActivePlayer, ActivePlayer.ManaZone.UntappedCards, CardToBeUsed.Cost) : null);
        //}

        /// <summary>
        /// Returns the cards that can be used.
        /// </summary>
        public static IEnumerable<Card> GetUsableCards(IEnumerable<Card> handCards, IEnumerable<Card> manaCards)
        {
            return handCards.Where(handCard => Duel.CanBeUsed(handCard, manaCards));
        }

        public override Step GetNextStep()
        {
            return new AttackDeclarationStep(ActivePlayer);
        }
    }
}
