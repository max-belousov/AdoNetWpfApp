using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AdoNetWpfApp.Model;

namespace AdoNetWpfApp.View
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private readonly Orders _orders;
        public OrdersWindow(string key)
        {
            InitializeComponent();
            _orders = new Orders(key, (DataRowView)gridView.SelectedItem);
            gridView.DataContext = _orders.DataTable.DefaultView;

        }

        /// <summary>
        /// Начало редактирования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _orders.RowView = (DataRowView)gridView.SelectedItem;
            _orders.RowView.BeginEdit();
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (_orders.RowView == null) return;
            _orders.RowView.EndEdit();
            try
            {
                _orders.DataAdapter.Update(_orders.DataTable);
            }
            catch (Exception)
            {
                MessageBox.Show("Для добавления клиента нажмите правую кнопку мыши");
                _orders.RowView.Delete();
            }

        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _orders.RowView = (DataRowView)gridView.SelectedItem;
            }
            catch (Exception)
            {
                MessageBox.Show("Нельзя удалить пустой объект");
            }
            
            _orders.RowView?.Row.Delete();
            try
            {
                _orders.DataAdapter.Update(_orders.DataTable);

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
        private void OrderAddClick(object sender, RoutedEventArgs e)
        {
            if (gridView.SelectedItem == null) return;
            var email = ((DataRowView)gridView.SelectedItems[0]).Row["Email"].ToString();
            DataRow r = _orders.DataTable.NewRow();
            AddOrderWindow add = new AddOrderWindow(r, email);
            add.ShowDialog();


            if (add.DialogResult.Value)
            {
                _orders.DataTable.Rows.Add(r);
                _orders.DataAdapter.Update(_orders.DataTable);
            }
        }
    }
}
