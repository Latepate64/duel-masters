using DuelMastersInterfaceModels.Events;
using DuelMastersModels;
using DuelMastersModels.Managers;
using DuelMastersModels.Zones;
using Moq;
using System;
using Xunit;

namespace DuelMastersModelTests
{
    public class PlayerTests
    {
        [Fact]
        public void ShuffleDeck_DeckNull_ThrowNullReferenceException()
        {
            _ = Assert.Throws<NullReferenceException>(() => new Player().ShuffleDeck());
        }

        [Fact]
        public void ShuffleDeck_DeckNotNull_ShuffleCalledOnce()
        {
            Mock<Deck> deck = new();
            Player player = new() { Deck = deck.Object };
            player.ShuffleDeck();
            deck.Verify(x => x.Shuffle(), Times.Once);
        }
    }
}
