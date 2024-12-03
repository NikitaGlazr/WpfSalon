using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private bool isAdminMode = false;

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
            using (var context = Helper.GetContext())
            {
                serviceVisits = context.Service.ToList();
                foreach (var service in serviceVisits)
                {
                  //  service.MainImagePath = @"C:\Users\nikit\OneDrive\Рабочий стол\рабочий стол№2\колледж\ДЕМКА\Salon\WpfSalon\WpfSalon\Услуги салона красоты\" + service.Title + ".png";
                    Console.WriteLine($"Service: {service.Title}, Discount: {service.Discount}, ImagePath: {service.MainImagePath}");
                }
            }
        }


        internal void Load()
        {
            try
            {
                // Применение фильтров и сортировки
                var filteredServices = FilterServices();
                var sortedServices = SortServices(filteredServices);

                // Пагинация
                var pagedServices = sortedServices.Skip(start * recordsPersonalPage).Take(recordsPersonalPage).ToList();

                // Обновление DataGrid
                agentGrid.ItemsSource = pagedServices;

                // Обновление информации о количестве записей
                full.Text = $"{pagedServices.Count} из {sortedServices.Count}";

                // Пагинация, брать сумму всех сервисов и поделить на кнопки.
                int totalPages = (int)Math.Ceiling((double)sortedServices.Count / recordsPersonalPage);
                pagin.Children.Clear();

                for (int i = 0; i < totalPages; i++)
                {
                    Button myButton = new Button();
                    myButton.Height = 30;
                    myButton.Content = i + 1;
                    myButton.Width = 20;
                    myButton.HorizontalAlignment = HorizontalAlignment.Center;
                    myButton.Tag = i;
                    myButton.Click += new RoutedEventHandler(paginButto_Click);
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
            }
        }

        private void turnButton()
        {
            if (start == 0)
            {
                back.IsEnabled = false;
            }
            else
            {
                back.IsEnabled = true;
            }

            if ((start + 1) * recordsPersonalPage >= fullCount)
            {
                forward.IsEnabled = false;
            }
            else
            {
                forward.IsEnabled = true;
            }
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

        private void addEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdminMode)
            {
                // Логика добавления новой услуги
            }
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load();
        }

        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Логика обработки клика по строке
        }

        private void FilterServicesByDiscount(int minDiscount, int maxDiscount)
        {
            // Логика фильтрации по скидке
        }

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Логика загрузки строки
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            Load();
        }

        private void agentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика обработки выбора строки
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminCodeTextBox.Text == "0000")
            {
                isAdminMode = true;
                addButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
                addButton.IsEnabled = true;
                EditButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Неверный код администратора");
                addButton.IsEnabled = false;
                EditButton.IsEnabled = false;
                addButton.Visibility = Visibility.Hidden;
                EditButton.Visibility = Visibility.Hidden;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdminMode)
            {
                // Логика редактирования услуги
            }
        }


        private List<Service> FilterServices()
        {
            var filteredServices = serviceVisits.AsQueryable();

            // Фильтрация по тексту
            if (!string.IsNullOrEmpty(fnd))
            {
                filteredServices = filteredServices.Where(s => s.Title.Contains(fnd) || s.Description.Contains(fnd));
            }

            // Фильтрация по скидке
            var selectedDiscount = SortDiscount.SelectedItem as ComboBoxItem;
            if (selectedDiscount != null)
            {
                var discountRange = selectedDiscount.Content.ToString();
                if (discountRange != "Все")
                {
                    var ranges = discountRange.Split(new[] { "от", "до" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
                    if (ranges.Length == 2)
                    {
                        var minDiscount = int.Parse(ranges[0].Replace("%", ""));
                        var maxDiscount = int.Parse(ranges[1].Replace("%", ""));
                        filteredServices = filteredServices.Where(s => s.Discount >= minDiscount && s.Discount < maxDiscount);
                    }
                    else
                    {
                        Console.WriteLine("Invalid discount range format");
                    }
                }
            }

            Console.WriteLine($"Filtered Services Count: {filteredServices.Count()}");
            return filteredServices.ToList();
        }



        private List<Service> SortServices(List<Service> services)
        {
            var selectedSort = Sort.SelectedItem as ComboBoxItem;
            if (selectedSort != null)
            {
                var sortTag = int.Parse(selectedSort.Tag.ToString());
                switch (sortTag)
                {
                    case 1:
                        services = services.OrderBy(s => s.Cost).ToList();
                        break;
                    case 2:
                        services = services.OrderByDescending(s => s.Cost).ToList();
                        break;
                    case 3:
                        services = services.OrderBy(s => s.Discount).ToList();
                        break;
                    case 4:
                        services = services.OrderByDescending(s => s.Discount).ToList();
                        break;
                }
            }

            Console.WriteLine($"Sorted Services Count: {services.Count}");
            return services;
        }

        private void paginButto_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                start = (int)button.Tag;
                Load();
            }
        }

        private void SortDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load();
        }
    }
}