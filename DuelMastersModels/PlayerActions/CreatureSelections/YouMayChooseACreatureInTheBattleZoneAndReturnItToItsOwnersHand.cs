using DuelMastersModels.Cards;
using System;

namespace DuelMastersModels.PlayerActions.CreatureSelections
{
    public class YouMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand : OptionalCreatureSelection
    {
        public YouMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand(Player player, ReadOnlyCreatureCollection creatures) : base(player, creatures)
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
                return duel.ReturnFromBattleZoneToHand(creature);
            }
        }
    }
}
