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
       // public int order = 0;
        private int sortOrder = 0; // 0 - без сортировки, 1 - по возрастанию стоимости, 2 - по убыванию стоимости
        private string fnd = "";
        private string gnd = "";
        private Frame frm;
        private int recordsPersonalPage = 10;
        private int totalRecords = 0;
        private List<Service> serviceVisits = new List<Service>();
        private bool ascending;
        private bool isAdmin = false;
        private int selectedDiscountRange = -1; // -1 means no filter

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
                    foreach (Service servicee in ag)
                    {
                        if (servicee.MainImagePath == "")
                        {
                            servicee.MainImagePath = "/Услуги салона красоты/122454.png";
                            
                        }
                    }
                    }
                else
                {
                    service = ag;
                    foreach (Service servicee in ag)
                    {
                        if (servicee.MainImagePath == "")
                        {
                            servicee.MainImagePath = "/Услуги салона красоты/122454.png";

                        }
                    }
                }

                // Применение фильтрации по скидке
                if (selectedDiscountRange != -1)
                {
                    service = service.Where(s =>
                    {
                        switch (selectedDiscountRange)
                        {

                            case 0: return s.Discount >= 0 && s.Discount < 5;
                            case 1: return s.Discount >= 5 && s.Discount < 15;
                            case 2: return s.Discount >= 15 && s.Discount < 30;
                            case 3: return s.Discount >= 30 && s.Discount < 70;
                            case 4: return s.Discount >= 70 && s.Discount <= 100;
                            default: return true;
                        }
                    }).ToList();
                }

                // Применение сортировки
                switch (sortOrder)
                {
                    case 1:
                        service = service.OrderBy(s => s.Cost).ToList();
                        break;
                    case 2:
                        service = service.OrderByDescending(s => s.Cost).ToList();
                        break;
                    case 3:
                        service = service.OrderBy(s => s.Discount).ToList();
                        break;
                    case 4:
                        service = service.OrderByDescending(s => s.Discount).ToList();
                        break;
                }


               

                fullCount = service.Count;
                full.Text = fullCount.ToString();
                var displayedServices = service.Skip(start * recordsPersonalPage).Take(recordsPersonalPage).ToList();
                agentGrid.ItemsSource = displayedServices;

                displayedCount.Text = $"{displayedServices.Count} из {fullCount}";


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
                    myButton.Click += PageButton_Click; // Добавляем обработчик событий
                    myButton.Tag = i;
                    pagin.Children.Add(myButton);
                }

                HighlightCurrentPage();
                turnButton();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            };
        }

        public void FilterServicesByDiscount(int minDiscount, int maxDiscount)
        {
            serviceVisits = Helper.GetContext().Service
                                 .Where(s => s.Discount >= minDiscount && s.Discount < maxDiscount)
                                 .ToList();
            Load();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sort.SelectedItem is ComboBoxItem selectedItem)
            {
                if (int.TryParse(selectedItem.Tag.ToString(), out int order))
                {
                    sortOrder = order;
                    Load();
                }
            }
        }
        private void SortDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortDiscount.SelectedItem is ComboBoxItem selectedItem)
            {
                string content = selectedItem.Content.ToString();
                switch (content)
                {
                    case "Все":
                        selectedDiscountRange = -1;
                        break;
                    case "От 0 до 5%":
                        selectedDiscountRange = 0;
                        break;
                    case "От 5 до 15%":
                        selectedDiscountRange = 1;
                        break;
                    case "От 15 до 30%":
                        selectedDiscountRange = 2;
                        break;
                    case "От 30 до 70%":
                        selectedDiscountRange = 3;
                        break;
                    case "От 70 до 100%":
                        selectedDiscountRange = 4;
                        break;
                }
                Load();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = (sender as TextBox).Text;
            Load();
        }

        private void addEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                // Логика для редактирования услуги
                frm.Content = new PageAddEditClient(null);
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
                // Логика для редактирования услуги
                frm.Content = new PageAddEditClient(null);
            }
            else
            {
                MessageBox.Show("Только администратор может добавлять услуги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var service = e.Row.DataContext as Service;
            if (service != null && service.Discount > 0)
            {
                e.Row.Background = new SolidColorBrush(Colors.LightGreen);
            }
        }

        private void agentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminCodeTextBox.Text == "0000")
            {
                MessageBox.Show("Режим администратора активирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                isAdmin = true;

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


        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                start = (int)button.Tag;
                Load();
            }
        }
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
    }
}