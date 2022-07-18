using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdoNetWpfApp.View;
using AdoNetWpfApp.ViewModel;

namespace AdoNetWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientsViewModel _clientViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _clientViewModel = new ClientsViewModel((DataRowView)gridView.SelectedItem);
            gridView.DataContext = _clientViewModel.Clients.DataTable.DefaultView;
        }


        /// <summary>
        /// Начало редактирования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _clientViewModel.Clients.RowView = (DataRowView)gridView.SelectedItem;
            _clientViewModel.Clients.RowView.BeginEdit();
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (_clientViewModel.Clients.RowView == null) return;
            _clientViewModel.Clients.RowView.EndEdit();
            try
            {
                _clientViewModel.Clients.DataAdapter.Update(_clientViewModel.Clients.DataTable);
            }
            catch (Exception)
            {
                MessageBox.Show("Для добавления клиента нажмите правую кнопку мыши");
                _clientViewModel.Clients.RowView.Delete();
            }
            
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _clientViewModel.Clients.RowView = (DataRowView)gridView.SelectedItem;
            }
            catch (Exception)
            {
                MessageBox.Show("Нельзя удалить пустой объект");
            }
            _clientViewModel.Clients.RowView?.Row.Delete();
            try
            {
                _clientViewModel.Clients.DataAdapter.Update(_clientViewModel.Clients.DataTable);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddClick(object sender, RoutedEventArgs e)
        {
            DataRow r = _clientViewModel.Clients.DataTable.NewRow();
            AddClientWindow add = new AddClientWindow(r);
            add.ShowDialog();


            if (add.DialogResult.Value)
            {
                _clientViewModel.Clients.DataTable.Rows.Add(r);
                _clientViewModel.Clients.DataAdapter.Update(_clientViewModel.Clients.DataTable);
            }
        }

        private void MenuItemOrdersClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var key = ((DataRowView)gridView.SelectedItems[0]).Row["Email"].ToString();
                OrdersWindow orders = new OrdersWindow(key);
                orders.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Выберите клиента");
            }

        }
    }
}
