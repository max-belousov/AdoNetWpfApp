using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Controls;
using AdoNetWpfApp.Model;

namespace AdoNetWpfApp.ViewModel
{
    internal class ClientsViewModel
    {
        public ClientsViewModel(DataRowView row)
        {
            Row = row;
            Clients = new Clients(row);
        }

        public Clients Clients { get; set; }
        public DataRowView Row { get; set; }

    }
}
