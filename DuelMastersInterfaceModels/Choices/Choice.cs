namespace DuelMastersInterfaceModels.Choices
{
    /// <summary>
    /// Represents a choice a player can make.
    /// </summary>
    public abstract class Choice : IChoice
    {
        /// <summary>
        /// Player who makes the choice.
        /// </summary>
        public int PlayerID { get; private set; }

        protected Choice(int playerID)
        {
            PlayerID = playerID;
        }
    }
}