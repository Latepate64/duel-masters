using DuelMastersModels.Cards;
using DuelMastersInterfaceModels.Choices;
using System;
using System.Linq;

namespace DuelMastersModels.Steps
{
    /// <summary>
    /// 503.1. The active player may put a card from their hand into their mana zone upside down.
    /// </summary>
    public class ChargeStep : PriorityStep
    {
        public IHandCard ChargedCard { get; set; }

        public ChargeStep(IPlayer player) : base(player)
        {
        }

        public IChoice ChargeMana(IHandCard card)
        {
            if (ChargedCard != null)
            {
                throw new InvalidOperationException("Mana has already been charged during this step.");
            }
            else if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            ActivePlayer.PutFromHandIntoManaZone(card);
            ChargedCard = card;
            return Proceed();
        }

        public override IStep GetNextStep()
        {
            return new MainStep(ActivePlayer);
        }

        public override IChoice PerformPriorityAction()
        {
            State = StepState.PriorityAction;
            if (ChargedCard != null)
            {
                return null;
            }
            else
            {
                return new ChargeChoice(ActivePlayer.ID, ActivePlayer.Hand.Cards.Select(c => c.ID));
            }
        }
    }
}
