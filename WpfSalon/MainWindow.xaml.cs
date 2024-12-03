using System.Windows;
using System.Windows.Navigation;
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
        }
    }
}
