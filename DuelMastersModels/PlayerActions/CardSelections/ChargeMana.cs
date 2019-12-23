using DuelMastersModels.Cards;
using System;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public class ChargeMana : OptionalCardSelection
    { 
        public ChargeMana() { }

        public ChargeMana(Player player) : base(player, new ReadOnlyCardCollection(player.Hand.Cards))
        { }

        public override PlayerAction Perform(Duel duel, GameCard card)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            if (card != null)
            {
                duel.PutFromHandIntoManaZone(Player, card);
            }
            return null;
        }
    }
}
