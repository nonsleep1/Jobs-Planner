﻿using Jobs_Planner.Database;
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

        private void WorkersDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var worker = e.Row.Item as Workers;
                if (worker != null)
                {
                    using (var connection = _databaseService.GetConnection())
                    {
                        if (worker.Id == 0) // New Worker
                        {
                            connection.Insert(worker);
                        }
                        else // Existing person
                        {
                            connection.Update(worker);
                        }
                    }
                    LoadWorkers();
                }
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
        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MarkAsDeleted_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            ContextMenu contextMenu = dataGrid.ContextMenu;
            contextMenu.IsOpen = true;
        }

    }
}
