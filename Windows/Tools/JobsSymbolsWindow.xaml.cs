using Jobs_Planner.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Jobs_Planner.Windows.Tools
{
    /// <summary>
    /// Interaction logic for JobsSymbolsWindow.xaml
    /// </summary>
    public partial class JobsSymbolsWindow : Window
    {

        private readonly DatabaseService _databaseService;

        public ObservableCollection<JobsSymbols> JobsSymbols_List { get; set; }


        public JobsSymbolsWindow(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            JobsSymbols_List = new ObservableCollection<JobsSymbols>();
            LoadJobsSymbols();
            this.DataContext = this;
        }

        private void LoadJobsSymbols()
        {
            try 
            {
                using (var connection = _databaseService.GetConnection())
                {
                    var _symbols = connection.Table<JobsSymbols>().Where(p => !p.IsDeleted).ToList();
                    JobsSymbols_List.Clear();
                    foreach (var symbol in _symbols)
                    {
                        JobsSymbols_List.Add(symbol);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void JobsSymbolsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedjob = JobsDataGrid.SelectedItem as JobsSymbols;
            if (selectedjob != null && selectedjob.IsDeleted)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    connection.Delete(selectedjob);
                    JobsSymbols_List.Remove(selectedjob);
                }
            }
        }




        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void JobsSymbolsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {

                    var symbol = e.Row.Item as JobsSymbols;
                    if (symbol != null)
                    {
                        using (var connection = _databaseService.GetConnection())
                        {
                            if (symbol.Id == 0) // New Sybol
                            {
                                connection.Insert(symbol);
                                symbol.Id = connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
                            }
                            else // Existing symbol
                            {
                                connection.Update(symbol);
                            }
                        }

                        // Ensure the newly added or edited item is selected and visible
                        JobsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        JobsDataGrid.ItemsSource = null; // Workaround to refresh the DataGrid
                        JobsDataGrid.ItemsSource = JobsSymbols_List;

                        // Get the index of the newly added or edited person
                        var index = JobsSymbols_List.IndexOf(symbol);

                        // Select the newly added or edited person
                        JobsDataGrid.SelectedIndex = index;

                        // Scroll to the newly added or edited person
                        JobsDataGrid.ScrollIntoView(symbol);

                        JobsDataGrid.UpdateLayout();
                        DataGridRow row = (DataGridRow)JobsDataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                        if (row != null)
                        {
                            row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }



                    }

                }), DispatcherPriority.Background);
            }
        }

        private void DeleteSelectedRow_Click(object sender, RoutedEventArgs e)
        {
            var selectedjob = JobsDataGrid.SelectedItem as JobsSymbols;
            if (selectedjob != null)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    // Mark as deleted in the database
                    selectedjob.IsDeleted = true;
                    connection.Update(selectedjob);
                }
                // Remove from the ObservableCollection to update the UI
                JobsSymbols_List.Remove(selectedjob);
            }

        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid? JobsdataGrid = sender as DataGrid;
            ContextMenu contextMenu = JobsdataGrid.ContextMenu;
            contextMenu.IsOpen = true;
        }

    }
}
