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
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow : Window
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Locations> Locations_List { get; set; }

        public LocationWindow(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            Locations_List = new ObservableCollection<Locations>();
            LoadLocations();
            this.DataContext = this;
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
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void LocationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedlocation = LocationDataGrid.SelectedItem as Locations;
            if (selectedlocation != null && selectedlocation.IsDeleted)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    connection.Delete(selectedlocation);
                    Locations_List.Remove(selectedlocation);
                }
            }
        }




        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LocationsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {

                    var location = e.Row.Item as Locations;
                    if (location != null)
                    {
                        using (var connection = _databaseService.GetConnection())
                        {
                            if (location.Id == 0) // New Sybol
                            {
                                connection.Insert(location);
                                location.Id = connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
                            }
                            else // Existing symbol
                            {
                                connection.Update(location);
                            }
                        }

                        // Ensure the newly added or edited item is selected and visible
                        LocationDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        LocationDataGrid.ItemsSource = null; // Workaround to refresh the DataGrid
                        LocationDataGrid.ItemsSource = Locations_List;

                        // Get the index of the newly added or edited person
                        var index = Locations_List.IndexOf(location);

                        // Select the newly added or edited person
                        LocationDataGrid.SelectedIndex = index;

                        // Scroll to the newly added or edited person
                        LocationDataGrid.ScrollIntoView(location);

                        LocationDataGrid.UpdateLayout();
                        DataGridRow row = (DataGridRow)LocationDataGrid.ItemContainerGenerator.ContainerFromIndex(index);
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
            var selectedlocation = LocationDataGrid.SelectedItem as Locations;
            if (selectedlocation != null)
            {
                using (var connection = _databaseService.GetConnection())
                {
                    // Mark as deleted in the database
                    selectedlocation.IsDeleted = true;
                    connection.Update(selectedlocation);
                }
                // Remove from the ObservableCollection to update the UI
                Locations_List.Remove(selectedlocation);
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
