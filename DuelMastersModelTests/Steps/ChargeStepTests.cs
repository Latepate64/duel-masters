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
            ChargeStep step = new(Mock.Of<Player>());
            IChoice choice = step.ChargeMana(Mock.Of<Card>());
            Assert.Null(choice);
        }

        [Fact]
        public void GetNextStep_ReturnMainStep()
        {
            ChargeStep step = new(Mock.Of<Player>());
            Step nextStep = step.GetNextStep();
            _ = Assert.IsType<MainStep>(nextStep);
        }

        [Fact]
        public void PerformPriorityAction_ChargedCardNull_ReturnPriorityActionChoice()
        {
            Mock<Player> player = new();
            _ = player.SetupGet(x => x.Hand).Returns(Mock.Of<Hand>());
            ChargeStep step = new(player.Object);

            IChoice choice = step.PerformPriorityAction();

            _ = Assert.IsType<ChargeChoice>(choice);
        }

        [Fact]
        public void PerformPriorityAction_ChargedCardNotNull_ReturnNull()
        {
            ChargeStep step = new(Mock.Of<Player>()) { ChargedCard = Mock.Of<Card>() };
            Assert.Null(step.PerformPriorityAction());
        }
    }
}
