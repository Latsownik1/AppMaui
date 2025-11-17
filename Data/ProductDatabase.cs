using SQLite;
using Produkty; // lub Produkty.Models, jeśli Product.cs masz w folderze Models

namespace Produkty.Data;


public class ProductDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public ProductDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Product>().Wait();
    }

    public Task<List<Product>> GetProductsAsync() =>
        _database.Table<Product>().ToListAsync();

    public Task<int> SaveProductAsync(Product product) =>
        _database.InsertOrReplaceAsync(product);

    public Task<int> DeleteProductAsync(Product product) =>
        _database.DeleteAsync(product);
}