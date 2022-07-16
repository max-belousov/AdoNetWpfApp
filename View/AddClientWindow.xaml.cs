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

namespace AdoNetWpfApp.View
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private AddClientWindow() { InitializeComponent(); }

        public AddClientWindow(DataRow row) : this()
        {
            cancelBtn.Click += delegate { this.DialogResult = false; };
            okBtn.Click += delegate
            {
                row["FirstName"] = txtFirstName.Text;
                row["SecondName"] = txtSecondName.Text;
                row["ThirdName"] = txtThirdName.Text;
                row["Phone"] = txtPhone.Text;
                row["Email"] = txtEmail.Text;
                this.DialogResult = !false;
            };

        }
    }
}
