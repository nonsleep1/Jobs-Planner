using Jobs_Planner.Database;
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
        public void Createdatabase(string dbPath)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<Users>();
                db.CreateTable<Role>();
                db.CreateTable<Workers>();
                db.CreateTable<ConfigurationDataBase>();
                db.CreateTable<JobsSymbols>();
                db.CreateTable<FreqTime>();
                db.CreateTable<Locations>();


                var initialRoles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "Vartotojas" }
                };
                db.InsertAll(initialRoles);

                var initialUsers = new List<Users>
                {
                    new Users {Username = "admin", Password="admin", RoleId = 1},
                };
                db.InsertAll(initialUsers);

                var initialFreqTime = new List<FreqTime>
                {
                    new FreqTime {Name = "Valandos"},
                    new FreqTime {Name = "Dienos"},
                    new FreqTime {Name = "Savaitės"},
                    new FreqTime {Name = "Mėnesiai"},
                    new FreqTime {Name = "Metai"}
                };
                db.InsertAll(initialFreqTime);


            }


        }

    }
}
