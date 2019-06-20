using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DuelMastersDeckBuilder
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public event EventHandler FilterApplied;

        public FilterWindow()
        {
            InitializeComponent();
            DataContext = new MyViewModel();
            //SetListBox.ItemsSource = Sets;
            /*var dataTemplate = new DataTemplate();
            dataTemplate.Template = new TextBlock();
            SetListBox.ItemTemplate = dataTemplate;*/
        }

        /*private void AddSetsToListBox(List<string> sets)
        {
            SetListBox.ItemsSource = 
            SetListBox.Items.Clear();
            foreach (var set in sets)
            {
                var listBoxItem = new ListBoxItem()
                {
                    Name = string.Format("ListBoxItem_{0}", set.Replace(' ', '_').Replace('-', '_')),
                };
                listBoxItem.
                SetListBox.Items.Add(listBoxItem);
            }
        }*/

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterApplied?.Invoke(this, EventArgs.Empty);
            Hide();
        }
    }

    public class MyViewModel
    {
        /*private List<string> _sets = new List<string>();
        public List<string> Sets
        {
            get { return _sets; }
            set { _sets = value; AddSetsToListBox(value); }
        }*/
        public ObservableCollection<string> Sets { get; set; }
        //public List<string> Sets { get; set; } = new List<string>();
    }
}
