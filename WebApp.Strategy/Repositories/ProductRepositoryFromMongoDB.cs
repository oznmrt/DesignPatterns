using MongoDB.Driver;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories
{
    public class ProductRepositoryFromMongoDB : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;
        public ProductRepositoryFromMongoDB(IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("MongoDB");

            var client = new MongoClient(connString);

            var dataBase = client.GetDatabase("ProductDB");

            _collection = dataBase.GetCollection<Product>("Products");
        }

        public async Task Delete(Product product)
        {
            await _collection.DeleteOneAsync(p => p.Id == product.Id);
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _collection.Find(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> Save(Product product)
        {
            await _collection.InsertOneAsync(product);

            return product;
        }

        public async Task Update(Product product)
        {
            await _collection.FindOneAndReplaceAsync(p => p.Id == product.Id, product);
        }
    }
}
