using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs_Planner
{
    public class DatabaseService
    {

        private readonly string _dbPath;

        public DatabaseService(string dbPath)
        {
            _dbPath = dbPath;
            //InitializeDatabase();
        }


        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_dbPath);
        }

    }
}
