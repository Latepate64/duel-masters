using DuelMastersModels.Cards;
using DuelMastersModels.Factories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DuelMastersDeckBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        ListView CollectionListView = new ListView()
        {
            Name = "CollectionListView"
        };
        FilterWindow _filterWindow = new FilterWindow();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _filterWindow.FilterApplied += _filterWindow_FilterApplied;
        }

        private void _filterWindow_FilterApplied(object sender, System.EventArgs e)
        {
            CollectionViewSource.GetDefaultView(CollectionListView.ItemsSource).Refresh();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "DuelMastersCards.json");
            string path = "C:\\duel-masters-json\\DuelMastersCards3.json"; //TODO: get from gui
            Collection<JsonCard> jsonCards = JsonCardFactory.GetJsonCards(path);
            var cardViewModels = jsonCards.Select(c => new CardViewModel()
            {
                Civilization = string.Join(" / ", c.Civilizations),
                Cost = c.Cost,
                Flavor = c.Flavor ?? "",
                Id = c.Id,
                Illustrator = c.Illustrator,
                Name = c.Name,
                Power = c.Power ?? "-",
                Race = c.Races != null ? string.Join(" / ", c.Races) : "-",
                Rarity = c.Rarity,
                Set = c.Set,
                Text = c.Text,
                Type = c.CardType,
            });
            MainGrid.Children.Add(CollectionListView);
            ModifyListViewForCards(CollectionListView);
            CollectionListView.ItemsSource = cardViewModels;

            var sets = cardViewModels.Select(c => c.Set).Distinct().ToList();
            _filterWindow.Sets = sets;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CollectionListView.ItemsSource);
            view.Filter = FilterByCardName;
        }

        private bool FilterByCardName(object item)
        {
            var nameFilter = _filterWindow.NameFilter.Text;
            if (string.IsNullOrEmpty(nameFilter))
            {
                return true;
            }
            else
            {
                //culture.CompareInfo.IndexOf(paragraph, word, CompareOptions.IgnoreCase) >= 0
                //return (item as CardViewModel).Name.Contains(nameFilter);
                return ((item as CardViewModel).Name.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void ModifyListViewForCards(ListView listView)
        {
            GridView gridView = new GridView();
            gridView.Columns.Add(GetGridViewColumn("Name", 130));
            gridView.Columns.Add(GetGridViewColumn("Set", 100));
            gridView.Columns.Add(GetGridViewColumn("Id", 50));
            gridView.Columns.Add(GetGridViewColumn("Civilization", 100));
            gridView.Columns.Add(GetGridViewColumn("Rarity", 100));
            gridView.Columns.Add(GetGridViewColumn("Type", 100));
            gridView.Columns.Add(GetGridViewColumn("Cost", 30));
            gridView.Columns.Add(GetGridViewColumn("Text", 100));
            gridView.Columns.Add(GetGridViewColumn("Flavor", 100));
            gridView.Columns.Add(GetGridViewColumn("Illustrator", 100));
            gridView.Columns.Add(GetGridViewColumn("Race", 100));
            gridView.Columns.Add(GetGridViewColumn("Power", 100));
            listView.View = gridView;
        }

        private GridViewColumn GetGridViewColumn(string name, int width)
        {
            GridViewColumnHeader gridViewColumnHeader = new GridViewColumnHeader() { Content = name, Name = name, Tag = name };
            gridViewColumnHeader.Click += GridViewColumnHeader_Click;
            GridViewColumn gridViewColumn = new GridViewColumn() { DisplayMemberBinding = new Binding(name), Header = gridViewColumnHeader, Width = width };
            return gridViewColumn;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection direction;
            if (e.OriginalSource is GridViewColumnHeader headerClicked)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;
                    Sort(sortBy, direction, CollectionListView);
                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }
                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }
                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction, ListView listView)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            _filterWindow.Show();
        }
    }
}
        /*
<ListView x:Name="CollectionListView" Grid.Column="0" ScrollViewer.HorizontalScrollBarVisibility="Auto">
   <ListView.View>
       <GridView x:Name="CollectionGridView">
           <GridViewColumn Header = "Name" Click DisplayMemberBinding = "{Binding Path=Name}" Width="120"/>
           <GridViewColumn Header = "Set" DisplayMemberBinding="{Binding Path=Set}" Width="100"/>
           <GridViewColumn Header = "Id" DisplayMemberBinding="{Binding Path=Id}" Width="50"/>
           <GridViewColumn Header = "Civilization" DisplayMemberBinding="{Binding Path=Civilizations}" Width="100"/>
           <GridViewColumn Header = "Rarity" DisplayMemberBinding="{Binding Path=Rarity}" Width="100"/>
           <GridViewColumn Header = "Type" DisplayMemberBinding="{Binding Path=CardType}" Width="100"/>
           <GridViewColumn Header = "Cost" DisplayMemberBinding="{Binding Path=Cost}" Width="30"/>
           <GridViewColumn Header = "Text" DisplayMemberBinding="{Binding Path=Text}" Width="100"/>
           <GridViewColumn Header = "Flavor" DisplayMemberBinding="{Binding Path=Flavor}" Width="100"/>
           <GridViewColumn Header = "Illustrator" DisplayMemberBinding="{Binding Path=Illustrator}" Width="100"/>
           <GridViewColumn Header = "Race" DisplayMemberBinding="{Binding Path=Race}" Width="100"/>
           <GridViewColumn Header = "Power" DisplayMemberBinding="{Binding Path=Power}" Width="100"/>
       </GridView>
   </ListView.View>
</ListView>*/
