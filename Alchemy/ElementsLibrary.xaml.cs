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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Alchemy.Code;
using System.Text.Json;
using System.IO;

namespace Alchemy
{
    /// <summary>
    /// Логика взаимодействия для ElementsLibrary.xaml
    /// </summary>
    public class ImageCollection
    {
        public List<System.Windows.Controls.Image> Images { get; } = new List<System.Windows.Controls.Image> { };
    }
    public partial class ElementsLibrary : Page
    {
        private List<ChemicalElement> elements;
        private ImageCollection imageCollection = new ImageCollection();
        private HashSet<string> addedImagePaths = new HashSet<string>(); // хранит пути изображений


        public ElementsLibrary()
        {
            InitializeComponent();

            // Загрузка JSON
            string json = File.ReadAllText("info.json");
            elements = JsonSerializer.Deserialize<List<ChemicalElement>>(json);
        }

        private void Button_Calсium_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Кальций");
        }

        private void Button_Magnium_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Магний");
        }

        private void Button_Calium_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Калий");
        }

        private void Button_Lithium_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Литий");
        }

        private void Button_Sodium_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Натрий");
        }

        private void Button_Hydrogen_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Водород");
        }

        private void Button_Oxygen_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Кислород");
        }

        private void Button_Zinc_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Цинк");
        }

        private void Button_Aluminum_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Алюминий");
        }

        private void Button_Carbonic_acid_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Угольная кислота");
        }

        private void Button_Sulfuric_acid_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Серная кислота");
        }

        private void Button_Hydrochloric_acid_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Соляная кислота");
        }

        private void Button_Water_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Вода");
        }

        private void Buttom_Sodium_hydroxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Гидкроксид натрия");
        }

        private void Buttom_Potassium_hydroxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Гидкроксид калия");
        }

        private void Buttom_Calcium_hydroxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Гидкроксид калия");
        }

        private void Buttom_Ammonium_hydroxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Гидкроксид аммония");
        }

        private void Buttom_Calcium_oxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Оксид кальция");
        }

        private void Buttom_Magnesium_oxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Оксид магния");
        }

        private void Buttom_Copper_oxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Оксид меди(II)");
        }

        private void Buttom_Zinc_oxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Оксид цинка");
        }

        private void Buttom_Aluminum_oxide_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo("Оксид алюминия");
        }

        private void ToLab(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LaboratoryPage());
        }

        private void ToMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainWindowPage());
        }




        private void ShowInfo(string name)
        /*Метод для вывода ифнормации о элементе
        принимает: название элементе
        return: всю информацию
        */
        {
            var element = elements.FirstOrDefault(e => e.Name == name);
            if (element == null) return;

            NameBlock.Text = $"{element.Name}";
            SymbolBlock.Text = $"Символ: {element.Symbol}";
            NumberBlock.Text = $"Номер: {element.Number}";
            MassBlock.Text = $"Масса: {element.Mass}";
            DescriptionBlock.Text = $"Описание: {element.Description}";
            HowToGetBlock.Text = $"Как получить: {element.How_to_get}";
            UsingBlock.Text = $"Использование: {element.Using}";

            // Вставка изображения
            try
            {
                var imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, element.Image_path);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                Console.WriteLine(imagePath.ToString());
                bitmap.UriSource = new Uri(imagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                // Устанавливаем в основной Image
                ElementImage.Source = bitmap;

                // Копия изображения в коллекцию
                if (!addedImagePaths.Contains(imagePath))
                {
                    addedImagePaths.Add(imagePath);
                    var imageCopy = new System.Windows.Controls.Image
                    {
                        Source = bitmap,
                        Width = 100,
                        Height = 100,
                    };

                    Console.WriteLine("Элемент добавлен в коллекцию");
                    imageCollection.Images.Add(imageCopy);
                }
                else
                {
                    MessageBox.Show("Этот элемент уже выбран!");
                }
            }
            catch (Exception ex)
            {
                // Можно логировать или выводить заглушку
                ElementImage.Source = null;
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
            }

            ChoosenPanel.Children.Clear();


            foreach (var bitmapImage in imageCollection.Images)
            {
                    Console.WriteLine("Изображение добавленно в коллекцию");
                    ChoosenPanel.Children.Add(bitmapImage);
              
            }
        }
       
    }
}
