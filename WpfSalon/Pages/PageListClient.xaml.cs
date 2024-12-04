using DocumentFormat.OpenXml.Math;
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
using System.Data.Entity;
using System.Globalization;

namespace WpfSalon.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListClient.xaml
    /// </summary>
    public partial class PageListClient : Page
    {
        public int start = 0;
        public int fullCount = 0;
        public int order = 0;
        private string fnd = "";
        private string gnd = "";
        private Frame frm;
        private int recordsPersonalPage = 10;
        private int totalRecords = 0;
        private List<Service> serviceVisits = new List<Service>();
        private bool ascending;
        private bool isAdmin = false; // Новая переменная для хранения состояния администратора


        public PageListClient(Frame frame)
        {
            InitializeComponent();
            frm = new Frame();

            loadListService();
            Load();

           
            addButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            addButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;
        }

        public void loadListService()
        {
            List<Service> service = new List<Service> { };
            service = Helper.GetContext().Service.ToList();
         
        }


        internal void Load()
        {
            try
            {
                List<Service> service = new List<Service>();
                var ag = Helper.GetContext().Service.Where(Service => Service.Title.Contains(fnd) || Service.Description.Contains(fnd)).ToList();
                if (isAdmin)
                {
                    service = ag;
                }
                else
                {
                    service = ag;
                }

                fullCount = service.Count; // Обновление общего количества
                full.Text = fullCount.ToString();
                agentGrid.ItemsSource = service.Skip(start * recordsPersonalPage).Take(recordsPersonalPage).ToList(); // Установите источник данных для agentGrid


                // Пагинация
                int ost = fullCount % 10;
                int pag = (fullCount - ost) / 10;
                if (ost > 0) pag++;
                pagin.Children.Clear();

                for (int i = 0; i < pag; i++)
                {
                    Button myButton = new Button();
                    myButton.Height = 30;
                    myButton.Content = i + 1;
                    myButton.Width = 20;
                    myButton.HorizontalAlignment = HorizontalAlignment.Center;
                    myButton.Tag = i;
                    //myButton.Click += new RoutedEventHandler(paginButto_Click);
                    pagin.Children.Add(myButton);
                }

                // Вызов метода HighlightCurrentPage для подсветки страницы
                HighlightCurrentPage();
                turnButton();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            };
        }

        //сортировка и фильтраци, поиск----------------------------------------------------------------------------

        public void FilterServicesByDiscount(int minDiscount, int maxDiscount)
        {
            serviceVisits = Helper.GetContext().Service
                                 .Where(s => s.Discount >= minDiscount && s.Discount < maxDiscount)
                                 .ToList();
            Load();
        }


        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SortDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //сортировка и фильтраци, поиск----------------------------------------------------------------------------


        //Кнокпи
        private void addEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                // Логика для редактирования услуги
            }
            else
            {
                MessageBox.Show("Только администратор может редактировать услуги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                // Логика для добавления услуги
            }
            else
            {
                MessageBox.Show("Только администратор может добавлять услуги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
        //Кнокпи



        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }


        private void agentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {  // Проверка кода для входа в режим администратора

            if (AdminCodeTextBox.Text == "0000")
            {
                MessageBox.Show("Режим администратора активирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                isAdmin = true; // Устанавливаем флаг на true

                UpdateAdminUI();
            }
            else
            {
                MessageBox.Show("Неверный код!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                addButton.Visibility = Visibility.Hidden;
                EditButton.Visibility = Visibility.Hidden;
                addButton.IsEnabled = false;
                EditButton.IsEnabled = false;
            }
        }


        private void UpdateAdminUI()
        {
            addButton.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Visible;


            addButton.IsEnabled = true;
            EditButton.IsEnabled = true;
        }


        private void turnButton()
        {
            if (start == 0) { back.IsEnabled = false; }
            else { back.IsEnabled = true; };
            if ((start + 1) * 10 > fullCount) { forward.IsEnabled = false; }
            else { forward.IsEnabled = true; };
        }
        private void forward_Click(object sender, RoutedEventArgs e)
        {
            start++;
            Load();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            start--;
            Load();
        }


        //Изменение цвета страниц на которой находится пользователь, для удобного понимания.
        private void HighlightCurrentPage()
        {
            foreach (var child in pagin.Children)
            {
                if (child is Button myButton)
                {
                    if ((int)myButton.Tag == start)
                    {
                        myButton.Background = new SolidColorBrush(Colors.Blue);
                        myButton.Foreground = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        myButton.Background = new SolidColorBrush(Colors.Transparent);
                        myButton.Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }


        public class DiscountVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value != null && (double)value > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


    }
}
