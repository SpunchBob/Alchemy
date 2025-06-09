using System;
using System.Collections.Generic;
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
        
        private Image selectedImage;
        private Image selectedImage_ToMove;
        public LaboratoryPage()
        {
            InitializeComponent();
            Objects_Creator(MainPanelGrid);
        }

        // Массив, полученный из модуля библиотеки химических элементов
        
        string[] names = { "Blue", "Yellow", "Green", "Purple", "Red"};

        public void Objects_Creator(Grid MainGrid)
        {
            int column = 0;
            // Добавляем строку и столбцы в Grids
            for (int i = 0; i < names.Length; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            foreach (string name in names)
            {
                Image chemical_object = new Image();
                
                chemical_object.Width = 100;
                chemical_object.Height = 100;
                chemical_object.Name = name;
                chemical_object.Margin = new Thickness(20, 10,  50, 35);

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
            }
        }

        // Метод создания объекта Label в Canvas
        private void Object_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image object_ = new Image();
            selectedImage_ToMove = sender as Image;
            object_.Width = 100;
            object_.Height = 100;

            object_.Source = selectedImage_ToMove.Source;
            object_.MouseMove += Object_MouseMove;

            Point point = e.GetPosition(TargetCanvas);

            Canvas.SetLeft(object_, point.X);
            Canvas.SetTop(object_, point.Y);

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

            Canvas.SetLeft(selectedImage, dropPosition.X);
            Canvas.SetTop(selectedImage, dropPosition.Y);

            CheckCollision(selectedImage, dropPosition);
        }
        
        // Метод для перемещения объекта в след за курсором
        private void Canvas_Drag(object sender, DragEventArgs e) 
        {
            Point dropPosition = e.GetPosition(TargetCanvas);

            Canvas.SetLeft(selectedImage, dropPosition.X);
            Canvas.SetTop(selectedImage, dropPosition.Y);
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
                if (child is Label label && label.Name != movingImage.Name)
                {
                    // Получаем границы второго объекта
                    Rect targetRect = new Rect(
                        Canvas.GetLeft(label),
                        Canvas.GetTop(label),
                        label.Width,
                        label.Height
                    );

                    // Проверяем пересечение
                    if (movingRect.IntersectsWith(targetRect))
                    {

                        elementsToRemove.Add(label);
                        elementsToRemove.Add(movingImage);
                        obj1_Name = label.Name;
                        obj2_Name = movingImage.Name;
                    }
                }
            }
            foreach (var element in elementsToRemove) 
            {
                TargetCanvas.Children.Remove(element);
            }
            
            // Условия для химических реакций
            
            if ((obj1_Name == "Yellow" && obj2_Name == "Blue") || (obj1_Name == "Blue" && obj2_Name == "Yellow")) { CreateGreen_Object(dropPosition); }
        }

        // Методы для создания новых объектов при колизии
        private void CreateGreen_Object(Point dropPosition) 
        {
            Image _object = new Image();

            _object.Width = 100;
            _object.Height = 100;

            _object.Source = new BitmapImage(new Uri("C:/Users/User/Desktop/rock.jpg", UriKind.RelativeOrAbsolute));
            _object.MouseMove += Object_MouseMove;

            Canvas.SetLeft(_object, dropPosition.X);
            Canvas.SetTop(_object, dropPosition.Y);

            _object.Name = "Yellow";
            TargetCanvas.Children.Add(_object);
        }

    }

}
