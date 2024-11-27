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

        public PageListClient(Frame frame)
        {
            InitializeComponent();
            frm = new Frame();

            loadListService();
            Load();
        }

        public void loadListService()
        {
            List<Service> service = new List<Service> { };
            service = Helper.GetContext().Service.ToList();
            service.Add(new Service { Title = "Все сервисы" });
            //Type.ItemsSource = service.OrderBy(AgentType => AgentType.ID);
        }


        internal void Load()
        {
            try
            {
                List<Service> service = new List<Service>();
                var ag = Helper.GetContext().Service.Where(Service => Service.Title.Contains(fnd) || Service.Description.Contains(fnd));
                if (order > 0) ag = Helper.GetContext().Service.Where(Service => (Service.Title.Contains(fnd) || Service.Description.Contains(fnd) && (Service.Discount == order)));
                service.Clear();
                foreach (Service servic in ag)
                {
                    if (servic.MainImagePath == "")
                    {
                        servic.MainImagePath = "/images/picture.png";
                    }
                    int sum = 0;
                    double fsum = 0;

                    //подсчет 

                    // 0-5
                    //5-15
                    //15-30
                    //30-70
                    //70-100

                }

                full.Text = fullCount.ToString();

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


        private void addEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (ascending)
            {
                serviceVisits = serviceVisits.OrderBy(s => s.Cost).ToList();
            }
            else
            {
                serviceVisits = serviceVisits.OrderByDescending(s => s.Cost).ToList();
            }
            Load(); 
            */
        }

        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void FilterServicesByDiscount(int minDiscount, int maxDiscount)
        {
            serviceVisits = Helper.GetContext().Service
                                 .Where(s => s.Discount >= minDiscount && s.Discount < maxDiscount)
                                 .ToList();
            Load();
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

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void agentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
