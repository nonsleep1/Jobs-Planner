using Jobs_Planner.Windows.Tools;
using System.Configuration;
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
using System.IO;

namespace Jobs_Planner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseService;

        public MainWindow()
        {
            InitializeComponent();

            DateTextBlock.Text ="Šiandienos diena: " + DateTime.Now.ToString("yyyy-MM-dd");
            var dbPath = ConfigurationManager.AppSettings["DatabasePath"];
            dbPath = Environment.ExpandEnvironmentVariables(dbPath);

            if (!File.Exists(dbPath))
            {
                MessageBox.Show("Database not found. Please select a database file.");
                
            }
            else
            {
                _databaseService = new DatabaseService(dbPath);
            }

        }

        //private void InitializeDatabaseService(string dbPath)
        //{
            
        //}

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

            
        }
        private void Configuration_Click(object sender, RoutedEventArgs e)
        {
            //
            if (_databaseService != null)
            {
                var workilistwindow = new Jobs_Planner.Windows.Tools.Configuration(_databaseService);
                workilistwindow.Show();
            }
            else
            {
                MessageBox.Show("Database service is not initialized.");
            }
        }
        private void WorkersList_Click(object sender, RoutedEventArgs e)
        {
            //
            if(_databaseService != null)
            {
                var workilistwindow = new WorkersList(_databaseService);
                workilistwindow.Show();
            }
            else
            {
                MessageBox.Show("Database service is not initialized.");
            }

            
            
        }
        private void JobsSymbols_Click(object sender, RoutedEventArgs e)
        {
            if (_databaseService != null)
            {
                var jobssymbolswindow = new JobsSymbolsWindow(_databaseService);
                jobssymbolswindow.Show();
            }
            else
            {
                MessageBox.Show("Database service is not initialized.");
            }
        }
    }
}