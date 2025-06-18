using Alchemy.Code;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Odbc;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alchemy
{
    /// <summary>
    /// Логика взаимодействия для LaboratoryPage.xaml
    /// </summary>
    public partial class LaboratoryPage : Page
    {
        static private Point offset;
        private Image selectedImage;
        private Image selectedImage_ToMove;
        public LaboratoryPage()
        {
            InitializeComponent();
            Objects_Creator(MainPanelGrid);
            SetData(MainPanelGrid, ElementsLibrary.GetData());
            GetObjName(MainPanelGrid);
        }

        // Массив, полученный из модуля библиотеки химических элементов

        static public void GetPosition(object sender, MouseButtonEventArgs mouseButtonEvent)
        {
            Image _object = sender as Image;
            offset = mouseButtonEvent.GetPosition(_object);
            Console.WriteLine($"x: {offset.X}; y: {offset.Y}");
        }
        private void SetData(StackPanel panel, ObservableCollection<ChemicalElement> collection)
        {
            int column = 0;
            if (collection.Count != 0)
            {
                foreach (ChemicalElement element in collection)
                {
                    Image copy = new Image()
                    {
                        Source = new BitmapImage(new Uri($"pack://application:,,,/{element.Image_path}", UriKind.Absolute)),
                        Width = 100 * 1.5,
                        Height = 100 * 1.5
                    };
                    Console.WriteLine($"Элемент добавленю Путь: {copy.Source}");
                    copy.MouseDown += Object_MouseDown;
                    copy.MouseDown += GetPosition;
                    panel.Children.Add(copy);
                    column++;
                }
            }

            else { Console.WriteLine("Коллекция элементов пуста"); }

        }

        public void Objects_Creator(StackPanel MainGrid)
        {
            int column = 0;

            Image chemical_object = new Image();

            chemical_object.Width = 100;
            chemical_object.Height = 100;
            chemical_object.Margin = new Thickness(20, 10, 50, 35);

            // Устанавливаем цвет фона в зависимости от имени
            /*if (name == "Blue") { chemical_object.Background = new SolidColorBrush(Colors.Blue); }
            if (name == "Yellow") { chemical_object.Background = new SolidColorBrush(Colors.Yellow); }
            if (name == "Green") { chemical_object.Background = new SolidColorBrush(Colors.Green); }
            if (name == "Purple") { chemical_object.Background = new SolidColorBrush(Colors.Purple); }
            if(name == "Red") { chemical_object.Background = new SolidColorBrush(Colors.Red); }*/

            //chemical_object.Source = new BitmapImage(new Uri("C:/Users/User/Desktop/rock.jpg", UriKind.RelativeOrAbsolute));

            //// Добавляем обработчик события
            //chemical_object.MouseDown += Object_MouseDown;

            //// Устанавливаем позицию в Grid
            //Grid.SetRow(chemical_object, 0);
            //Grid.SetColumn(chemical_object, column);

            //// Добавляем элемент в Grid
            //MainGrid.Children.Add(chemical_object);

            //// Увеличиваем номер столбца
            //column++;
            //}
        }

        // Метод создания объекта Label в Canvas
        private void Object_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image object_ = new Image();
            selectedImage_ToMove = sender as Image;

            object_.Width = selectedImage_ToMove.Width;
            object_.Height = selectedImage_ToMove.Height;

            object_.Source = selectedImage_ToMove.Source;
            object_.MouseMove += Object_MouseMove;
            object_.MouseDown += GetPosition;

            Point point = e.GetPosition(TargetCanvas);

            Canvas.SetLeft(object_, point.X - offset.X);
            Canvas.SetTop(object_, point.Y - offset.Y);

            object_.Name = selectedImage_ToMove.Name;
            TargetCanvas.Children.Add(object_);
        }

        // метод возврата на предыдущуюю странцу
        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainWindowPage());
        }

        // Метод инициализации перетаскивания объекта 
        public void Object_MouseMove(object sender, MouseEventArgs e)
        {
            selectedImage = sender as Image;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(selectedImage, selectedImage, DragDropEffects.Move);
            }
        }

        // Метод для установления новой позиции объекта при отпускании
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(TargetCanvas);

            Canvas.SetLeft(selectedImage, dropPosition.X - offset.X);
            Canvas.SetTop(selectedImage, dropPosition.Y - offset.Y);
            e.Handled = true;
            CheckCollision(selectedImage, dropPosition);
        }

        // Метод для перемещения объекта в след за курсором
        private void Canvas_Drag(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(TargetCanvas);

            Canvas.SetLeft(selectedImage, dropPosition.X - offset.X);
            Canvas.SetTop(selectedImage, dropPosition.Y - offset.Y);
            e.Handled = true;
        }

        private void GetObjName(StackPanel panel)
        {
            foreach (ChemicalElement element in ElementsLibrary.data)
            {
                //Console.WriteLine(element.Name);
                foreach (Image image in panel.Children)
                {
                    Console.WriteLine($"Image.source: {image.Source}");
                    Console.WriteLine($"BitmapImage: {new BitmapImage(new Uri($"pack://application:,,,/{element.Image_path}", UriKind.Absolute))}");
                    if (image.Source.ToString() == new BitmapImage(new Uri($"pack://application:,,,/{element.Image_path}", UriKind.Absolute)).ToString())
                    {
                        Console.WriteLine("Присвоенно имя");
                        image.Name = element.Name;
                    }
                }
            }
        }

        // Метод для проеверки коллизии ( Deep Seek )
        private void CheckCollision(Image movingImage, Point dropPosition)
        {
            var elementsToRemove = new List<UIElement>();

            string obj1_Name = " ";
            string obj2_Name = " ";

            // Получаем границы перемещаемого объекта
            Rect movingRect = new Rect(
                Canvas.GetLeft(movingImage),
                Canvas.GetTop(movingImage),
                movingImage.Width,
                movingImage.Height
            );

            // Проверяем коллизию с каждым объектом на Canvas
            foreach (var child in TargetCanvas.Children)
            {
                Console.WriteLine(((Image)child).Name);
                Console.WriteLine(movingImage.Name);
                if (child is Image image && image.Name != movingImage.Name)
                {
                    Console.WriteLine("Проверка условия на коллизию");
                    // Получаем границы второго объекта
                    Rect targetRect = new Rect(
                        Canvas.GetLeft(image),
                        Canvas.GetTop(image),
                        image.Width,
                        image.Height
                    );

                    // Проверяем пересечение
                    if (movingRect.IntersectsWith(targetRect))
                    {
                        Console.WriteLine("Коллизия");
                        elementsToRemove.Add(image);
                        elementsToRemove.Add(movingImage);
                        obj1_Name = image.Name;
                        obj2_Name = movingImage.Name;
                    }
                }
            }


            // Условия для химических реакций

            if ((obj1_Name == "Натрий" && obj2_Name == "Вода") || (obj1_Name == "Вода" && obj2_Name == "Натрий"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/гидроксид_натрия.png");
                Create_Object(dropPosition, "pictures/водород.png");
            }
            if ((obj1_Name == "Калий" && obj2_Name == "Вода") || (obj1_Name == "Вода" && obj2_Name == "Калий"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/гидроксид_калия.png");
                Create_Object(dropPosition, "pictures/водород.png");
            }
            if ((obj1_Name == "Алюминий" && obj2_Name == "Кислород") || (obj1_Name == "Кислород" && obj2_Name == "Алюминий"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/оксид_алюминия.png");
            }
            if ((obj1_Name == "Кальций" && obj2_Name == "Кислород") || (obj1_Name == "Кислород" && obj2_Name == "Кальций"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/оксид_кальция.png");
            }
            if ((obj1_Name == "Магний" && obj2_Name == "Кислород") || (obj1_Name == "Кислород" && obj2_Name == "Магний"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/оксид_магния.png");
            }
            if ((obj1_Name == "Медь" && obj2_Name == "Кислород") || (obj1_Name == "Кислород" && obj2_Name == "Медь"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/оксид_меди.png");
            }
            if ((obj1_Name == "Цинк" && obj2_Name == "Кислород") || (obj1_Name == "Кислород" && obj2_Name == "Цинк"))
            {
                foreach (var element in elementsToRemove)
                {
                    TargetCanvas.Children.Remove(element);
                }
                Create_Object(dropPosition, "pictures/оксид_цинка.png");
            }
        }


        // Методы для создания новых объектов при колизии
        private void Create_Object(Point dropPosition, string path) 
        {
            Image _object = new Image();

            _object.Width = 160;
            _object.Height = 160;

            _object.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            _object.MouseMove += Object_MouseMove;
            _object.MouseDown += GetPosition;

            Canvas.SetLeft(_object, dropPosition.X);
            Canvas.SetTop(_object, dropPosition.Y);

            _object.Name = "Yellow";
            TargetCanvas.Children.Add(_object);
        }

    }

}
