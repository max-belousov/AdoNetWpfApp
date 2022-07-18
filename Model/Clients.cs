using System.Data;
using System.Data.SqlClient;


namespace AdoNetWpfApp.Model
{
    internal class Clients
    {
        public Clients(DataRowView row)
        {
            Preparing();
            RowView = row;
        }

        public SqlConnection Connection { get; set; }
        public SqlDataAdapter DataAdapter { get; set; }
        public DataTable DataTable { get; set; }
        public DataRowView RowView { get; set; }

        private void Preparing()
        {
            #region Init

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "DELICIA-ASUS-ZB",
                InitialCatalog = "MSSQLOnlineShopdb",
                IntegratedSecurity = true
            };

            Connection = new SqlConnection(connectionStringBuilder.ConnectionString);
            DataTable = new DataTable();
            DataAdapter = new SqlDataAdapter();

            #endregion

            #region select


            var sql = @"SELECT * FROM Customer Order By Customer.Id";
            DataAdapter.SelectCommand = new SqlCommand(sql, Connection);

            #endregion

            #region insert

            sql = @"INSERT INTO Customer (FirstName, SecondName, ThirdName, Phone, Email) 
                                 VALUES (@FirstName, @SecondName, @ThirdName, @Phone, @Email); 
                     SET @Id = @@IDENTITY;";

            DataAdapter.InsertCommand = new SqlCommand(sql, Connection);

            DataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            DataAdapter.InsertCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar, 40, "FirstName");
            DataAdapter.InsertCommand.Parameters.Add("@SecondName", SqlDbType.NVarChar, 40, "SecondName");
            DataAdapter.InsertCommand.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 40, "ThirdName");
            DataAdapter.InsertCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 50, "Email");
            DataAdapter.InsertCommand.Parameters.Add("@Phone", SqlDbType.NVarChar, 15, "Phone");

            #endregion

            #region update

            sql = @"UPDATE Customer 
                    SET 
                           FirstName = @FirstName,
                           SecondName = @SecondName, 
                           ThirdName = @ThirdName,
                           Email = @Email,
                           Phone = @Phone
                    WHERE Id = @Id";

            DataAdapter.UpdateCommand = new SqlCommand(sql, Connection);
            DataAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id").SourceVersion = DataRowVersion.Original;
            DataAdapter.UpdateCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar, 40, "FirstName");
            DataAdapter.UpdateCommand.Parameters.Add("@SecondName", SqlDbType.NVarChar, 40, "SecondName");
            DataAdapter.UpdateCommand.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 40, "ThirdName");
            DataAdapter.UpdateCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 50, "Email");
            DataAdapter.UpdateCommand.Parameters.Add("@Phone", SqlDbType.NVarChar, 15, "Phone");

            #endregion

            #region delete

            sql = "DELETE FROM Customer WHERE Id = @Id";

            DataAdapter.DeleteCommand = new SqlCommand(sql, Connection);
            DataAdapter.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

            #endregion


            DataAdapter.Fill(DataTable);

        }
    }

}
