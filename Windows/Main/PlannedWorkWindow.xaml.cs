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

namespace Jobs_Planner.Windows.Main
{
    /// <summary>
    /// Interaction logic for PlannedWorkWindow.xaml
    /// </summary>
    public partial class PlannedWorkWindow : Window
    {

        private readonly DatabaseService _databaseService;

        public ObservableCollection<Devices> Devices_List { get; set; }
        public ObservableCollection<Locations> Locations_List { get; set; }
        public ObservableCollection<PlannedWork> PlannedWork_List { get; set; }


        public PlannedWorkWindow(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            Devices_List = new ObservableCollection<Devices>();
            Locations_List = new ObservableCollection<Locations>();
            PlannedWork_List = new ObservableCollection<PlannedWork>();
            LoadDevices();
            LoadLocations();
            LoadPlannedWorks();

            this.DataContext = this;
        }

        private void LoadDevices()
        {
            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    var _devices = connection.Table<Devices>().Where(p => !p.IsDeleted).ToList();
                    Devices_List.Clear();
                    foreach (var device in _devices)
                    {
                        Devices_List.Add(device);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadLocations()
        {
            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    var _locations = connection.Table<Locations>().Where(p => !p.IsDeleted).ToList();
                    Locations_List.Clear();
                    foreach (var _location in _locations)
                    {
                        Locations_List.Add(_location);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadPlannedWorks()
        {
            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    var _plannedwork = connection.Table<PlannedWork>().Where(p => !p.IsDeleted).ToList();
                    PlannedWork_List.Clear();
                    foreach (var _planWork in _plannedwork)
                    {
                        PlannedWork_List.Add(_planWork);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void PlannedWorkDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedwork = PlannedWorkDataGrid.SelectedItem as PlannedWork;
            if (selectedwork != null && selectedwork.IsDeleted)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    connection.Delete(selectedwork);
                    PlannedWork_List.Remove(selectedwork);
                }
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PlannedWorkDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {

                    var planedwork = e.Row.Item as PlannedWork;
                    if (planedwork != null)
                    {
                        using (var connection = _databaseService.GetConnection())
                        {
                            if (planedwork.Id == 0) // New device
                            {
                                connection.Insert(planedwork);
                                planedwork.Id = connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
                            }
                            else // Existing device
                            {
                                connection.Update(planedwork);
                            }
                        }

                        // Ensure the newly added or edited item is selected and visible
                        PlannedWorkDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        PlannedWorkDataGrid.ItemsSource = null; // Workaround to refresh the DataGrid
                        PlannedWorkDataGrid.ItemsSource = PlannedWork_List;

                        // Get the index of the newly added or edited person
                        var index = PlannedWork_List.IndexOf(planedwork);

                        // Select the newly added or edited person
                        PlannedWorkDataGrid.SelectedIndex = index;

                        // Scroll to the newly added or edited person
                        PlannedWorkDataGrid.ScrollIntoView(planedwork);

                        PlannedWorkDataGrid.UpdateLayout();
                        DataGridRow row = (DataGridRow)PlannedWorkDataGrid.ItemContainerGenerator.ContainerFromIndex(index);
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
            var selectedwork = PlannedWorkDataGrid.SelectedItem as PlannedWork;
            if (selectedwork != null)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    // Mark as deleted in the database
                    selectedwork.IsDeleted = true;
                    connection.Update(selectedwork);
                }
                // Remove from the ObservableCollection to update the UI
                PlannedWork_List.Remove(selectedwork);
            }

        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid? PlannedWorkDataGrid = sender as DataGrid;
            ContextMenu contextMenu = PlannedWorkDataGrid.ContextMenu;
            contextMenu.IsOpen = true;
        }








    }
}
