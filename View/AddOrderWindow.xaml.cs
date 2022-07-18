using System.Data;
using System.Windows;

namespace AdoNetWpfApp.View
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
        }

        public AddOrderWindow(DataRow row, string email) : this()
        {
            Email = email;
            cancelBtn.Click += delegate { this.DialogResult = false; };
            okBtn.Click += delegate
            {
                row["ItemName"] = txtItemName.Text;
                row["ItemCode"] = txtItemCode.Text;
                row["Email"] = Email;
                this.DialogResult = !false;
            };
        }

        public string Email { get; set; }
    }
}
