using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DatabaseManager.Interfaces
{
    public interface IDataBaseService
    {
        Task CreateDatabase();
        Task DropDatabase();
        Task CreateTable(Dictionary<string, Tuple<int, int, Type>> dictionary);        
        Task<List<string>> GetColumnList();        
        Task Insert(DataTable dataTable);
        Task DropTable();
        Task<DataTable> Select();
    }
}
