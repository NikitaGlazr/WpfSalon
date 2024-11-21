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
        private List<Client> clientVisits = new List<Client>();


        public PageListClient(Frame frame)
        {
            InitializeComponent();
            frm = new Frame();
            List<Gender> gender = new List<Gender> { };
            gender = Helper.GetContext().Gender.ToList();
            gender.Add(new WpfSalon.Gender { Name = "Все полы"});
            Gender.ItemsSource = gender.OrderBy(Gender => Gender.Code);

            Load();
        }

        internal void Load()
        {
            try 
            {
                List<Client> clients = new List<Client>();
                var cl = Helper.GetContext().Client.Where(Client => Client.LastName.Contains(fnd) ||
                                                                    Client.FirstName.Contains(fnd) ||
                                                                    Client.Patronymic.Contains(fnd) ||
                                                                    Client.Phone.Contains(fnd) ||
                                                                    Client.Email.Contains(fnd));

                clients.Clear();
                clientVisits.Clear();
                
                foreach ( Client client in cl )
                {
                   // var lastVisit = Helper.GetContext().ClientService.Where(c);

                    clientVisits.Add(client);//Добавление клиентов в список 
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;

            }
        }

        private void clientsDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void RecordsPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FIO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
