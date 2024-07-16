using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jobs_Planner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DateTextBlock.Text ="Šiandienos diena: " + DateTime.Now.ToString("yyyy-MM-dd");

        }
        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

            //Close();
        }
        private void Configuration_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}