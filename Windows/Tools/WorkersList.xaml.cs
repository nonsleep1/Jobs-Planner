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
    /// Interaction logic for WorkersList.xaml
    /// </summary>
    public partial class WorkersList : Window
    {

        private readonly DatabaseService _databaseService;

        public ObservableCollection<Workers> Workers_List { get; set; }



        public WorkersList(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            Workers_List = new ObservableCollection<Workers>();
            LoadWorkers();
            this.DataContext = this;
        }
        private void LoadWorkers()
        {
            try 
            {
                using (var connection = _databaseService.GetConnection())
                {
                    var _workers = connection.Table<Workers>().Where(p => !p.IsDeleted).ToList();
                    Workers_List.Clear();
                    foreach (var worker in _workers)
                    {
                        Workers_List.Add(worker);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void WorkersDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {

                    var worker = e.Row.Item as Workers;
                    if (worker != null)
                    {
                        using (var connection = _databaseService.GetConnection())
                        {
                            if (worker.Id == 0) // New Worker
                            {
                                connection.Insert(worker);
                                worker.Id = connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
                            }
                            else // Existing person
                            {
                                connection.Update(worker);
                            }
                        }
                        
                        // Ensure the newly added or edited item is selected and visible
                        dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        dataGrid.ItemsSource = null; // Workaround to refresh the DataGrid
                        dataGrid.ItemsSource = Workers_List;

                        // Get the index of the newly added or edited person
                        var index = Workers_List.IndexOf(worker);

                        // Select the newly added or edited person
                        dataGrid.SelectedIndex = index;

                        // Scroll to the newly added or edited person
                        dataGrid.ScrollIntoView(worker);

                        dataGrid.UpdateLayout();
                        DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                        if (row != null)
                        {
                            row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }



                    }

                }), DispatcherPriority.Background);
            }
        }

        private void WorkersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedWorker = dataGrid.SelectedItem as Workers;
            if (selectedWorker != null && selectedWorker.IsDeleted)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    connection.Delete(selectedWorker);
                    Workers_List.Remove(selectedWorker);
                }
            }
        }




        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteSelectedRow_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorker = dataGrid.SelectedItem as Workers;
            if (selectedWorker != null)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    // Mark as deleted in the database
                    selectedWorker.IsDeleted = true;
                    connection.Update(selectedWorker);
                }
                // Remove from the ObservableCollection to update the UI
                Workers_List.Remove(selectedWorker);
            }

        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            ContextMenu contextMenu = dataGrid.ContextMenu;
            contextMenu.IsOpen = true;
        }

    }
}
