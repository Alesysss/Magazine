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
        public async Task<int> SaveProducts(Products products)
        {
            return await database.InsertAsync(products);
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
        //public async Task<Users> AuthenticateAsync(string login, string password)
        //{
        //    return await database.Table<Users>()
        //                         .Where(c => c.Email == login && c.Passwordd == password)
        //                         .FirstOrDefaultAsync();
        //}

        // Предполагается, что это метод аутентификации в вашем классе DB.cs
        public async Task<Users> AuthenticateAsync(string login, string password)
        {
            var user = await database.Table<Users>()
                                     .Where(c => c.Email == login && c.Passwordd == password)
                                     .FirstOrDefaultAsync();

            if (user != null)
            {
                App.SetCurrentUser(user); // Установка текущего пользователя
            }

            return user;
        }
        public async Task<Users> GetUsersAsync(int Id)
        {
            return await database.FindAsync<Users>(Id);
        }

        // В классе DB обновим метод AddToCartAsync:

        public async Task<int> AddToCartAsync(int userId, int productId, decimal price)
        {
            try
            {
                var cartItem = await database.Table<Cart>().FirstOrDefaultAsync(c => c.Users_id == userId && c.Product_id == productId);

                if (cartItem != null)
                {
                    // Если товар уже есть в корзине, увеличим значение столбца Quantity на 1
                    cartItem.Quantity++;
                    return await database.UpdateAsync(cartItem);
                }
                else
                {
                    // Если товара нет в корзине, добавим его и установим значение Quantity равным 1
                    cartItem = new Cart
                    {
                        Users_id = userId,
                        Product_id = productId,
                        Moneys = price,
                        Quantity = 1
                    };
                    return await database.InsertAsync(cartItem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to cart: {ex.Message}");
                return -1;
            }
        }


        public async Task<List<Products>> GetCartProductsAsync(int userId)
        {
            var cartItems = await database.Table<Cart>().Where(c => c.Users_id == userId).ToListAsync();
            var productIds = cartItems.Select(item => item.Product_id).ToList();
            var products = await database.Table<Products>().Where(p => productIds.Contains(p.Id)).ToListAsync();
            return products;
        }
        // В классе DB добавьте следующие методы:

        public async Task<bool> IsProductInCartAsync(int userId, int productId)
        {
            var cartItem = await database.Table<Cart>().FirstOrDefaultAsync(c => c.Users_id == userId && c.Product_id == productId);
            return cartItem != null;
        }

        public async Task<int> IncreaseQuantityAsync(int userId, int productId)
        {
            var cartItem = await database.Table<Cart>().FirstOrDefaultAsync(c => c.Users_id == userId && c.Product_id == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                return await database.UpdateAsync(cartItem);
            }
            return -1;
        }

        public async Task<decimal> GetTotalPriceAsync(int userId)
        {
            var cartItems = await database.Table<Cart>().Where(c => c.Users_id == userId).ToListAsync();
            return cartItems.Sum(item => item.Moneys * item.Quantity);
        }

        public async Task<List<Cart>> GetCartItemsAsync(int userId)
        {
            return await database.Table<Cart>().Where(c => c.Users_id == userId).ToListAsync();
        }
        public async Task<int> UpdateCartItemAsync(Cart cartItem)
        {
            return await database.UpdateAsync(cartItem);
        }

        public async Task<int> DeleteCartItemAsync(Cart cartItem)
        {
            return await database.DeleteAsync(cartItem);
        }

        public async Task<int> ClearCartAsync(int userId)
        {
            try
            {
                var cartItems = await GetCartItemsAsync(userId);
                foreach (var item in cartItems)
                {
                    await DeleteCartItemAsync(item);
                }
                return 1; // Успешно очищено
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart: {ex.Message}");
                return -1; // Произошла ошибка
            }
        }
        public async Task<List<Products>> SearchProductsAsync(string searchText)
        {
            searchText = searchText.ToLower(); // Преобразуем текст поиска в нижний регистр

            // Получаем все товары из базы данных и фильтруем по введенному тексту
            var products = await GetProductsAsync();
            var searchResults = products.Where(p => p.Namee.ToLower().Contains(searchText)).ToList();

            return searchResults;
        }
        public async Task<int> DeleteProducts(Products products)
        {
            return await database.DeleteAsync(products);
        }
        public async Task<int> UpdateProductAsync(Products product)
        {
            return await database.UpdateAsync(product);
        }



    }
}
