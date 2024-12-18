using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
using WpfSalon.Pages;

namespace WpfSalon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
            frame.Content = new PageListClient(frame);
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                PageListClient pg = (PageListClient)e.Content;
                pg.Load();
            }
            catch { };

            /*
             1000-7
             993-7
             986-7

             */
        }
    }
}
