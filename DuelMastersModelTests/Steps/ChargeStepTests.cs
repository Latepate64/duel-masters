﻿using DuelMastersInterfaceModels.Cards;
using DuelMastersInterfaceModels.Choices;
using DuelMastersModels;
using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using DuelMastersModels.Zones;
using Moq;
using Xunit;

namespace DuelMastersModelTests.Steps
{
    public class ChargeStepTests
    {
        [Fact]
        public void ChargeMana_ManaNotChargedBefore_ReturnNull()
        {
            ChargeStep step = new ChargeStep(Mock.Of<IPlayer>());
            IChoice choice = step.ChargeMana(Mock.Of<ICard>());
            Assert.Null(choice);
        }

        [Fact]
        public void GetNextStep_ReturnMainStep()
        {
            ChargeStep step = new ChargeStep(Mock.Of<IPlayer>());
            IStep nextStep = step.GetNextStep();
            _ = Assert.IsType<MainStep>(nextStep);
        }

        [Fact]
        public void PerformPriorityAction_ChargedCardNull_ReturnPriorityActionChoice()
        {
            Mock<IPlayer> player = new Mock<IPlayer>();
            _ = player.SetupGet(x => x.Hand).Returns(Mock.Of<Hand>());
            ChargeStep step = new(player.Object);

            IChoice choice = step.PerformPriorityAction();

            _ = Assert.IsType<ChargeChoice>(choice);
        }

        [Fact]
        public void PerformPriorityAction_ChargedCardNotNull_ReturnNull()
        {
            ChargeStep step = new(Mock.Of<IPlayer>()) { ChargedCard = Mock.Of<ICard>() };
            Assert.Null(step.PerformPriorityAction());
        }
    }
}
