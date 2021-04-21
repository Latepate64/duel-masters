namespace DuelMastersClientForms
{
    internal class Player
    {
        internal string Name { get; set; }
        internal int ID { get; }

        public Player(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
