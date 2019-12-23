using DuelMastersModels.Cards;
using System;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public class SoulSwapSelection : OptionalCreatureSelection
    {
        public SoulSwapSelection(Player player, ReadOnlyCreatureCollection creatures) : base(player, creatures)
        { }

        public override PlayerAction Perform(Duel duel, GameCreature creature)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            else if (creature == null)
            {
                return null;
            }
            else
            {
                duel.PutFromBattleZoneIntoOwnersManazone(creature);
                Player owner = duel.GetOwner(creature);
                return new ChooseANonEvolutionCreatureInThatPlayersManaZoneThatCostsTheSameAsOrLessThanTheNumberOfCardsInThatManaZoneThatPlayerPutsThatCreatureIntoTheBattleZone(Player, owner.ManaZone.NonEvolutionCreaturesThatCostTheSameAsOrLessThanTheNumberOfCardsInTheZone);
            }
        }
    }
}
