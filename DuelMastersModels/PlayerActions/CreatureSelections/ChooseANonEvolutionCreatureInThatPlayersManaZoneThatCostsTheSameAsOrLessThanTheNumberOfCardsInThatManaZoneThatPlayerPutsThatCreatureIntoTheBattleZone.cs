using DuelMastersModels.Cards;
using System;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public class ChooseANonEvolutionCreatureInThatPlayersManaZoneThatCostsTheSameAsOrLessThanTheNumberOfCardsInThatManaZoneThatPlayerPutsThatCreatureIntoTheBattleZone : MandatoryCreatureSelection
    {
        public ChooseANonEvolutionCreatureInThatPlayersManaZoneThatCostsTheSameAsOrLessThanTheNumberOfCardsInThatManaZoneThatPlayerPutsThatCreatureIntoTheBattleZone(Player player, ReadOnlyCreatureCollection creatures) : base(player, creatures)
        { }

        public override PlayerAction Perform(Duel duel, GameCreature creature)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            else
            {
                return duel.PutFromManaZoneIntoTheBattleZone(creature);
            }
        }
    }
}
