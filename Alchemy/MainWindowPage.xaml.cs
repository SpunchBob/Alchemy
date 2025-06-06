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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alchemy
{
    /// <summary>
    /// Логика взаимодействия для MainWindowPage.xaml
    /// </summary>
    public partial class MainWindowPage : Page
    {
        public MainWindowPage()
        {
            InitializeComponent();
        }

        private void LabButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LaboratoryPage());
        }
    }
}
