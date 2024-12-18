namespace Udemy.Course.Domain.Interfaces.Repository;

public interface IElasticSearchRepository
{
    Task<T?> GetByIdAsync<T>(string id) where T : class;
    Task<IEnumerable<T>> SearchAsync<T>(string query) where T : class;
    Task IndexAsync<T>(T document) where T : class;
    Task UpdateAsync<T>(string id, T document) where T : class;
    Task DeleteAsync<T>(string id) where T : class;
    Task<bool> IndexExistsAsync(string indexName);
    Task CreateIndexAsync(string indexName);
    Task DeleteIndexAsync(string indexName);
    Task BulkIndexAsync<T>(IEnumerable<T> documents) where T : class;
    Task BulkDeleteAsync<T>(IEnumerable<string> ids) where T : class;
    Task<long> CountAsync<T>(string query) where T : class;
}
