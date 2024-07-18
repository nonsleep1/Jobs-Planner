using System.Configuration;
using System.Data;
using System.Windows;

using System.IO;

namespace Jobs_Planner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private DatabaseService? _databaseService;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                // Show the splash screen


                // Load the main window asynchronously
                Task.Run(() =>
                {


                    var dbPath = ConfigurationManager.AppSettings["DatabasePath"];
                    dbPath = Environment.ExpandEnvironmentVariables(dbPath);
                    var dbFolder = Path.GetDirectoryName(dbPath);

                    // Ensure the database folder exists
                    if (!Directory.Exists(dbFolder))
                    {
                        Directory.CreateDirectory(dbFolder);
                    }
                    _databaseService = new DatabaseService(dbPath);

                    if (!File.Exists(dbPath))
                    {
                        MessageBoxResult result = MessageBox.Show("Nerasta duomenų bazė, nerasta database.db", "Ar sukurti database.db", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            //sukurti baze
                            _databaseService.Createdatabase(dbPath);
                            MessageBox.Show("Duomenų bazė buvo sukurta, paleiskite programa iš naujo");
                            Environment.Exit(0);

                        }
                        else if (result == MessageBoxResult.No)
                        {
                            MessageBox.Show("Duomenų bazė privalo egzistuoti");
                            
                            Environment.Exit(0);
                        }

                    }
                    else
                    {
                        // Simulate some work (e.g., loading resources)
                        System.Threading.Thread.Sleep(3000);

                        // Load the main window
                        this.Dispatcher.Invoke(() =>
                        {
                            var mainWindow = new MainWindow();
                            mainWindow.Show();

                        });
                    }


                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }   
    }

}

