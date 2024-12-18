using Microsoft.Win32;
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

namespace WpfSalon.Pages
{

    /// <summary>
    /// Логика взаимодействия для PageAddEditClient.xaml
    /// </summary>
    /// 
    public partial class PageAddEditClient : Page
    {
        private Service serv;

        public PageAddEditClient(Service service)
        {
            InitializeComponent();
            serv = service;

            if (service != null)
            {
                LoadServiceData();
            }
        }

        private void LoadServiceData()
        {
            TitleTextBox.Text = serv.Title;
            CostTextBox.Text = serv.Cost.ToString();
            DurationTextBox.Text = serv.DurationInMinutes.ToString();
            DescriptionTextBox.Text = serv.Description;
            DiscountTextBox.Text = serv.Discount.ToString();
          

            // Загружаем главное изображение
            MainImage.Source = new BitmapImage(new Uri(serv.MainImagePath, UriKind.RelativeOrAbsolute));
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления новой услуги
            Service newService = new Service
            {
                Title = TitleTextBox.Text,
                Cost = decimal.Parse(CostTextBox.Text),
                DurationInMinutes = int.Parse(DurationTextBox.Text),
                Description = DescriptionTextBox.Text,
                Discount = float.Parse(DiscountTextBox.Text),
              
            };

            Helper.GetContext().Service.Add(newService);
            Helper.GetContext().SaveChanges();
            MessageBox.Show("Услуга успешно добавлена!", "Успех", MessageBoxButton.OK);
        }

        private void SaveServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (serv == null) return;

            // Логика для сохранения изменений существующей услуги
            serv.Title = TitleTextBox.Text;
            serv.Cost = decimal.Parse(CostTextBox.Text);
            serv.DurationInMinutes = int.Parse(DurationTextBox.Text);
            serv.Description = DescriptionTextBox.Text;
            serv.Discount = float.Parse(DiscountTextBox.Text);
           

            Helper.GetContext().SaveChanges();
            MessageBox.Show("Услуга успешно сохранена!", "Успех", MessageBoxButton.OK);
        }

        private void DeleteServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (serv == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить эту услугу?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Helper.GetContext().Service.Remove(serv);
                Helper.GetContext().SaveChanges();
                MessageBox.Show("Услуга успешно удалена!", "Успех", MessageBoxButton.OK);
                // Вернуться на предыдущую страницу
                NavigationService.GoBack();
            }
        }

        private void ImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {

                //MainImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
