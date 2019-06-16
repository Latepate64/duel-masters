using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DuelMastersDeckBuilder
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public event EventHandler FilterApplied;

        private List<string> _sets = new List<string>();
        public List<string> Sets
        {
            get { return _sets; }
            set { _sets = value; AddSetsToListBox(value); }
        } 

        public FilterWindow()
        {
            InitializeComponent();
        }

        private void AddSetsToListBox(List<string> sets)
        {
            SetListBox.Items.Clear();
            foreach (var set in sets)
            {
                var listBoxItem = new ListBoxItem()
                {
                    Name = string.Format("ListBoxItem_{0}", set.Replace(' ', '_').Replace('-', '_')),
                    
                };
                //listbox
                SetListBox.Items.Add(listBoxItem);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterApplied?.Invoke(this, EventArgs.Empty);
            Hide();
        }
    }
}
