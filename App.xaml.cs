using System.Configuration;
using System.Data;
using System.Windows;

namespace Jobs_Planner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show the splash screen
           

            // Load the main window asynchronously
            Task.Run(() =>
            {
                // Simulate some work (e.g., loading resources)
                System.Threading.Thread.Sleep(3000);

                // Load the main window
                this.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    
                });
            });
        }
    }

}
