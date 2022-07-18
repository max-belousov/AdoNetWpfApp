using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace AdoNetWpfApp.Model
{
    internal class Orders
    {
        public Orders(string key, DataRowView row)
        {
            Key = key;
            Preparing();
            RowView = row;
        }
        public string Key { get; set; }
        public OleDbConnection Connection { get; set; }
        public OleDbDataAdapter DataAdapter { get; set; }
        public DataTable DataTable { get; set; }
        public DataRowView RowView { get; set; }

        private void Preparing()
        {
            #region Init
            var connectionStringBuilder = new OleDbConnectionStringBuilder()
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = @"C:\Users\delicia\Documents\MSAccessOnlineShopdb.accdb"

            };

            Connection = new OleDbConnection(connectionStringBuilder.ConnectionString);
            DataTable = new DataTable();
            DataAdapter = new OleDbDataAdapter();

            #endregion

            #region select


            //var sql = @"SELECT * FROM Orders Order By Orders.Id WHERE Email = @Email";
            //var sql = @"SELECT * FROM Orders Order By Orders.Id";
            //var sql = $@"SELECT * FROM Orders WHERE Email = @[{Key}] Order By Id ";
            var sql = @"SELECT * FROM Orders WHERE Email = @Email;";


            DataAdapter.SelectCommand = new OleDbCommand(sql, Connection);

            DataAdapter.SelectCommand.Parameters.Add("@Email", OleDbType.VarChar, 50, "Email").Value = Key;
            //DataAdapter.SelectCommand.Parameters.Add($"@{Key}", OleDbType.VarChar, 20, "Email").SourceVersion = DataRowVersion.Original;


            #endregion

            #region insert

            sql = @"INSERT INTO Orders (ItemName, ItemCode, Email) 
                                 VALUES (@ItemName, @ItemCode, @Email);";

            DataAdapter.InsertCommand = new OleDbCommand(sql, Connection);

            DataAdapter.InsertCommand.Parameters.Add("@ItemName", OleDbType.VarChar, 20, "ItemName");
            DataAdapter.InsertCommand.Parameters.Add("@ItemCode", OleDbType.Integer, 20, "ItemCode");
            DataAdapter.InsertCommand.Parameters.Add("@Email", OleDbType.VarChar, 50, "Email");

            #endregion

            #region update

            sql = @"UPDATE Orders 
                    SET 
                           ItemName = @ItemName,
                           ItemCode = @ItemCode, 
                           Email = @Email
                    WHERE Id = @Id";

            DataAdapter.UpdateCommand = new OleDbCommand(sql, Connection);
            DataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Integer, 0, "Id").SourceVersion = DataRowVersion.Original;
            DataAdapter.UpdateCommand.Parameters.Add("@ItemName", OleDbType.VarChar, 20, "ItemName");
            DataAdapter.UpdateCommand.Parameters.Add("@ItemCode", OleDbType.Integer, 20, "ItemCode");
            DataAdapter.UpdateCommand.Parameters.Add("@Email", OleDbType.VarChar, 50, "Email");

            #endregion

            #region delete

            sql = "DELETE FROM Orders WHERE Id = @Id";

            DataAdapter.DeleteCommand = new OleDbCommand(sql, Connection);
            DataAdapter.DeleteCommand.Parameters.Add("@Id", OleDbType.Integer, 4, "Id");

            #endregion


            //DataAdapter.Fill(DataTable);
            try
            {
                DataAdapter.Fill(DataTable);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

}
