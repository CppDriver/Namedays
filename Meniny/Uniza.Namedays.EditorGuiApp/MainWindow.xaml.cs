using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public string[] Months { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //Months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            Months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Where(m => !string.IsNullOrEmpty(m)).ToArray();
            DataContext = this;
        }

        private void MenuItem_AccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
        {
            if (((MenuItem)sender).IsSubmenuOpen == false )
            {
                ((MenuItem)sender).IsSubmenuOpen = true;
            }
            else
            {
                ((MenuItem)sender).IsSubmenuOpen = false;
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
