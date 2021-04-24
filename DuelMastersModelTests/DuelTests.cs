using DuelMastersInterfaceModels.Choices;
using DuelMastersModels;
using DuelMastersModels.Zones;
using Moq;
using System;
using Xunit;

namespace DuelMastersModelTests
{
    public class DuelTests
    {
        [Theory]
        [InlineData(DuelState.InProgress)]
        [InlineData(DuelState.Over)]
        public void Start_StateNotSetup_ThrowInvalidOperationException(DuelState state)
        {
            _ = Assert.Throws<InvalidOperationException>(() => new Duel { State = state }.Start());
        }

        [Fact]
        public void Start_StartingPlayerNull_ThrowNullReferenceException()
        {
            _ = Assert.Throws<NullReferenceException>(() => new Duel().Start());
        }

        [Fact]
        public void Start_StartingPlayerOpponentNull_ThrowNullReferenceException()
        {
            _ = Assert.Throws<NullReferenceException>(() => new Duel { StartingPlayer = Mock.Of<Player>() }.Start());
        }

        [Fact]
        public void Start_StartingPlayerWithoutHandAndOpponentGiven_ThrowNullReferenceException()
        {
            Mock<Player> player = new();
            _ = player.SetupGet(x => x.Opponent).Returns(Mock.Of<Player>());

            _ = Assert.Throws<NullReferenceException>(() => new Duel { StartingPlayer = player.Object }.Start());
        }

        [Fact]
        public void Start_StartingPlayerWithHandAndOpponentGiven_ReturnPriorityActionChoice()
        {
            Mock<Player> player = new();
            _ = player.SetupGet(x => x.Opponent).Returns(Mock.Of<Player>());
            _ = player.SetupGet(x => x.Hand).Returns(Mock.Of<Hand>());

            IChoice choice = new Duel { StartingPlayer = player.Object }.Start();

            _ = Assert.IsType<ChargeChoice>(choice);
        }
    }
}
