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

        private void LabButton_Click(object sender, RoutedEventArgs e)  // Переход в библиотеку реагентов
        {
            NavigationService.Navigate(new ElementsLibrary());
        }
        private void MineralButton_Click(object sender, RoutedEventArgs e) // Переход в библиотеку минералов
        {
            NavigationService.Navigate(new MineralsLibrary());
        }
        private void Exit(object sender, RoutedEventArgs e) // Закрытие окна
        {
            if (MessageBox.Show("Выйти из приложения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e) // Закрытие окна
        {
            string Text = "Библиотка минералов (разработчик - Волкова Ника) - список различных минералов и других веществ," +
                " с полной информацией о них: фото, физические свойства, молекулярная структура, роль в природе, применение человеком и тд.\n\n" +
                "Библиотека реагентов (разработчик - Шульга Иван) - список химических реагентов, где пользователь может выбрать нужные ему для проведения реакции." +
                " В этом модуле также можно получить полную информацию о веществах: фото, физические свойства, применение человеком и тд.\n\n" +
                "Лаборатория (разработчик - Прокудин Александр) - рабочий стол, где вы можете смешивать различный вещества, выбранные в библиотеке реагентов." +
                " Помимо новго вещества в результате смешивания, Вы получите уравнение этой реакции.";

            MessageBox.Show(Text, "Справка о программе", MessageBoxButton.OK, MessageBoxImage.Information);


        }
    }
}
