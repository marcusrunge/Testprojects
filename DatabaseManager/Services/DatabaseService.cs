using DatabaseManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{
    public class DatabaseService : IDataBaseService
    {
        SqlConnection _sqlConnection;
        IDatabaseSettings _dataBaseSettings;

        public DatabaseService(IDatabaseSettings dataBaseSettings)
        {
            _dataBaseSettings = dataBaseSettings;
            if (!String.IsNullOrEmpty(_dataBaseSettings.ConnectionString)) _sqlConnection = new SqlConnection(_dataBaseSettings.ConnectionString);
        }

        public async Task CreateDatabase()
        {
            _sqlConnection = new SqlConnection($"Data Source={_dataBaseSettings.Source};Persist Security Info=True;User ID={_dataBaseSettings.Id};Password={_dataBaseSettings.Password}");
            SqlCommand sqlCommand = new SqlCommand($"CREATE DATABASE [{_dataBaseSettings.DatabaseName}]", _sqlConnection);
            await _sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            _sqlConnection.Close();
        }

        public async Task DropDatabase()
        {
            _sqlConnection = new SqlConnection($"Data Source={_dataBaseSettings.Source};Persist Security Info=True;User ID={_dataBaseSettings.Id};Password={_dataBaseSettings.Password}");
            SqlCommand sqlCommand = new SqlCommand($"DROP DATABASE {_dataBaseSettings.DatabaseName}", _sqlConnection);
            await _sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            _sqlConnection.Close();
        }

        public async Task CreateTable(Dictionary<string, Tuple<int, int, Type>> dictionary)
        {
            var sqlCommandString = $"CREATE TABLE {_dataBaseSettings.TableName} (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY";
            for (int i = 0; i < dictionary.Count; i++) sqlCommandString = String.Concat(sqlCommandString, $", [{dictionary.ElementAt(i).Key}] {ResolveDatabaseType(dictionary.ElementAt(i).Value.Item3)}");
            sqlCommandString = String.Concat(sqlCommandString, ")");
            _sqlConnection = new SqlConnection(_dataBaseSettings.ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(sqlCommandString, _sqlConnection);
            await _sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            _sqlConnection.Close();
        }

        public async Task<List<string>> GetColumnList()
        {
            string[] restrictionValues = new string[4] { null, null, _dataBaseSettings.TableName, null };
            await _sqlConnection.OpenAsync();
            var columnList = _sqlConnection.GetSchema("Columns", restrictionValues).AsEnumerable().Select(dataRow => dataRow.Field<String>("Column_Name")).ToList();
            _sqlConnection.Close();
            return columnList;
        }

        public async Task Insert(DataTable dataTable)
        {
            try
            {
                _sqlConnection = new SqlConnection(_dataBaseSettings.ConnectionString);
                await _sqlConnection.OpenAsync();
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(_sqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = dataTable.TableName;
                    foreach (var column in dataTable.Columns) sqlBulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                    await sqlBulkCopy.WriteToServerAsync(dataTable);
                }
                _sqlConnection.Close();
            }
            catch (Exception)
            {
                _sqlConnection.Close();
            }
        }

        public async Task DropTable()
        {
            _sqlConnection = new SqlConnection(_dataBaseSettings.ConnectionString);
            SqlCommand sqlCommand = new SqlCommand($"DROP TABLE [{_dataBaseSettings.TableName}]", _sqlConnection);
            await _sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            _sqlConnection.Close();
        }

        public async Task<DataTable> Select()
        {
            _sqlConnection = new SqlConnection(_dataBaseSettings.ConnectionString);
            SqlCommand sqlCommand = new SqlCommand($"", _sqlConnection);
            await _sqlConnection.OpenAsync();
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (await sqlDataReader.ReadAsync())
            {
                //Hier ist noch Arbeit!
            }
            _sqlConnection.Close();
            return null;
        }

        string ResolveDatabaseType(Type t)
        {
            if (t == typeof(int)) return "int";
            if (t == typeof(decimal)) return "decimal(18,9)";
            if (t == typeof(DateTime)) return "datetime";
            return "varchar(MAX)";
        }
    }
}