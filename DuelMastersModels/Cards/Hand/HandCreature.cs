﻿using DuelMastersModels.Abilities.TriggerAbilities;
using System.Collections.Generic;

namespace DuelMastersModels.Cards
{
    internal class HandCreature : HandCard, IHandCreature
    {
        public int Power { get; }
        public ICollection<Race> Races { get; }
        public bool SummoningSickness { get; set; }
        public ICollection<ITriggerAbility> TriggerAbilities { get; }

        internal HandCreature(ICard card) : base(card)
        {
        }
    }
}
