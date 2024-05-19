using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using static Xamarin.Essentials.Permissions;


namespace Magazine.Model
{
    public class DB
    {
        SQLiteAsyncConnection database;

        public DB (string databasePath)
        {
            database = new SQLiteAsyncConnection(databasePath);
            database.CreateTableAsync<Users>().Wait();
            database.CreateTableAsync<Products>().Wait();
            database.CreateTableAsync<Orders>().Wait();
            database.CreateTableAsync<Favourites>().Wait();
            database.CreateTableAsync<Categories>().Wait();
            database.CreateTableAsync<Cart>().Wait();
        }

        public async Task<List<Products>> GetProductsAsync()
        {
            return await database.Table<Products>().ToListAsync();
        }

        public async Task<Products> GetProductByIdAsync(int id)
        {
            return await database.Table<Products>().Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveUsers(Users users)
        {
            return await database.InsertAsync(users);
        }
        public async Task<int> SaveUser(Users users)
        {
            if (users.Id != 0)
            {
                return await database.UpdateAsync(users);
            }
            else
            {
                return await database.InsertAsync(users);
            }
        }
        public async Task<Users> AuthenticateAsync(string login, string password)
        {
            return await database.Table<Users>()
                                 .Where(c => c.Email == login && c.Passwordd == password)
                                 .FirstOrDefaultAsync();
        }
        public async Task<Users> GetUsersAsync(int Id)
        {
            return await database.FindAsync<Users>(Id);
        }

    }
}
