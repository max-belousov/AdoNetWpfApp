using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using AdoNetWpfApp.Model;
using AdoNetWpfApp.View;


namespace AdoNetWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Clients _clients;
        public MainWindow()
        {
            InitializeComponent();
            _clients = new Clients((DataRowView)gridView.SelectedItem);
            gridView.DataContext = _clients.DataTable.DefaultView;
        }


        /// <summary>
        /// Начало редактирования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _clients.RowView = (DataRowView)gridView.SelectedItem;
            _clients.RowView.BeginEdit();
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (_clients.RowView == null) return;
            _clients.RowView.EndEdit();
            try
            {
                _clients.DataAdapter.Update(_clients.DataTable);
            }
            catch (Exception)
            {
                MessageBox.Show("Для добавления клиента нажмите правую кнопку мыши");
                _clients.RowView.Delete();
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
                _clients.RowView = (DataRowView)gridView.SelectedItem;
                var choiceDelete = MessageBox.Show(this,
                    $"Вы действительно хотите удалить {(((DataRowView)gridView.SelectedItems[0])!).Row["Email"]}?",
                    "Удаление клиента",
                    MessageBoxButton.YesNo);
                if (choiceDelete == MessageBoxResult.No) return;
                _clients.RowView?.Row.Delete();
                _clients.DataAdapter.Update(_clients.DataTable);
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
        private void MenuItemAddClick(object sender, RoutedEventArgs e)
        {
            var r = _clients.DataTable.NewRow();
            var add = new AddClientWindow(r);
            add.ShowDialog();


            if (add.DialogResult != null && add.DialogResult.Value)
            {
                _clients.DataTable.Rows.Add(r);
                _clients.DataAdapter.Update(_clients.DataTable);
            }
        }

        private void MenuItemOrdersClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var key = (((DataRowView)gridView.SelectedItems[0])!).Row["Email"].ToString();
                var orders = new OrdersWindow(key);
                orders.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите клиента");
            }

        }
    }
}
