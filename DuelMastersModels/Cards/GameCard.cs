using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Abilities.Trigger;

namespace DuelMastersModels.Cards
{
    public abstract class GameCard : System.ComponentModel.INotifyPropertyChanged
    {
        #region Properties
        public string Name => BaseCard.Name;
        public string Set => BaseCard.Set;
        public string Id => BaseCard.Id;
        public ReadOnlyCivilizationCollection Civilizations => BaseCard.Civilizations;
        public Rarity Rarity => BaseCard.Rarity;
        public int Cost => BaseCard.Cost;
        public string Text => BaseCard.Text;

        /// <summary>
        /// Unique identifier during a game.
        /// </summary>
        public int GameId { get; private set; }

        private bool _tapped;
        public bool Tapped
        {
            get => _tapped;
            set
            {
                if (value != _tapped)
                {
                    _tapped = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool KnownToOwner { get; set; }
        public bool KnownToOpponent { get; set; }

        private bool _knownToPlayerWithPriority;
        public bool KnownToPlayerWithPriority
        {
            get => _knownToPlayerWithPriority;
            set
            {
                _knownToPlayerWithPriority = value;
                NotifyPropertyChanged();
            }
        }

        private bool _knownToPlayerWithoutPriority;
        public bool KnownToPlayerWithoutPriority
        {
            get => _knownToPlayerWithoutPriority;
            set
            {
                _knownToPlayerWithoutPriority = value;
                NotifyPropertyChanged();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public AbilityCollection Abilities => BaseCard.Abilities;
        public ReadOnlyStaticAbilityCollection StaticAbilities => BaseCard.StaticAbilities;
        public ReadOnlyTriggerAbilityCollection TriggerAbilities => BaseCard.TriggerAbilities;

        protected Card BaseCard { get; private set; }
        #endregion Properties

        protected GameCard(Card card, int gameId)
        {
            BaseCard = card;
            GameId = gameId;
        }

        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
