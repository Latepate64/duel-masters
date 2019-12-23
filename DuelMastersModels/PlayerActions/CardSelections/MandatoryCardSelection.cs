using DuelMastersModels.Cards;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public abstract class MandatoryCardSelection : CardSelection
    {
        protected MandatoryCardSelection(Player player, ReadOnlyCardCollection cards) : base(player, 1, 1, cards)
        { }

        public GameCard SelectedCard { get; set; }

        public override PlayerAction TryToPerformAutomatically(Duel duel)
        {
            if (Cards.Count == 1)
            {
                SelectedCard = Cards[0];
                duel.CurrentTurn.CurrentStep.PlayerActions.Add(this);
                return Perform(duel, Cards[0]);
            }
            else
            {
                return this;
            }
        }

        public bool Validate(GameCard card)
        {
            return Cards.Contains(card);
        }

        public abstract PlayerAction Perform(Duel duel, GameCard card);
    }
}