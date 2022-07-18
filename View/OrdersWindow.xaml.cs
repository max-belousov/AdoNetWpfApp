using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using AdoNetWpfApp.Model;

namespace AdoNetWpfApp.View
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private readonly Orders _orders;
        private readonly string _email;
        public OrdersWindow(string key)
        {
            InitializeComponent();
            _orders = new Orders(key, (DataRowView)OrdersGridView.SelectedItem);
            OrdersGridView.DataContext = _orders.DataTable.DefaultView;
            _email = key;

        }

        //public string Email { get; set; }
        
        /// <summary>
        /// Начало редактирования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _orders.RowView = (DataRowView)OrdersGridView.SelectedItem;
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
                MessageBox.Show("Для добавления заказа нажмите правую кнопку мыши");
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
                _orders.RowView = (DataRowView)OrdersGridView.SelectedItem;
                var choiceDelete = MessageBox.Show(this,
                    $"Вы действительно хотите удалить {(((DataRowView)OrdersGridView.SelectedItems[0])!).Row["ItemName"]}?",
                    "Удаление клиента",
                    MessageBoxButton.YesNo);
                if (choiceDelete == MessageBoxResult.No) return;
                _orders.RowView?.Row.Delete();
                _orders.DataAdapter.Update(_orders.DataTable);
            }
            catch (Exception)
            {
                MessageBox.Show("Нельзя удалить пустой объект");
            }
        }

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderAddClick(object sender, RoutedEventArgs e)
        {
            var r = _orders.DataTable.NewRow();
            var add = new AddOrderWindow(r, _email);
            add.ShowDialog();
            if (add.DialogResult != null && add.DialogResult.Value)
            {
                _orders.DataTable.Rows.Add(r);
                _orders.DataAdapter.Update(_orders.DataTable);
            }
        }
    }
}
