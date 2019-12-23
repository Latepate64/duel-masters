using DuelMastersModels;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.ContinuousEffects;
using DuelMastersModels.Factories;
using DuelMastersModels.PlayerActions;
using DuelMastersModels.PlayerActions.AutomaticActions;
using DuelMastersModels.PlayerActions.CardSelections;
using DuelMastersModels.PlayerActions.CreatureSelections;
using DuelMastersModels.PlayerActions.OptionalActions;
using DuelMastersModels.PlayerActionResponses;
using DuelMastersModels.Steps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DuelMastersApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, System.ComponentModel.INotifyPropertyChanged
    {
        #region Constants
        const int SideColumnWidth = 200;

        const double ZoomCardCanvasMaximumWidth = ZoomCardCanvasMaximumHeight * 222 / 307;
        const double ZoomCardCanvasMaximumHeight = 500;

        const string ConfigurationPath = "./configuration.xml";
        #endregion Constants

        #region Fields
        Duel _duel = new Duel();
        Grid _mainGrid = new Grid();
        Grid _centerGrid = new Grid();
        Grid _rightGrid = new Grid();

        Grid _actionButtonGrid = new Grid();

        Canvas _setupCanvas;
        Canvas _mainCanvas = new Canvas();
        ListView _listViewPlayer1Hand = new ListView() { Background = Brushes.Aqua, Name = "ListViewPriorityHand" };
        ListView _listViewPlayer1ManaZone = new ListView() { Background = Brushes.LawnGreen, Name = "ListViewPriorityManaZone" };
        ListView _listViewPlayer1BattleZone = new ListView() { Background = Brushes.PaleVioletRed, Name = "ListViewPriorityBattleZone" };
        ListView _listViewPlayer2BattleZone = new ListView() { Background = Brushes.PaleVioletRed, Name = "ListViewNonPriorityBattleZone" };
        ListView _listViewPlayer2ManaZone = new ListView() { Background = Brushes.LawnGreen, Name = "ListViewNonPriorityManaZone" };
        ListView _listViewPlayer2Hand = new ListView() { Background = Brushes.Aqua, Name = "ListViewNonPriorityHand" };

        TextBlock _textBlockPlayer2Name = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBlock _textBlockPlayer1Name = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        Button _actionButton = new Button() { Content = "Action button" };
        Button _declineButton = new Button() { Content = "Decline" };
        TextBlock _actionTextBlock = new TextBlock() { TextWrapping = TextWrapping.Wrap, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBox _textBoxLog = new TextBox() { IsReadOnly = true, TextWrapping = TextWrapping.Wrap, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };

        ObservableCollection<string> LogMessages { get; } = new ObservableCollection<string>();

        Button _buttonPlayer1Shields = new Button();
        Button _buttonPlayer1Hand = new Button();
        Button _buttonPlayer1Graveyard = new Button() { Content = new Image() { Source = new BitmapImage(new Uri("Images/graveyard.png", UriKind.Relative)), Stretch = Stretch.Fill } };
        Button _buttonPlayer2Shields = new Button();
        Button _buttonPlayer2Hand = new Button();
        Button _buttonPlayer2Graveyard = new Button() { Content = new Image() { Source = new BitmapImage(new Uri("Images/graveyard.png", UriKind.Relative)), Stretch = Stretch.Fill } };

        TextBlock _textBlockPlayer1Shields = new TextBlock() { FontSize = 70, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBlock _textBlockPlayer2Shields = new TextBlock() { FontSize = 70, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBlock _textBlockPlayer1Hand = new TextBlock() { FontSize = 70, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBlock _textBlockPlayer2Hand = new TextBlock() { FontSize = 70, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBlock _textBlockPlayer1Graveyard = new TextBlock() { FontSize = 70, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        TextBlock _textBlockPlayer2Graveyard = new TextBlock() { FontSize = 70, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };

        Grid _gridPlayer1DeckAndGraveyard = new Grid();
        Grid _gridPlayer1ShieldsAndHand = new Grid();
        Grid _gridPlayer2DeckAndGraveyard = new Grid();
        Grid _gridPlayer2ShieldsAndHand = new Grid();

        Image _player1DeckImage = new Image() { Source = new BitmapImage(new Uri("Images/card_back.jpg", UriKind.Relative)), Stretch = Stretch.Fill };
        Image _player2DeckImage = new Image() { Source = new BitmapImage(new Uri("Images/card_back.jpg", UriKind.Relative)), Stretch = Stretch.Fill };
        TextBlock _textBlockPlayer1Deck = new TextBlock();
        TextBlock _textBlockPlayer2Deck = new TextBlock();

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        ObservableCardCollection _selectedCards = new ObservableCardCollection();
        public ObservableCardCollection SelectedCards
        {
            get { return _selectedCards; }
            set
            {
                _selectedCards = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        Player _previousPriorityPlayer;

        CardCanvas _zoomCardCanvas = new CardCanvas() { Width = ZoomCardCanvasMaximumWidth, Height = ZoomCardCanvasMaximumHeight, Opacity = 0, Visibility = Visibility.Hidden };

        GameCard _zoomCard;
        public GameCard ZoomCard
        {
            get { return _zoomCard; }
            set
            {
                _zoomCard = value;
                NotifyPropertyChanged();
            }
        }

        ZoneCanvas _player1ShieldZoneCanvas = new ZoneCanvas();
        ZoneCanvas _player2ShieldZoneCanvas = new ZoneCanvas();
        ZoneCanvas _player1GraveyardCanvas = new ZoneCanvas();
        ZoneCanvas _player2GraveyardCanvas = new ZoneCanvas();
        ZoneCanvas _player1HandCanvas = new ZoneCanvas();
        ZoneCanvas _player2HandCanvas = new ZoneCanvas();

        PendingAbilitiesCanvas _activePlayerPendingAbilitiesCanvas;
        PendingAbilitiesCanvas _nonActivePlayerPendingAbilitiesCanvas;
        #endregion Fields

        public MainWindow()
        {
            _duel.Turns.CollectionChanged += Turns_CollectionChanged;
            LogMessages.CollectionChanged += LogMessages_CollectionChanged;

            _activePlayerPendingAbilitiesCanvas = new PendingAbilitiesCanvas(this);
            _nonActivePlayerPendingAbilitiesCanvas = new PendingAbilitiesCanvas(this);

            _setupCanvas = new SetupCanvas(this, Deserialize<Configuration>(ConfigurationPath));
            InitializeComponent();
            Title = "Duel Masters Application";
            WindowState = WindowState.Maximized;
            
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SideColumnWidth) });
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SideColumnWidth) });
            _mainGrid.RowDefinitions.Add(new RowDefinition());

            _centerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _centerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _centerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            _centerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            _centerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _centerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });

            Grid.SetColumn(_centerGrid, 1);
            _mainGrid.Children.Add(_centerGrid);

            Grid.SetColumn(_textBoxLog, 0);
            _mainGrid.Children.Add(_textBoxLog);

            _rightGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            _rightGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            #region gridPlayer2DeckAndGraveyard
            _gridPlayer2DeckAndGraveyard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer2DeckAndGraveyard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer2DeckAndGraveyard.RowDefinitions.Add(new RowDefinition());

            SetupGraveyardButton(false);
            _textBlockPlayer2Deck.SetBinding(TextBlock.TextProperty, new Binding("Player2.Deck.Cards.Count") { Source = _duel });
            _player2DeckImage.ToolTip = _textBlockPlayer2Deck;
            _buttonPlayer2Graveyard.Click += _buttonPlayer2Graveyard_Click;

            Grid.SetColumn(_player2DeckImage, 0);
            Grid.SetRow(_player2DeckImage, 0);
            _gridPlayer2DeckAndGraveyard.Children.Add(_player2DeckImage);

            Grid.SetColumn(_buttonPlayer2Graveyard, 1);
            Grid.SetRow(_buttonPlayer2Graveyard, 0);
            _gridPlayer2DeckAndGraveyard.Children.Add(_buttonPlayer2Graveyard);
            #endregion gridPlayer2DeckAndGraveyard

            #region gridPlayer2ShieldsAndHand
            _gridPlayer2ShieldsAndHand.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer2ShieldsAndHand.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer2ShieldsAndHand.RowDefinitions.Add(new RowDefinition());

            SetupShieldsButton(false);
            SetupHandButton(false);

            _buttonPlayer2Shields.Click += _buttonPlayer2Shields_Click;
            _buttonPlayer2Hand.Click += _buttonPlayer2Hand_Click;

            Grid.SetColumn(_buttonPlayer2Shields, 0);
            Grid.SetRow(_buttonPlayer2Shields, 0);
            _gridPlayer2ShieldsAndHand.Children.Add(_buttonPlayer2Shields);

            Grid.SetColumn(_buttonPlayer2Hand, 1);
            Grid.SetRow(_buttonPlayer2Hand, 0);
            _gridPlayer2ShieldsAndHand.Children.Add(_buttonPlayer2Hand);
            #endregion gridPlayer2ShieldsAndHand

            #region gridPlayer1DeckAndGraveyard
            _gridPlayer1DeckAndGraveyard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer1DeckAndGraveyard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer1DeckAndGraveyard.RowDefinitions.Add(new RowDefinition());

            SetupGraveyardButton(true);
            _textBlockPlayer1Deck.SetBinding(TextBlock.TextProperty, new Binding("Player1.Deck.Cards.Count") { Source = _duel });
            _player1DeckImage.ToolTip = _textBlockPlayer1Deck;
            _buttonPlayer1Graveyard.Click += _buttonPlayer1Graveyard_Click;

            Grid.SetColumn(_player1DeckImage, 0);
            Grid.SetRow(_player1DeckImage, 0);
            _gridPlayer1DeckAndGraveyard.Children.Add(_player1DeckImage);

            Grid.SetColumn(_buttonPlayer1Graveyard, 1);
            Grid.SetRow(_buttonPlayer1Graveyard, 0);
            _gridPlayer1DeckAndGraveyard.Children.Add(_buttonPlayer1Graveyard);
            #endregion gridPlayer1DeckAndGraveyard

            #region gridPlayer1ShieldsAndHand
            _gridPlayer1ShieldsAndHand.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer1ShieldsAndHand.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _gridPlayer1ShieldsAndHand.RowDefinitions.Add(new RowDefinition());

            SetupShieldsButton(true);
            SetupHandButton(true);

            _buttonPlayer1Shields.Click += _buttonPlayer1Shields_Click;
            _buttonPlayer1Hand.Click += _buttonPlayer1Hand_Click;

            Grid.SetColumn(_buttonPlayer1Shields, 0);
            Grid.SetRow(_buttonPlayer1Shields, 0);
            _gridPlayer1ShieldsAndHand.Children.Add(_buttonPlayer1Shields);

            Grid.SetColumn(_buttonPlayer1Hand, 1);
            Grid.SetRow(_buttonPlayer1Hand, 0);
            _gridPlayer1ShieldsAndHand.Children.Add(_buttonPlayer1Hand);
            #endregion gridPlayer1ShieldsAndHand

            Grid.SetColumn(_rightGrid, 2);
            _mainGrid.Children.Add(_rightGrid);

            _mainCanvas.Children.Add(_mainGrid);
            _mainCanvas.Children.Add(_setupCanvas);

            _mainCanvas.Children.Add(_player1ShieldZoneCanvas);
            _mainCanvas.Children.Add(_player2ShieldZoneCanvas);
            _mainCanvas.Children.Add(_player1GraveyardCanvas);
            _mainCanvas.Children.Add(_player2GraveyardCanvas);
            _mainCanvas.Children.Add(_player1HandCanvas);
            _mainCanvas.Children.Add(_player2HandCanvas);

            _mainCanvas.Children.Add(_activePlayerPendingAbilitiesCanvas);
            _mainCanvas.Children.Add(_nonActivePlayerPendingAbilitiesCanvas);

            _mainCanvas.Children.Add(_zoomCardCanvas);

            Content = _mainCanvas;

            _actionButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _actionButtonGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _actionButtonGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Star) });
            Grid.SetRow(_actionButton, 0);
            _actionButtonGrid.Children.Add(_actionButton);
            Grid.SetRow(_declineButton, 1);
            _actionButtonGrid.Children.Add(_declineButton);

            _mainCanvas.SizeChanged += MainCanvas_SizeChanged;
            _actionButton.Click += _actionButton_Click;
            _declineButton.Click += _declineButton_Click;
        }

        #region Public methods
        public void InitializeDuel(Configuration configuration)
        {
            _duel.Player1 = configuration.Player1Configuration.ControlledByComputer ? new AIPlayer() : new Player();
            _duel.Player1.Id = 0;
            _duel.Player1.Name = configuration.Player1Configuration.Name;
            _duel.Player2 = configuration.Player2Configuration.ControlledByComputer ? new AIPlayer() : new Player();
            _duel.Player2.Id = 1;
            _duel.Player2.Name = configuration.Player2Configuration.Name;
            int gameId = 0;

            Collection<JsonCard> player1JsonCards, player2JsonCards;
            if (configuration.FetchJsonOnline)
            {
                Uri uri = new Uri(configuration.JsonUrlOrPath);
                player1JsonCards = JsonCardFactory.GetJsonCardsFromUrl(uri, Deserialize<XmlDeck>(configuration.Player1Configuration.DeckPath));
                player2JsonCards = JsonCardFactory.GetJsonCardsFromUrl(uri, Deserialize<XmlDeck>(configuration.Player2Configuration.DeckPath));
            }
            else
            {
                player1JsonCards = JsonCardFactory.GetJsonCards(configuration.JsonUrlOrPath, Deserialize<XmlDeck>(configuration.Player1Configuration.DeckPath));
                player2JsonCards = JsonCardFactory.GetJsonCards(configuration.JsonUrlOrPath, Deserialize<XmlDeck>(configuration.Player2Configuration.DeckPath));
            }

            _duel.Player1.SetDeckBeforeDuel(CardFactory.GetCardsFromJsonCards(player1JsonCards, ref gameId, _duel.Player1));
            _duel.Player2.SetDeckBeforeDuel(CardFactory.GetCardsFromJsonCards(player2JsonCards, ref gameId, _duel.Player2));

            if (Duel.NotParsedAbilities.Count > 0)
            {
                string message = "The following abilities could not be parsed (the game can still be played without those abilities):";
                List<string> abilities = Duel.NotParsedAbilities.Distinct().ToList();
                MessageBox.Show(string.Join("\r\n\r\n", message, string.Join("\r\n", abilities)));
            }

            _duel.Player1.SetupDeck(_duel);
            _duel.Player2.SetupDeck(_duel);

            //TODO: test
            _duel.ContinuousEffects.CollectionChanged += ObservableContinuousEffects_CollectionChanged;

            PlayerAction playerAction = _duel.StartDuel((StartingPlayer)configuration.WhoGoesFirst);

            Serialize(configuration, ConfigurationPath);

            _textBlockPlayer1Name.SetBinding(TextBlock.TextProperty, new Binding("Player1.Name") { Source = _duel });
            _textBlockPlayer2Name.SetBinding(TextBlock.TextProperty, new Binding("Player2.Name") { Source = _duel });

            _listViewPlayer1BattleZone.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Player1.BattleZone.Cards") { Source = _duel });
            _listViewPlayer1Hand.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Player1.Hand.Cards") { Source = _duel });
            _listViewPlayer1ManaZone.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Player1.ManaZone.Cards") { Source = _duel });
            _listViewPlayer2BattleZone.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Player2.BattleZone.Cards") { Source = _duel });
            _listViewPlayer2Hand.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Player2.Hand.Cards") { Source = _duel });
            _listViewPlayer2ManaZone.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Player2.ManaZone.Cards") { Source = _duel });

            _player1ShieldZoneCanvas.Initialize(string.Format("{0}'s shield zone", _duel.Player1.Name), new Binding("Player1.ShieldZone.Cards") { Source = _duel }, this);
            _player2ShieldZoneCanvas.Initialize(string.Format("{0}'s shield zone", _duel.Player2.Name), new Binding("Player2.ShieldZone.Cards") { Source = _duel }, this);
            _player1GraveyardCanvas.Initialize(string.Format("{0}'s graveyard", _duel.Player1.Name), new Binding("Player1.Graveyard.Cards") { Source = _duel }, this);
            _player2GraveyardCanvas.Initialize(string.Format("{0}'s graveyard", _duel.Player2.Name), new Binding("Player2.Graveyard.Cards") { Source = _duel }, this);
            _player1HandCanvas.Initialize(string.Format("{0}'s hand", _duel.Player1.Name), new Binding("Player1.Hand.Cards") { Source = _duel }, this, showKnownToPlayerWithoutPriority: false);
            _player2HandCanvas.Initialize(string.Format("{0}'s hand", _duel.Player2.Name), new Binding("Player2.Hand.Cards") { Source = _duel }, this, showKnownToPlayerWithoutPriority: false);

            BindCardCanvasToListView(_listViewPlayer1Hand);
            BindCardCanvasToListView(_listViewPlayer2Hand);
            BindBattleZoneCreatureCanvasToListView(_listViewPlayer1BattleZone);
            BindBattleZoneCreatureCanvasToListView(_listViewPlayer2BattleZone);
            BindManaZoneCardCanvasToListView(_listViewPlayer1ManaZone);
            BindManaZoneCardCanvasToListView(_listViewPlayer2ManaZone);

            UpdateZoomCard(0);
            BindZoomCardCanvas();

            UpdateViewToShowPlayerAction(playerAction);
        }

        public void ZoomCardCanvas(int gameId)
        {
            _zoomCardCanvas.Visibility = Visibility.Visible;

            Point mousePosition = PointToScreen(Mouse.GetPosition(this));
            const int ZoomCardCanvasOffset = 10;

            double leftPosition = mousePosition.X + ZoomCardCanvasOffset + ZoomCardCanvasMaximumWidth / 2;
            double rightPosition = leftPosition + ZoomCardCanvasMaximumWidth;
            if (Width < rightPosition)
            {
                leftPosition = mousePosition.X - ZoomCardCanvasMaximumWidth;
            }
            Canvas.SetLeft(_zoomCardCanvas, leftPosition);

            double topPosition = Math.Max(0, mousePosition.Y - ZoomCardCanvasMaximumHeight / 2);
            double bottomPosition = topPosition + ZoomCardCanvasMaximumHeight;
            if (ActualHeight < bottomPosition)
            {
                topPosition = mousePosition.Y - ZoomCardCanvasMaximumHeight;
            }
            Canvas.SetTop(_zoomCardCanvas, topPosition);
            UpdateZoomCard(gameId);
            _zoomCardCanvas.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 300))) { BeginTime = new TimeSpan(0, 0, 0, 0, 800) });

            _zoomCardCanvas.AdjustCardNameSize(ZoomCardCanvasMaximumWidth, ZoomCardCanvasMaximumHeight);
            if (_zoomCardCanvas is CardCanvas cardCanvas)
            {
                cardCanvas.AdjustRaceSize(ZoomCardCanvasMaximumWidth, ZoomCardCanvasMaximumHeight);
            }
        }

        public void UnzoomCardCanvas()
        {
            _zoomCardCanvas.Visibility = Visibility.Hidden;
            _zoomCardCanvas.BeginAnimation(OpacityProperty, null);
            _zoomCardCanvas.Opacity = 0;
        }

        public void BindCardCanvasToListView(ListView listView, bool showKnownToPlayerWithoutPriority = true)
        {
            FrameworkElementFactory cardCanvasFactory = new FrameworkElementFactory(typeof(CardCanvas));
            BindAbstractCardCanvas(listView, cardCanvasFactory, 222.0 / 307.0);

            cardCanvasFactory.SetBinding(CardCanvas.CivilizationProperty, new Binding("Civilizations"));
            cardCanvasFactory.SetBinding(CardCanvas.CardTextProperty, new Binding("Text"));
            cardCanvasFactory.SetBinding(CardCanvas.CostProperty, new Binding("Cost"));
            cardCanvasFactory.SetBinding(CardCanvas.RaceProperty, new Binding("Races"));
            cardCanvasFactory.SetBinding(CardCanvas.CardTypeProperty, new Binding() { Converter = new ObjectToCardTypeConverter() });
            cardCanvasFactory.SetBinding(CardCanvas.KnownToPlayerWithPriorityProperty, new Binding("KnownToPlayerWithPriority"));
            if (showKnownToPlayerWithoutPriority)
            {
                cardCanvasFactory.SetBinding(CardCanvas.KnownToPlayerWithoutPriorityProperty, new Binding("KnownToPlayerWithoutPriority"));
            }
        }
        #endregion Public methods

        #region Private methods
        private void UpdateViewToShowPlayerAction(PlayerAction playerAction)
        {
            if (playerAction == null)
            {
                if (_duel.Ended)
                {
                    string resultText = string.Format("{0} won the duel!", _duel.Winner.Name);
                    _actionTextBlock.Text = resultText;
                    LogMessages.Add(resultText);
                }
                else
                {
                    throw new Exception("Expected player action.");
                }
            }
            else
            {
                ObservableCardCollection cardsOwner = _duel.Player1.DeckBeforeDuel;
                ObservableCardCollection cardsOpponent = _duel.Player2.DeckBeforeDuel;
                if (playerAction.Player == _duel.Player2)
                {
                    cardsOwner = _duel.Player2.DeckBeforeDuel;
                    cardsOpponent = _duel.Player1.DeckBeforeDuel;
                }
                foreach (GameCard card in cardsOwner)
                {
                    card.KnownToPlayerWithPriority = card.KnownToOwner;
                    card.KnownToPlayerWithoutPriority = card.KnownToOpponent;
                }
                foreach (GameCard card in cardsOpponent)
                {
                    card.KnownToPlayerWithPriority = card.KnownToOpponent;
                    card.KnownToPlayerWithoutPriority = card.KnownToOwner;
                }

                if (playerAction.Player != _previousPriorityPlayer)
                {
                    _previousPriorityPlayer = playerAction.Player;
                    _centerGrid.Children.Clear();
                    _rightGrid.Children.Clear();

                    if (playerAction.Player == _duel.Player1)
                    {
                        Grid.SetColumn(_listViewPlayer1Hand, 0);
                        Grid.SetRow(_listViewPlayer1Hand, 4);
                        _centerGrid.Children.Add(_listViewPlayer1Hand);

                        Grid.SetColumn(_listViewPlayer1ManaZone, 0);
                        Grid.SetRow(_listViewPlayer1ManaZone, 3);
                        _centerGrid.Children.Add(_listViewPlayer1ManaZone);

                        Grid.SetColumn(_listViewPlayer1BattleZone, 0);
                        Grid.SetRow(_listViewPlayer1BattleZone, 2);
                        _centerGrid.Children.Add(_listViewPlayer1BattleZone);

                        Grid.SetColumn(_listViewPlayer2BattleZone, 0);
                        Grid.SetRow(_listViewPlayer2BattleZone, 1);
                        _centerGrid.Children.Add(_listViewPlayer2BattleZone);

                        Grid.SetColumn(_listViewPlayer2ManaZone, 0);
                        Grid.SetRow(_listViewPlayer2ManaZone, 0);
                        _centerGrid.Children.Add(_listViewPlayer2ManaZone);


                        Grid.SetColumn(_gridPlayer2ShieldsAndHand, 0);
                        Grid.SetRow(_gridPlayer2ShieldsAndHand, 1);
                        _rightGrid.Children.Add(_gridPlayer2ShieldsAndHand);

                        Grid.SetColumn(_gridPlayer2DeckAndGraveyard, 0);
                        Grid.SetRow(_gridPlayer2DeckAndGraveyard, 2);
                        _rightGrid.Children.Add(_gridPlayer2DeckAndGraveyard);

                        Grid.SetColumn(_gridPlayer1DeckAndGraveyard, 0);
                        Grid.SetRow(_gridPlayer1DeckAndGraveyard, 5);
                        _rightGrid.Children.Add(_gridPlayer1DeckAndGraveyard);

                        Grid.SetColumn(_gridPlayer1ShieldsAndHand, 0);
                        Grid.SetRow(_gridPlayer1ShieldsAndHand, 6);
                        _rightGrid.Children.Add(_gridPlayer1ShieldsAndHand);

                        Grid.SetColumn(_textBlockPlayer2Name, 0);
                        Grid.SetRow(_textBlockPlayer2Name, 0);
                        _rightGrid.Children.Add(_textBlockPlayer2Name);

                        Grid.SetColumn(_textBlockPlayer1Name, 0);
                        Grid.SetRow(_textBlockPlayer1Name, 7);
                        _rightGrid.Children.Add(_textBlockPlayer1Name);
                    }
                    else
                    {
                        Grid.SetColumn(_listViewPlayer1ManaZone, 0);
                        Grid.SetRow(_listViewPlayer1ManaZone, 0);
                        _centerGrid.Children.Add(_listViewPlayer1ManaZone);

                        Grid.SetColumn(_listViewPlayer1BattleZone, 0);
                        Grid.SetRow(_listViewPlayer1BattleZone, 1);
                        _centerGrid.Children.Add(_listViewPlayer1BattleZone);

                        Grid.SetColumn(_listViewPlayer2BattleZone, 0);
                        Grid.SetRow(_listViewPlayer2BattleZone, 2);
                        _centerGrid.Children.Add(_listViewPlayer2BattleZone);

                        Grid.SetColumn(_listViewPlayer2ManaZone, 0);
                        Grid.SetRow(_listViewPlayer2ManaZone, 3);
                        _centerGrid.Children.Add(_listViewPlayer2ManaZone);

                        Grid.SetColumn(_listViewPlayer2Hand, 0);
                        Grid.SetRow(_listViewPlayer2Hand, 4);
                        _centerGrid.Children.Add(_listViewPlayer2Hand);

                        Grid.SetColumn(_gridPlayer2ShieldsAndHand, 0);
                        Grid.SetRow(_gridPlayer2ShieldsAndHand, 6);
                        _rightGrid.Children.Add(_gridPlayer2ShieldsAndHand);

                        Grid.SetColumn(_gridPlayer2DeckAndGraveyard, 0);
                        Grid.SetRow(_gridPlayer2DeckAndGraveyard, 5);
                        _rightGrid.Children.Add(_gridPlayer2DeckAndGraveyard);

                        Grid.SetColumn(_gridPlayer1DeckAndGraveyard, 0);
                        Grid.SetRow(_gridPlayer1DeckAndGraveyard, 2);
                        _rightGrid.Children.Add(_gridPlayer1DeckAndGraveyard);

                        Grid.SetColumn(_gridPlayer1ShieldsAndHand, 0);
                        Grid.SetRow(_gridPlayer1ShieldsAndHand, 1);
                        _rightGrid.Children.Add(_gridPlayer1ShieldsAndHand);

                        Grid.SetColumn(_textBlockPlayer1Name, 0);
                        Grid.SetRow(_textBlockPlayer1Name, 0);
                        _rightGrid.Children.Add(_textBlockPlayer1Name);

                        Grid.SetColumn(_textBlockPlayer2Name, 0);
                        Grid.SetRow(_textBlockPlayer2Name, 7);
                        _rightGrid.Children.Add(_textBlockPlayer2Name);
                    }

                    Grid.SetColumn(_actionTextBlock, 0);
                    Grid.SetRow(_actionTextBlock, 3);
                    _rightGrid.Children.Add(_actionTextBlock);

                    Grid.SetColumn(_actionButtonGrid, 0);
                    Grid.SetRow(_actionButtonGrid, 4);
                    _rightGrid.Children.Add(_actionButtonGrid);

                    _centerGrid.BeginAnimation(OpacityProperty, new DoubleAnimation(0.5, 1.0, new Duration(new TimeSpan(0, 0, 2))));
                }

                _textBlockPlayer1Shields.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer2Shields.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer1Hand.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer2Hand.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer1Graveyard.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer2Graveyard.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer1Deck.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                _textBlockPlayer2Deck.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();

                _actionButton.Content = "";
                _actionButton.IsEnabled = false;

                _actionButtonGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);

                HideZoneCanvases();
                _activePlayerPendingAbilitiesCanvas.Visibility = Visibility.Hidden;
                _nonActivePlayerPendingAbilitiesCanvas.Visibility = Visibility.Hidden;

                if (playerAction is CardSelection cardSelection)
                {
                    if (cardSelection is OptionalCardSelection optionalCardSelection)
                    {
                        UpdateOptionalCardSelection(optionalCardSelection);
                    }
                    else if (cardSelection is MandatoryMultipleCardSelection mandatoryMultipleCardSelection)
                    {
                        UpdateMandatoryMultipleCardSelection(mandatoryMultipleCardSelection);
                    }
                    else if (cardSelection is MultipleCardSelection multipleCardSelection)
                    {
                        UpdateMultipleCardSelection(multipleCardSelection);
                    }
                    else if (cardSelection is MandatoryCardSelection mandatoryCardSelection)
                    {
                        UpdateMandatoryCardSelection(mandatoryCardSelection);
                    }
                    else
                    {
                        throw new ArgumentException("Unknown card selection.");
                    }
                }
                else if (playerAction is CreatureSelection creatureSelection)
                {
                    if (creatureSelection is OptionalCreatureSelection optionalCreatureSelection)
                    {
                        UpdateOptionalCreatureSelection(optionalCreatureSelection);
                    }
                    else if (creatureSelection is MandatoryCreatureSelection mandatoryCreatureSelection)
                    {
                        if (mandatoryCreatureSelection is ChooseANonEvolutionCreatureInThatPlayersManaZoneThatCostsTheSameAsOrLessThanTheNumberOfCardsInThatManaZoneThatPlayerPutsThatCreatureIntoTheBattleZone)
                        {
                            _actionTextBlock.Text = "Choose a non-evolution creature in that player's mana zone that costs the same as or less than the number of cards in that mana zone. That player puts that creature into the battle zone.";
                        }
                        else
                        {
                            throw new ArgumentException("Unknown mandatory creature selection.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Unknown creature selection.");
                    }
                }
                else if (playerAction is OptionalAction optionalAction)
                {
                    _actionButton.IsEnabled = true;
                    _actionButton.Content = "Take action";
                    _actionButtonGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                    if (optionalAction is YouMayDrawACard youMayDrawACard)
                    {
                        _actionTextBlock.Text = "You may draw a card.";
                    }
                    else
                    {
                        throw new ArgumentException("Unknown optional action.");
                    }
                }
                else if (playerAction is SelectAbilityToResolve selectAbilityToResolve)
                {
                    UpdateSelectAbilityToResolve(selectAbilityToResolve);
                }
                else
                {
                    throw new ArgumentException("Unknown player action.");
                }
            }
        }

        private void UpdateOptionalCreatureSelection(OptionalCreatureSelection optionalCreatureSelection)
        {
            _actionButton.IsEnabled = true;
            if (optionalCreatureSelection is DeclareAttacker)
            {
                _actionTextBlock.Text = "You may declare a creature to attack with.";
                _actionButton.Content = "Do not attack";
            }
            else if (optionalCreatureSelection is DeclareTargetOfAttack)
            {
                _actionTextBlock.Text = "Declare the target of the attack.";
                if (_duel.CanAttackOpponent((_duel.CurrentTurn.CurrentStep as AttackDeclarationStep).AttackingCreature))
                {
                    _actionButton.Content = "Attack opponent";
                }
                else
                {
                    _actionButton.IsEnabled = false;
                }
            }
            else if (optionalCreatureSelection is DeclareBlock)
            {
                _actionTextBlock.Text = "You may declare a creature to block the attack with.";
                _actionButton.Content = "Do not block";
            }
            else if (optionalCreatureSelection is YouMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand)
            {
                _actionTextBlock.Text = "You may choose a creature in the battle zone and return it to its owner's hand.";
                _actionButton.Content = "Decline";
            }
            else if (optionalCreatureSelection is SoulSwapSelection)
            {
                _actionTextBlock.Text = "You may choose a creature in the battle zone and put it into its owner's mana zone. If you do, choose a non-evolution creature in that player's mana zone that costs the same as or less than the number of cards in that mana zone. That player puts that creature into the battle zone.";
                _actionButton.Content = "Decline";
            }
            else
            {
                throw new ArgumentException("Unknown optional creature selection.");
            }
        }

        private void UpdateSelectAbilityToResolve(SelectAbilityToResolve selectAbilityToResolve)
        {
            _actionTextBlock.Text = "Select which ability resolves first.";
            if (selectAbilityToResolve.Player == _duel.CurrentTurn.ActivePlayer)
            {
                _activePlayerPendingAbilitiesCanvas.Visibility = Visibility.Visible;
                _activePlayerPendingAbilitiesCanvas.SetItemsSource(_duel.PendingAbilitiesForActivePlayer);
            }
            else if (selectAbilityToResolve.Player == _duel.CurrentTurn.NonActivePlayer)
            {
                _nonActivePlayerPendingAbilitiesCanvas.Visibility = Visibility.Visible;
                _nonActivePlayerPendingAbilitiesCanvas.SetItemsSource(_duel.PendingAbilitiesForNonActivePlayer);
            }
            else
            {
                throw new InvalidOperationException();
            }
            //_activePlayerPendingAbilitiesCanvas// = new PendingAbilitiesCanvas(this, selectAbilityToResolve.Abilities);
        }

        private void UpdateOptionalCardSelection(OptionalCardSelection optionalCardSelection)
        {
            _actionButton.IsEnabled = true;
            if (optionalCardSelection is ChargeMana chargeMana)
            {
                _actionTextBlock.Text = "You may charge mana.";
                _actionButton.Content = "Skip";
            }
            else if (optionalCardSelection is UseCard useCard)
            {
                _actionTextBlock.Text = "You may use a card.";
                _actionButton.Content = "Skip";
            }
            else if (optionalCardSelection is YouMayAddACardFromYourHandToYourShieldsFaceDownIfYouDoChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShield)
            {
                _actionTextBlock.Text = "You may add a card from your hand to your shields face down. If you do, choose one of your shields and put it into your hand. You can't use the \"shield trigger\" ability of that shield.";
                _actionButton.Content = "Decline";
            }
            else
            {
                throw new ArgumentException("Unknown card selection.");
            }
        }

        private void UpdateMandatoryMultipleCardSelection(MandatoryMultipleCardSelection mandatoryMultipleCardSelection)
        {
            if (mandatoryMultipleCardSelection is PayCost)
            {
                _actionTextBlock.Text = string.Format("Pay the mana cost for {0}.", (_duel.CurrentTurn.CurrentStep as MainStep).CardToBeUsed.Name);
            }
            else if (mandatoryMultipleCardSelection is BreakShields breakShields)
            {
                _actionTextBlock.Text = (breakShields.MinimumSelection == 1) ? "Choose a shield to break" : string.Format("Choose {0} shields to break.", breakShields.MinimumSelection);
                ToggleVisibility(GetShieldZoneCanvas(_duel.GetOpponent(breakShields.Player)));
            }
            else
            {
                throw new ArgumentException("Unknown mandatory multiple card selection.");
            }
        }

        private void UpdateMultipleCardSelection(MultipleCardSelection multipleCardSelection)
        {
            _actionButton.IsEnabled = true;
            if (multipleCardSelection is DeclareShieldTriggers)
            {
                _actionTextBlock.Text = "Declare shield triggers to be used.";
                _actionButton.Content = "Confirm";
            }
            else
            {
                throw new ArgumentException("Unknown multiple card selection.");
            }
        }

        private void UpdateMandatoryCardSelection(MandatoryCardSelection mandatoryCardSelection)
        {
            if (mandatoryCardSelection is UseShieldTrigger)
            {
                _actionTextBlock.Text = "Declare shield trigger to be used.";
            }
            else if (mandatoryCardSelection is ChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShield)
            {
                _actionTextBlock.Text = "Choose one of your shields and put it into your hand. You can't use the \"shield trigger\" ability of that shield.";
                ToggleVisibility(GetShieldZoneCanvas(mandatoryCardSelection.Player));
            }
            else
            {
                throw new ArgumentException("Unknown mandatory card selection.");
            }
        }

        private UIElement GetShieldZoneCanvas(Player player)
        {
            if (player == _duel.Player1)
            {
                return _player1ShieldZoneCanvas;
            }
            else if (player == _duel.Player2)
            {
                return _player2ShieldZoneCanvas;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static T Deserialize<T>(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }

        private static void Serialize<T>(T objectToSerialize, string configurationPath)
        {
            using (FileStream file = File.Create(configurationPath))
            {
                new XmlSerializer(typeof(T)).Serialize(file, objectToSerialize);
            }
        }

        private static FrameworkElementFactory StackPanelFactory
        {
            get
            {
                FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                return stackPanelFactory;
            }
        }

        private void BindBattleZoneCreatureCanvasToListView(ListView listView)
        {
            FrameworkElementFactory canvasFactory = new FrameworkElementFactory(typeof(BattleZoneCreatureCanvas));
            BindAbstractCardCanvas(listView, canvasFactory, 1);

            canvasFactory.SetBinding(BattleZoneCreatureCanvas.CivilizationProperty, new Binding("Civilizations"));
            canvasFactory.SetBinding(BattleZoneCreatureCanvas.SummoningSicknessProperty, new Binding("SummoningSickness"));
            canvasFactory.SetBinding(BattleZoneCreatureCanvas.AbilitiesProperty, new Binding("Abilities"));
        }

        private void BindManaZoneCardCanvasToListView(ListView listView)
        {
            FrameworkElementFactory canvasFactory = new FrameworkElementFactory(typeof(ManaZoneCardCanvas));
            BindAbstractCardCanvas(listView, canvasFactory, 1, 0.92);

            canvasFactory.SetBinding(ManaZoneCardCanvas.CivilizationProperty, new Binding("Civilizations"));
        }

        private void BindAbstractCardCanvas(ListView listView, FrameworkElementFactory cardCanvasFactory, double multiplier, double listViewScale = 0.96)
        {
            listView.ItemsPanel = new ItemsPanelTemplate() { VisualTree = StackPanelFactory};

            cardCanvasFactory.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = listView, Converter = new ListViewSizeToCardCanvasSizeConverter(), ConverterParameter = listViewScale });
            cardCanvasFactory.SetBinding(WidthProperty, new Binding("ActualHeight") { Source = listView, Converter = new ListViewSizeToCardCanvasSizeConverter(), ConverterParameter = listViewScale * multiplier });

            cardCanvasFactory.SetBinding(RectangleCardCanvas.CardNameProperty, new Binding("Name"));

            cardCanvasFactory.SetBinding(RectangleCardCanvas.PowerProperty, new Binding("Power"));

            cardCanvasFactory.SetBinding(AbstractCardCanvas.GameIdProperty, new Binding("GameId"));
            cardCanvasFactory.SetBinding(AbstractCardCanvas.TappedProperty, new Binding("Tapped"));

            MultiBinding multiBinding = new MultiBinding() { Converter = new SetAndIdConverter() };
            multiBinding.Bindings.Add(new Binding("Set"));
            multiBinding.Bindings.Add(new Binding("Id"));
            cardCanvasFactory.SetBinding(AbstractCardCanvas.SetAndIdProperty, multiBinding);
            cardCanvasFactory.SetBinding(AbstractCardCanvas.CandidateGameIdsProperty, new Binding("CurrentPlayerAction") { Converter = new PlayerActionToIntCollectionConverter(), Source = _duel });
            cardCanvasFactory.SetBinding(AbstractCardCanvas.SelectedGameIdsProperty, new Binding("SelectedCards") { Converter = new CardCollectionToIntCollectionConverter(), Source = this });
            cardCanvasFactory.SetBinding(AbstractCardCanvas.MainWindowProperty, new Binding() { Source = this });

            cardCanvasFactory.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(CardCanvas_MouseLeftButtonDown));
            listView.ItemTemplate = new DataTemplate() { VisualTree = cardCanvasFactory };
        }

        private void BindZoomCardCanvas()
        {
            _zoomCardCanvas.SetBinding(RectangleCardCanvas.CardNameProperty, new Binding("ZoomCard.Name") { Source = this });            
            _zoomCardCanvas.SetBinding(RectangleCardCanvas.PowerProperty, new Binding("ZoomCard.Power") { Source = this });
            _zoomCardCanvas.SetBinding(AbstractCardCanvas.GameIdProperty, new Binding("ZoomCard.GameId") { Source = this });
            MultiBinding multiBinding = new MultiBinding() { Converter = new SetAndIdConverter() };
            multiBinding.Bindings.Add(new Binding("ZoomCard.Set") { Source = this });
            multiBinding.Bindings.Add(new Binding("ZoomCard.Id") { Source = this });
            _zoomCardCanvas.SetBinding(AbstractCardCanvas.SetAndIdProperty, multiBinding);

            _zoomCardCanvas.SetBinding(CardCanvas.CivilizationProperty, new Binding("ZoomCard.Civilizations") { Source = this });
            _zoomCardCanvas.SetBinding(CardCanvas.CardTextProperty, new Binding("ZoomCard.Text") { Source = this });
            _zoomCardCanvas.SetBinding(CardCanvas.CostProperty, new Binding("ZoomCard.Cost") { Source = this });
            _zoomCardCanvas.SetBinding(CardCanvas.RaceProperty, new Binding("ZoomCard.Races") { Source = this });
            _zoomCardCanvas.SetBinding(CardCanvas.CardTypeProperty, new Binding("ZoomCard") { Converter = new ObjectToCardTypeConverter(), Source = this });
            _zoomCardCanvas.SetBinding(CardCanvas.KnownToPlayerWithPriorityProperty, new Binding("ZoomCard.KnownToPlayerWithPriority") { Source = this });
        }

        private void LogPlayerAction(PlayerAction playerAction)
        {
            if (playerAction is AutomaticAction automaticAction)
            {
                if (automaticAction is DrawCard drawCard)
                {
                    LogMessages.Add(string.Format("{0} drew a card.", drawCard.Player.Name));
                }
                else if (automaticAction is PutTheTopCardOfYourDeckIntoYourManaZone)
                {
                    LogMessages.Add(string.Format("{0} put the top card of their deck into their mana zone.", automaticAction.Player.Name));
                }
                else if (automaticAction is AddTheTopCardOfYourDeckToYourShieldsFaceDown)
                {
                    LogMessages.Add(string.Format("{0} added the top card of their deck to their shields face down.", automaticAction.Player.Name));
                }
                else if (playerAction is TapAllYourOpponentsCreaturesInTheBattleZone)
                {
                    LogMessages.Add(string.Format("{0} tapped all their opponent's creatures in the battle zone.", automaticAction.Player.Name));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (playerAction is CardSelection cardSelection)
            {
                if (cardSelection is OptionalCardSelection optionalCardSelection)
                {
                    if (optionalCardSelection is ChargeMana chargeMana)
                    {
                        if (chargeMana.SelectedCard != null)
                        {
                            LogMessages.Add(string.Format("{0} charged {1} as mana.", chargeMana.Player.Name, chargeMana.SelectedCard.Name));
                        }
                    }
                    else if (optionalCardSelection is UseCard useCard)
                    {
                        if (useCard.SelectedCard != null)
                        {
                            if (useCard.SelectedCard is GameCreature creature)
                            {
                                LogMessages.Add(string.Format("{0} summoned {1}.", useCard.Player.Name, creature.Name));
                            }
                            else if (useCard.SelectedCard is GameSpell spell)
                            {
                                LogMessages.Add(string.Format("{0} cast {1}.", useCard.Player.Name, spell.Name));
                            }
                        }
                    }
                    else if (optionalCardSelection is YouMayAddACardFromYourHandToYourShieldsFaceDownIfYouDoChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShield)
                    {
                        LogMessages.Add(string.Format("{0} added a card from their hand to their shields face down.", optionalCardSelection.Player.Name));
                    }
                    else
                    {
                        throw new InvalidOperationException("optionalCardSelection");
                    }
                }
                else if (cardSelection is MandatoryCardSelection mandatoryCardSelection)
                {
                    if (mandatoryCardSelection is UseShieldTrigger useShieldTrigger)
                    {
                        LogMessages.Add(string.Format("{0} used the shield trigger ability of {1}.", useShieldTrigger.Player.Name, useShieldTrigger.SelectedCard.Name));
                    }
                    else if (mandatoryCardSelection is ChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShield)
                    {
                        LogMessages.Add(string.Format("{0} chose one of their shields and put it into their hand.", mandatoryCardSelection.Player.Name));
                    }
                    else
                    {
                        throw new InvalidOperationException("mandatoryCardSelection");
                    }
                }
                else if (cardSelection is MandatoryMultipleCardSelection mandatoryMultipleCardSelection)
                {
                    if (mandatoryMultipleCardSelection is BreakShields breakShields)
                    {
                        if (breakShields.MinimumSelection > 1)
                        {
                            LogMessages.Add(string.Format("{0} broke {1} of {2}'s shields.", breakShields.ShieldBreakingCreature.Name, breakShields.MinimumSelection, _duel.GetOpponent(breakShields.Player).Name));
                        }
                        else
                        {
                            LogMessages.Add(string.Format("{0} broke one of {1}'s shields.", breakShields.ShieldBreakingCreature.Name, _duel.GetOpponent(breakShields.Player).Name));
                        }

                    }
                    else if (!(mandatoryMultipleCardSelection is PayCost payCost))
                    {
                        throw new ArgumentOutOfRangeException("Unknown mandatory multiple card selection.");
                    }
                }
                else if (cardSelection is MultipleCardSelection multipleCardSelection)
                {
                    if (multipleCardSelection is DeclareShieldTriggers declareShieldTriggers)
                    {
                        if (declareShieldTriggers.SelectedCards.Count > 0)
                        {
                            LogMessages.Add(string.Format("{0} declared to use the following shield triggers: {1}", declareShieldTriggers.Player.Name, string.Join("; ", declareShieldTriggers.SelectedCards.Select(c => c.Name))));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("multipleCardSelection");
                    }
                }
                else
                {
                    throw new InvalidOperationException("cardSelection");
                }
            }
            else if (playerAction is CreatureSelection creatureSelection)
            {
                if (creatureSelection is OptionalCreatureSelection optionalCreatureSelection)
                {
                    if (optionalCreatureSelection is DeclareAttacker declareAttacker)
                    {
                        if (declareAttacker.SelectedCreature != null)
                        {
                            LogMessages.Add(string.Format("{0} declared {1} as an attacking creature.", declareAttacker.Player.Name, declareAttacker.SelectedCreature.Name));
                        }
                    }
                    else if (optionalCreatureSelection is DeclareTargetOfAttack declareTargetOfAttack)
                    {
                        if (declareTargetOfAttack.SelectedCreature != null)
                        {
                            LogMessages.Add(string.Format("{0} declared {1} as the target of attack.", declareTargetOfAttack.Player.Name, declareTargetOfAttack.SelectedCreature.Name));
                        }
                        else
                        {
                            LogMessages.Add(string.Format("{0} declared {1} as the target of attack.", declareTargetOfAttack.Player.Name, _duel.GetOpponent(declareTargetOfAttack.Player).Name));
                        }
                    }
                    else if (optionalCreatureSelection is DeclareBlock declareBlock)
                    {
                        if (declareBlock.SelectedCreature != null)
                        {
                            LogMessages.Add(string.Format("{0} blocked the attack with {1}.", declareBlock.Player.Name, declareBlock.SelectedCreature.Name));
                        }
                    }
                    else if (optionalCreatureSelection is YouMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand youMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand)
                    {
                        if (youMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand.SelectedCreature != null)
                        {
                            LogMessages.Add(string.Format("{0} returned {1} to its owner's hand.", youMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand.Player.Name, youMayChooseACreatureInTheBattleZoneAndReturnItToItsOwnersHand.SelectedCreature.Name));
                        }
                    }
                    else if (optionalCreatureSelection is SoulSwapSelection soulSwapSelection)
                    {
                        if (soulSwapSelection.SelectedCreature != null)
                        {
                            LogMessages.Add(string.Format("{0} put {1}'s {2} from the battle zone into its owner's mana zone.", optionalCreatureSelection.Player.Name, _duel.GetOwner(soulSwapSelection.SelectedCreature).Name, soulSwapSelection.SelectedCreature.Name));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("optionalCreatureSelection");
                    }
                }
                else if (creatureSelection is MandatoryCreatureSelection mandatoryCreatureSelection)
                {
                    if (mandatoryCreatureSelection is ChooseANonEvolutionCreatureInThatPlayersManaZoneThatCostsTheSameAsOrLessThanTheNumberOfCardsInThatManaZoneThatPlayerPutsThatCreatureIntoTheBattleZone c)
                    {
                        if (c.SelectedCreature != null)
                        {
                            LogMessages.Add(string.Format("{0} put {1} from their mana zone into the battle zone.", _duel.GetOwner(c.SelectedCreature).Name, c.SelectedCreature.Name));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("mandatoryCreatureSelection");
                    }
                }
                else
                {
                    throw new InvalidOperationException("creatureSelection");
                }
            }
            else if (playerAction is SelectAbilityToResolve selectAbilityToResolve)
            {
                LogMessages.Add(string.Format("{0} started resolving an ability generated by {1}.", playerAction.Player.Name, selectAbilityToResolve.SelectedAbility.Source.Name));
            }
            else if (!(playerAction is OptionalAction))
            {
                throw new ArgumentOutOfRangeException("Unknown player action.");
            }
        }

        private void SetupShieldsButton(bool player1)
        {
            Button buttonShields = _buttonPlayer1Shields;
            TextBlock textBlockShields = _textBlockPlayer1Shields;
            string playerName = "Player1";
            if (!player1)
            {
                buttonShields = _buttonPlayer2Shields;
                textBlockShields = _textBlockPlayer2Shields;
                playerName = "Player2";
            }
            Canvas canvasPlayerShields = new Canvas()
            {
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/shield.png", UriKind.Relative)),
                    Stretch = Stretch.Fill,
                },
            };
            canvasPlayerShields.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = buttonShields });
            canvasPlayerShields.SetBinding(WidthProperty, new Binding("ActualWidth") { Source = buttonShields });

            Grid gridPlayerShields = new Grid();
            gridPlayerShields.ColumnDefinitions.Add(new ColumnDefinition());
            gridPlayerShields.RowDefinitions.Add(new RowDefinition());

            gridPlayerShields.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = canvasPlayerShields });
            gridPlayerShields.SetBinding(WidthProperty, new Binding("ActualWidth") { Source = canvasPlayerShields });

            textBlockShields.SetBinding(TextBlock.TextProperty, new Binding(string.Format("{0}.ShieldZone.Cards.Count", playerName)) { Source = _duel });
            gridPlayerShields.Children.Add(textBlockShields);
            canvasPlayerShields.Children.Add(gridPlayerShields);

            buttonShields.Content = canvasPlayerShields;
        }

        private void SetupHandButton(bool player1)
        {
            Button buttonHand = _buttonPlayer1Hand;
            TextBlock textBlockHand = _textBlockPlayer1Hand;
            string playerName = "Player1";
            if (!player1)
            {
                buttonHand = _buttonPlayer2Hand;
                textBlockHand = _textBlockPlayer2Hand;
                playerName = "Player2";
            }
            Canvas canvasPlayerHand = new Canvas()
            {
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/hand.png", UriKind.Relative)),
                    Stretch = Stretch.Fill,
                },
            };
            canvasPlayerHand.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = buttonHand });
            canvasPlayerHand.SetBinding(WidthProperty, new Binding("ActualWidth") { Source = buttonHand });

            Grid gridPlayerHand = new Grid();
            gridPlayerHand.ColumnDefinitions.Add(new ColumnDefinition());
            gridPlayerHand.RowDefinitions.Add(new RowDefinition());

            gridPlayerHand.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = canvasPlayerHand });
            gridPlayerHand.SetBinding(WidthProperty, new Binding("ActualWidth") { Source = canvasPlayerHand });

            textBlockHand.SetBinding(TextBlock.TextProperty, new Binding(string.Format("{0}.Hand.Cards.Count", playerName)) { Source = _duel });
            gridPlayerHand.Children.Add(textBlockHand);
            canvasPlayerHand.Children.Add(gridPlayerHand);

            buttonHand.Content = canvasPlayerHand;
        }

        private void SetupGraveyardButton(bool player1)
        {
            Button buttonGraveyard = _buttonPlayer1Graveyard;
            TextBlock textBlockGraveyard = _textBlockPlayer1Graveyard;
            string playerName = "Player1";
            if (!player1)
            {
                buttonGraveyard = _buttonPlayer2Graveyard;
                textBlockGraveyard = _textBlockPlayer2Graveyard;
                playerName = "Player2";
            }
            Canvas canvasPlayerGraveyard = new Canvas()
            {
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/graveyard.png", UriKind.Relative)),
                    Stretch = Stretch.Fill,
                },
            };
            canvasPlayerGraveyard.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = buttonGraveyard });
            canvasPlayerGraveyard.SetBinding(WidthProperty, new Binding("ActualWidth") { Source = buttonGraveyard });

            Grid gridPlayerGraveyard = new Grid();
            gridPlayerGraveyard.ColumnDefinitions.Add(new ColumnDefinition());
            gridPlayerGraveyard.RowDefinitions.Add(new RowDefinition());

            gridPlayerGraveyard.SetBinding(HeightProperty, new Binding("ActualHeight") { Source = canvasPlayerGraveyard });
            gridPlayerGraveyard.SetBinding(WidthProperty, new Binding("ActualWidth") { Source = canvasPlayerGraveyard });

            textBlockGraveyard.SetBinding(TextBlock.TextProperty, new Binding(string.Format("{0}.Graveyard.Cards.Count", playerName)) { Source = _duel });
            gridPlayerGraveyard.Children.Add(textBlockGraveyard);
            canvasPlayerGraveyard.Children.Add(gridPlayerGraveyard);

            buttonGraveyard.Content = canvasPlayerGraveyard;
        }

        /*
        private void GetJsonCardsTest()
        {
            Collection<JsonCard> jsonCards = JsonCardFactory.GetJsonCards(JsonPath);
            System.Collections.Generic.List<XmlCard> xmlCards = jsonCards.Select(j => new XmlCard() { Set = j.Set, Id = j.Id, Amount = 1 }).ToList();
            XmlSerializer writer = new XmlSerializer(typeof(System.Collections.Generic.List<XmlCard>));
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//AllCards.xml";
            FileStream file = File.Create(path);
            writer.Serialize(file, xmlCards);
            file.Close();
        }
        */

        private void UpdateZoomCard(int gameId)
        {
            ZoomCard = _duel.GetCard(gameId);
        }

        private void ToggleVisibility(UIElement uiElement)
        {
            if (uiElement.Visibility == Visibility.Hidden)
            {
                uiElement.Visibility = Visibility.Visible;
            }
            else if (uiElement.Visibility == Visibility.Visible)
            {
                uiElement.Visibility = Visibility.Hidden;
            }
        }

        private void HideZoneCanvases()
        {
            foreach (Canvas canvas in new List<Canvas>() { _player1ShieldZoneCanvas, _player2ShieldZoneCanvas, _player1GraveyardCanvas, _player2GraveyardCanvas, _player1HandCanvas, _player2HandCanvas })
            {
                canvas.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateSelectedCards(AbstractCardCanvas cardCanvas, ReadOnlyCardCollection cards)
        {
            GameCard card = cards.First(c => c.GameId == cardCanvas.GameId);
            if (!SelectedCards.Contains(card))
            {
                SelectedCards = new ObservableCardCollection(SelectedCards) { card };
            }
            else
            {
                ObservableCardCollection newCards = new ObservableCardCollection(SelectedCards);
                newCards.Remove(card);
                SelectedCards = newCards;
            }
        }
        #endregion Private methods

        #region Events
        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _mainGrid.Width = e.NewSize.Width;
            _mainGrid.Height = e.NewSize.Height;
            
            Canvas.SetLeft(_setupCanvas, (e.NewSize.Width - _setupCanvas.Width) / 2);
            Canvas.SetTop(_setupCanvas, (e.NewSize.Height - _setupCanvas.Height) / 2);

            Canvas.SetTop(_zoomCardCanvas, (e.NewSize.Height - _zoomCardCanvas.Height) / 2);
            List<Canvas> canvases = new List<Canvas>() { _player1ShieldZoneCanvas, _player2ShieldZoneCanvas, _player1GraveyardCanvas, _player2GraveyardCanvas, _player1HandCanvas, _player2HandCanvas, _activePlayerPendingAbilitiesCanvas, _nonActivePlayerPendingAbilitiesCanvas };
            foreach (Canvas canvas in canvases)
            {
                canvas.Width = 0.6 * e.NewSize.Width;
                canvas.Height = 0.35 * e.NewSize.Height;
                Canvas.SetLeft(canvas, (e.NewSize.Width - canvas.Width) / 2);
                Canvas.SetTop(canvas, (e.NewSize.Height - canvas.Height) / 2);
            }
        }

        private void PlayerActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object newItem in e.NewItems)
                {
                    PlayerAction playerAction = newItem as PlayerAction;
                    LogPlayerAction(playerAction);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void LogMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (object item in e.NewItems)
            {
                string text = item as string;
                if (!string.IsNullOrEmpty(_textBoxLog.Text))
                {
                    _textBoxLog.AppendText("\r\n");
                    if (text.Contains("started turn"))
                    {
                        _textBoxLog.AppendText("\r\n");
                    }
                }
                _textBoxLog.AppendText(text);
            }
            _textBoxLog.ScrollToEnd();
        }

        private void Turns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (object item in e.NewItems)
            {
                Turn turn = item as Turn;
                turn.Steps.CollectionChanged += Steps_CollectionChanged;
                LogMessages.Add(string.Format("{0} started turn {1}.", turn.ActivePlayer.Name, turn.Number));
            }
        }

        private void Steps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (object item in e.NewItems)
            {
                Step step = item as DuelMastersModels.Steps.Step;
                step.PlayerActions.CollectionChanged += PlayerActions_CollectionChanged1;
            }
        }

        private void PlayerActions_CollectionChanged1(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (object item in e.NewItems)
            {
                PlayerAction playerAction = item as PlayerAction;
                LogPlayerAction(playerAction);
            }
        }

        /// <summary>
        /// TODO: test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservableContinuousEffects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    ContinuousEffect continuousEffect = item as ContinuousEffect;
                    LogMessages.Add(string.Format("Added continuous effect: {0} Period: {1}.", continuousEffect.ToString(), continuousEffect.Period.ToString()));

                    if (continuousEffect is PowerEffect powerEffect)
                    {
                        /*BattleZoneCreatureCanvas.PowerProperty.
                        var rect = new BattleZoneCreatureCanvas();
                        rect.prop*/
                        //powerEffect.
                    }
                    else
                    {
                        throw new NotImplementedException(continuousEffect.ToString());
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    ContinuousEffect continuousEffect = item as ContinuousEffect;
                    LogMessages.Add(string.Format("Removed continuous effect: {0} Period: {1}.", continuousEffect.ToString(), continuousEffect.Period.ToString()));

                    //TODO: powereffect
                }
            }
            else
            {
                throw new NotImplementedException(string.Format("NotifyCollectionChangedAction not supported: {0}", e.Action));
            }
        }

        public void AbilityCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AbilityCanvas abilityCanvas = sender as AbilityCanvas;
            UpdateViewToShowPlayerAction(_duel.Progress(new SelectAbilityToResolveResponse(abilityCanvas.Ability)));
        }

        private void CardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AbstractCardCanvas cardCanvas = sender as AbstractCardCanvas;
            if (_duel.CurrentPlayerAction is CardSelection cardSelection)
            {
                if (cardSelection.CardIds.Contains(cardCanvas.GameId))
                {
                    if (cardSelection is OptionalCardSelection || cardSelection is MandatoryCardSelection mandatoryCardSelection)
                    {
                        UpdateViewToShowPlayerAction(_duel.Progress(new CardSelectionResponse(new ReadOnlyCardCollection(cardSelection.Cards.Where(c => c.GameId == cardCanvas.GameId).ToList()))));
                    }
                    else if (cardSelection is MandatoryMultipleCardSelection mandatoryMultipleCardSelection)
                    {
                        UpdateSelectedCards(cardCanvas, mandatoryMultipleCardSelection.Cards);
                        if (mandatoryMultipleCardSelection.Validate(new ReadOnlyCardCollection(SelectedCards)) && !(mandatoryMultipleCardSelection is PayCost payCost && !PayCost.Validate(new ReadOnlyCardCollection(SelectedCards), (_duel.CurrentTurn.CurrentStep as MainStep).CardToBeUsed)))
                        {
                            CardSelectionResponse response = new CardSelectionResponse(new ReadOnlyCardCollection(SelectedCards));
                            UpdateViewToShowPlayerAction(_duel.Progress(response));
                            SelectedCards = new ObservableCardCollection();
                        }
                    }
                    else if (cardSelection is MultipleCardSelection multipleCardSelection)
                    {
                        UpdateSelectedCards(cardCanvas, multipleCardSelection.Cards);
                    }
                }
            }
            else if (_duel.CurrentPlayerAction is CreatureSelection creatureSelection)
            {
                if (creatureSelection.CreatureIds.Contains(cardCanvas.GameId))
                {
                    UpdateViewToShowPlayerAction(_duel.Progress(new CreatureSelectionResponse(new ReadOnlyCreatureCollection(creatureSelection.Creatures.Where(c => c.GameId == cardCanvas.GameId).ToList()))));
                }
            }
            else if (!(_duel.CurrentPlayerAction is OptionalAction) && !(_duel.CurrentPlayerAction is SelectAbilityToResolve))
            {
                throw new ArgumentException("Unknown player action.");
            }
        }

        private void _actionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_duel.CurrentPlayerAction is CardSelection)
            {
                UpdateViewToShowPlayerAction(_duel.Progress(new CardSelectionResponse(new ReadOnlyCardCollection(SelectedCards))));
                SelectedCards = new ObservableCardCollection();
            }
            else if (_duel.CurrentPlayerAction is OptionalCreatureSelection)
            {
                UpdateViewToShowPlayerAction(_duel.Progress(new CreatureSelectionResponse(new ReadOnlyCreatureCollection(SelectedCards.Cast<GameCreature>().ToList()))));
                SelectedCards = new ObservableCardCollection();
            }
            else if (_duel.CurrentPlayerAction is OptionalAction optionalAction)
            {
                UpdateViewToShowPlayerAction(_duel.Progress(new OptionalActionResponse(true)));
            }
            else
            {
                throw new ArgumentOutOfRangeException("Player action");
            }
        }

        private void _declineButton_Click(object sender, RoutedEventArgs e)
        {
            if (_duel.CurrentPlayerAction is OptionalAction optionalAction)
            {
                UpdateViewToShowPlayerAction(_duel.Progress(new OptionalActionResponse(false)));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void _buttonPlayer1Hand_Click(object sender, RoutedEventArgs e)
        {
            HideZoneCanvases();
            ToggleVisibility(_player1HandCanvas);
        }

        private void _buttonPlayer1Shields_Click(object sender, RoutedEventArgs e)
        {
            HideZoneCanvases();
            ToggleVisibility(_player1ShieldZoneCanvas);
        }

        private void _buttonPlayer1Graveyard_Click(object sender, RoutedEventArgs e)
        {
            HideZoneCanvases();
            ToggleVisibility(_player1GraveyardCanvas);
        }

        private void _buttonPlayer2Hand_Click(object sender, RoutedEventArgs e)
        {
            HideZoneCanvases();
            ToggleVisibility(_player2HandCanvas);
        }

        private void _buttonPlayer2Shields_Click(object sender, RoutedEventArgs e)
        {
            HideZoneCanvases();
            ToggleVisibility(_player2ShieldZoneCanvas);
        }

        private void _buttonPlayer2Graveyard_Click(object sender, RoutedEventArgs e)
        {
            HideZoneCanvases();
            ToggleVisibility(_player2GraveyardCanvas);
        }
        #endregion Events
    }

    #region Converters
    public class SetAndIdConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                string wholeId = values[1].ToString();
                string id = wholeId.Split('/')[0];
                return new SetAndId() { Set = values[0].ToString(), Id = id };
            }
            else
            {
                throw new Exception("Expected 2 values.");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class ObjectToCardTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type = value.GetType();
            if (type == typeof(GameEvolutionCreature))
            {
                return "Evolution Creature";
            }
            else if (type == typeof(GameCreature))
            {
                return "Creature";
            }
            else if (type == typeof(GameSpell))
            {
                return "Spell";
            }
            /*else if (type == typeof(GameCrossGear))
            {
                return "Cross Gear";
            }*/
            else
            {
                throw new Exception("Unknown card type.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

    [ValueConversion(typeof(Collection<string>), typeof(string))]
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Join("\r\n", ((Collection<string>)value).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(PlayerAction), typeof(Collection<int>))]
    public class PlayerActionToIntCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is CardSelection cardSelection)
            {
                return cardSelection.CardIds;
            }
            else if (value is CreatureSelection creatureSelection)
            {
                return creatureSelection.CreatureIds;
            }
            else
            {
                return new Collection<int>();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(ObservableCardCollection), typeof(Collection<int>))]
    public class CardCollectionToIntCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCardCollection cards = value as ObservableCardCollection;
            return new Collection<int>(cards.Select(c => c.GameId).ToList());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class ListViewSizeToCardCanvasSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)parameter * (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
    #endregion Converters
}
