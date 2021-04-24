using DuelMastersInterfaceModels.Choices;
using System.Collections.Generic;
using System.Linq;
using DuelMastersInterfaceModels.Cards;

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
        internal ICard CardToBeUsed { get; set; }

        public MainStep(IPlayer player) : base(player)
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
            //IEnumerable<ICard> usableCards = MainStep.GetUsableCards(ActivePlayer.Hand.Cards, ActivePlayer.ManaZone.UntappedCards);
            //return new PriorityActionChoice(ActivePlayer, ActivePlayer.Hand.Cards, usableCards, duel.GetCreaturesThatCanAttack(ActivePlayer));
        }

        //TODO
        //public override IChoice PlayerActionRequired(IDuel duel)
        //{
        //    //IEnumerable<ICard> usableCards = GetUsableCards(ActivePlayer.Hand.Cards, ActivePlayer.ManaZone.UntappedCards);
        //    throw new System.NotImplementedException();
        //    //return State == MainStepState.Use && usableCards.Any()
        //    //    ? new UseCard(ActivePlayer, usableCards)
        //    //    : (Choice)(State == MainStepState.Pay ? new PayCost(ActivePlayer, ActivePlayer.ManaZone.UntappedCards, CardToBeUsed.Cost) : null);
        //}

        /// <summary>
        /// Returns the cards that can be used.
        /// </summary>
        public static IEnumerable<ICard> GetUsableCards(IEnumerable<ICard> handCards, IEnumerable<ICard> manaCards)
        {
            return handCards.Where(handCard => Duel.CanBeUsed(handCard, manaCards));
        }

        public override IStep GetNextStep()
        {
            return new AttackDeclarationStep(ActivePlayer);
        }
    }
}
