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
    /// Interaction logic for DevicesWindow.xaml
    /// </summary>
    public partial class DevicesWindow : Window
    {

        private readonly DatabaseService _databaseService;

        public ObservableCollection<Devices> Devices_List { get; set; }
        public ObservableCollection<Locations> Locations_List { get; set; }


        public DevicesWindow(DatabaseService databaseService)
        {

            InitializeComponent();
            _databaseService = databaseService;
            Devices_List = new ObservableCollection<Devices>();
            Locations_List = new ObservableCollection<Locations>();
            LoadLocations();
            LoadDevices();
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


        private void DevicesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecteddevice = DeviceDataGrid.SelectedItem as Devices;
            if (selecteddevice != null && selecteddevice.IsDeleted)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    connection.Delete(selecteddevice);
                    Devices_List.Remove(selecteddevice);
                }
            }
        }




        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DevicesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {

                    var device = e.Row.Item as Devices;
                    if (device != null)
                    {
                        using (var connection = _databaseService.GetConnection())
                        {
                            if (device.Id == 0) // New Sybol
                            {
                                connection.Insert(device);
                                device.Id = connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
                            }
                            else // Existing symbol
                            {
                                connection.Update(device);
                            }
                        }

                        // Ensure the newly added or edited item is selected and visible
                        DeviceDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        DeviceDataGrid.ItemsSource = null; // Workaround to refresh the DataGrid
                        DeviceDataGrid.ItemsSource = Devices_List;

                        // Get the index of the newly added or edited person
                        var index = Devices_List.IndexOf(device);

                        // Select the newly added or edited person
                        DeviceDataGrid.SelectedIndex = index;

                        // Scroll to the newly added or edited person
                        DeviceDataGrid.ScrollIntoView(device);

                        DeviceDataGrid.UpdateLayout();
                        DataGridRow row = (DataGridRow)DeviceDataGrid.ItemContainerGenerator.ContainerFromIndex(index);
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
            var selecteddevice = DeviceDataGrid.SelectedItem as Devices;
            if (selecteddevice != null)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    // Mark as deleted in the database
                    selecteddevice.IsDeleted = true;
                    connection.Update(selecteddevice);
                }
                // Remove from the ObservableCollection to update the UI
                Devices_List.Remove(selecteddevice);
            }

        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid? DeviceDataGrid = sender as DataGrid;
            ContextMenu contextMenu = DeviceDataGrid.ContextMenu;
            contextMenu.IsOpen = true;
        }






    }
}
