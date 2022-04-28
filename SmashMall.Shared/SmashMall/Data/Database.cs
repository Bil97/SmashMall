using SmashMall.Models.UserAccount;
using SQLite;
using System;
using System.Threading.Tasks;

namespace SmashMall.Data
{
    public class Database
    {
        SQLiteAsyncConnection _database;

        public Database()
        {
            string dbPath = $"{Environment.SpecialFolder.ApplicationData}/SmashMall/SmashMall.db";
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ApplicationUser>().Wait();
        }

        public ApplicationUser GetUser() => _database.Table<ApplicationUser>().FirstOrDefaultAsync().Result;
        public Task SaveUserAsync(ApplicationUser user) => _database.InsertOrReplaceAsync(user);
    }
}