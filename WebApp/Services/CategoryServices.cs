using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApp.Models;

namespace WebApp.Services
{
    public class CategoryServices
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryServices(DatabaseSetting databaseSetting)
        {
            var mongoClient = new MongoClient(databaseSetting.ConnectionString);
            _categories = mongoClient.GetDatabase(databaseSetting.DatabaseName).GetCollection<Category>(databaseSetting.CollectionName);
        }

        public async Task<List<Category>> Get() => await _categories.Find(_ => true).ToListAsync();

        public async Task<Category> Get(string id) => await _categories.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task Create(Category category) => await _categories.InsertOneAsync(category);

        public async Task Update(string id, Category updateCategory) => await _categories.ReplaceOneAsync(x => x.Id == id, updateCategory);

        public async Task Delete(string id) => await _categories.FindOneAndDeleteAsync(x => x.Id == id);
    }
}
