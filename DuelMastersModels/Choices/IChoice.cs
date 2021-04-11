﻿namespace DuelMastersModels.Choices
{
    /// <summary>
    /// Represents a choice a player can make.
    /// </summary>
    public interface IChoice
    {
        /// <summary>
        /// Player who makes the choice.
        /// </summary>
        IPlayer Player { get; }
    }
}
