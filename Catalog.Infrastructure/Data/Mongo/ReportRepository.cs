using Catalog.Domain.Entities.Mongo;
using Catalog.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Mongo
{
    public class ReportRepository : IReportRepository
    {
        private readonly CollectionConfiguration _configuration;
        private readonly IMongoCollection<Report> _reportCollection;

        public ReportRepository(IOptions<CollectionConfiguration> configuration)
        {
            _configuration = configuration.Value;
            var client = new MongoClient(_configuration.ConnectionString);
            var database = client.GetDatabase(_configuration.DatabaseName);
            _reportCollection = database.GetCollection<Report>(_configuration.CollectionName);
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _reportCollection.Find(report => true).ToListAsync();
        }

        public async Task<Report> GetByIdAsync(string id)
        {
            return await _reportCollection.Find(report => report.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Report report)
        {
            await _reportCollection.InsertOneAsync(report);
        }

        public async Task UpdateAsync(string id, Report report)
        {
            await _reportCollection.ReplaceOneAsync(report => report.Id == id, report);
        }

        public async Task DeleteAsync(string id)
        {
            await _reportCollection.DeleteOneAsync(report => report.Id == id);
        }
    }
}