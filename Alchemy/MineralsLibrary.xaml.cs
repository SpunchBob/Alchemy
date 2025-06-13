using Alchemy;
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
    /// Логика взаимодействия для MineralsLibrary.xaml
    /// </summary>
    public partial class MineralsLibrary : Page
    {
        private Poisk poisk;
        private string currentChemicalName;
        private bool isShowingCrystalStructure = false; // Флаг для текущего состояния изображения
        public MineralsLibrary()
        {
            InitializeComponent();
            poisk = new Poisk();
            ChemicalImage.Visibility = Visibility.Hidden;// Скрыто изображение при старте
        }
        // Обработчик нажатия кнопки поиска
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            var result = poisk.Search(query);

            if (result != null)// Если вещество найдено
            {
                try
                {
                    // Отображение изображения вещества
                    ChemicalImage.Visibility = Visibility.Visible;
                    var imagePath = Images.ChemicalImages[result.Name];

                    // Загрузка изображения
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                    bitmapImage.EndInit();
                    ChemicalImage.Source = bitmapImage;

                    // Сохранение текущего состояния
                    currentChemicalName = result.Name;
                    isShowingCrystalStructure = false;

                    // Обновление названия и формулы вещества
                    SubstanceName.Text = result.Name;
                    if (Biblio.Chemicals.TryGetValue(result.Name, out var chemicalInfo))
                    {
                        SubstanceFormula.Text = chemicalInfo.Formula;
                    }
                    else
                    {
                        SubstanceFormula.Text = string.Empty;
                    }

                    InteractionHint.Text = "Нажмите на изображение чтобы увидеть его кристаллическую решётку";
                    InfoText.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
                }
            }
            else // Если совпадения нет
            {
                var potentialMatches = poisk.SearchPartial(query);
                if (potentialMatches.Count > 0)
                {
                    MessageBox.Show("Найдено несколько веществ: " + string.Join(", ", potentialMatches));
                }
                else
                {
                    MessageBox.Show("Такого вещества нет.");
                    // Очищаем все данные
                    ClearChemicalData();
                }
            }
        }

        // Метод для очистки данных о веществе
        private void ClearChemicalData()
        {
            // Сброс всех визуальных элементов
            ChemicalImage.Visibility = Visibility.Hidden;
            ChemicalImage.Source = null;
            currentChemicalName = null;
            isShowingCrystalStructure = false;

            // Очистка текстовых полей
            SubstanceName.Text = string.Empty;
            SubstanceFormula.Text = string.Empty;
            InteractionHint.Text = string.Empty;
            InfoText.Text = string.Empty;
        }

        // Обработчик нажатия кнопки раздела 
        private void SectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string sectionName = button.DataContext.ToString();

                // Проверка выбранно ли вещество
                if (!string.IsNullOrEmpty(currentChemicalName))
                {
                    var sectionInfo = SubstanceInfo.GetInfo(currentChemicalName);// Получение информации о разделе
                    if (sectionInfo != null && sectionInfo.ContainsKey(sectionName))
                    {
                        InfoText.Text = $"{sectionName}:\n{sectionInfo[sectionName]}";// Отображение информации
                    }
                    else
                    {
                        InfoText.Text = "Запрашиваемая информация не найдена.";
                    }
                }
                else
                {
                    MessageBox.Show("Сначала выполните поиск вещества!", "Предупреждение",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    InfoText.Text = string.Empty; // Очищаем информацию, если вещество не найдено
                }
            }
        }

        // Вспомогательный метод для получения названия вещества по изображению
        private string GetChemicalName(BitmapImage bitmapImage)
        {
            foreach (var pair in Images.ChemicalImages)
            {
                if (pair.Value == bitmapImage.UriSource.ToString())
                {
                    return pair.Key; // Возвращаем название вещества
                }
            }
            return string.Empty; // Если не найдено
        }

        // Обработчик клика по изображению
        private void ChemicalImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(currentChemicalName))
            {
                MessageBox.Show("Сначала выполните поиск вещества!");
                return;
            }

            try
            {
                if (!isShowingCrystalStructure)
                {
                    // Показываем кристаллическую решётку
                    if (Images.CrystalStructureImages.ContainsKey(currentChemicalName))
                    {
                        var crystalImage = new BitmapImage();
                        crystalImage.BeginInit();
                        crystalImage.UriSource = new Uri(Images.CrystalStructureImages[currentChemicalName], UriKind.RelativeOrAbsolute);
                        crystalImage.EndInit();
                        ChemicalImage.Source = crystalImage;

                        // Обновляем текст
                        SubstanceName.Text = "Кристаллическая решётка " + currentChemicalName;
                        InteractionHint.Text = "Нажмите на изображение, чтобы увидеть вещество";
                        isShowingCrystalStructure = true;
                    }
                    else
                    {
                        MessageBox.Show("Нет доступного изображения кристаллической решётки для выбранного вещества.");
                    }
                }
                else  // Если в данный момент отображается решётка
                {
                    // Возвращаем исходное изображение вещества
                    var substanceImage = new BitmapImage();
                    substanceImage.BeginInit();
                    substanceImage.UriSource = new Uri(Images.ChemicalImages[currentChemicalName], UriKind.RelativeOrAbsolute);
                    substanceImage.EndInit();
                    ChemicalImage.Source = substanceImage;

                    // Обновляем текст
                    SubstanceName.Text = currentChemicalName;
                    if (Biblio.Chemicals.TryGetValue(currentChemicalName, out var chemicalInfo))
                    {
                        SubstanceFormula.Text = chemicalInfo.Formula;
                    }
                    InteractionHint.Text = "Нажмите на изображение чтобы увидеть его кристаллическую решётку";
                    isShowingCrystalStructure = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
