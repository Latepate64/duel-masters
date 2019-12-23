using DuelMastersModels.Abilities.Trigger;
using DuelMastersModels.Cards;
using System.Collections.Generic;
using System.Linq;

namespace DuelMastersModels.Zones
{
    public class BattleZone : Zone
    {
        public override bool Public { get; } = true;
        public override bool Ordered { get; } = false;
        
        public BattleZone(Player owner) : base(owner) { }

        public override void Add(GameCard card, Duel duel)
        {
            if (card is GameCreature creature)
            {
                creature.SummoningSickness = true;
            }
            Cards.Add(card);
            card.KnownToOwner = true;
            card.KnownToOpponent = true;
            foreach (TriggerAbility ability in card.TriggerAbilities.Where(ability => ability.TriggerCondition is WhenYouPutThisCreatureIntoTheBattleZone))
            {
                duel.TriggerTriggerAbility(ability, Owner);
            }
            foreach (GameCreature battleZoneCreature in duel.CreaturesInTheBattleZone.Except(new List<GameCard>() { card }))
            {
                foreach (TriggerAbility ability in battleZoneCreature.TriggerAbilities.Where(ability => ability.TriggerCondition is WheneverAnotherCreatureIsPutIntoTheBattleZone))
                {
                    duel.TriggerTriggerAbility(ability, ability.Controller);
                }
            }
        }

        public override void Remove(GameCard card, Duel duel)
        {
            Cards.Remove(card);
            card.Tapped = false;
            if (card is GameCreature creature)
            {
                creature.SummoningSickness = true;
            }
        }
    }
}