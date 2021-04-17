using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Choices
{
    public class ChargeChoice : Choice, ICardUsageChoice, IAttackerChoice, IEndTurnChoice
    {
        public IEnumerable<int> ChargeCards { get; }
        public IEnumerable<int> UseCards { get; }
        public IEnumerable<int> AttackCreatures { get; }
        public bool TurnEndable { get; }

        public ChargeChoice(int player, IEnumerable<int> chargeCards) : base(player)
        {
            //, ActivePlayer.Hand.Cards, usableCards, duel.GetCreaturesThatCanAttack(ActivePlayer)

            ChargeCards = chargeCards;
            //UseCards //TODO: Check which cards can be used
            //AttackCreatures //TODO: Check which creatures are able to attack
            TurnEndable = true; //TODO: Consider situations where it is not possible to end turn. (eg. creature must attack if able)
        }
    }
}