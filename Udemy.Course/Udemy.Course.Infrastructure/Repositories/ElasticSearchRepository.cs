using Elastic.Clients.Elasticsearch;
using Udemy.Course.Domain.Interfaces.Repository;

namespace Udemy.Course.Infrastructure.Repositories;

public class ElasticSearchRepository(ElasticsearchClient elasticClient) : IElasticSearchRepository
{
    private readonly ElasticsearchClient _elasticClient = elasticClient;

    public async Task<T?> GetByIdAsync<T>(string id) where T : class
    {
        var response = await _elasticClient.GetAsync<T>(id);
        return response.Found ? response.Source : null;
    }

    public async Task<IEnumerable<T>> SearchAsync<T>(string query) where T : class
    {
        var searchResponse = await _elasticClient.SearchAsync<T>(s => s
            .Query(q => q
                .QueryString(d => d
                        .Query(query)
                )
            )
        );

        if (searchResponse.IsValidResponse && searchResponse.Documents.Any())
        {
            return searchResponse.Documents;
        }

        return [];
    }

    public async Task IndexAsync<T>(T document) where T : class
    {
        var response = await _elasticClient.IndexAsync(document);
        if (!response.IsValidResponse)
        {
            throw new Exception($"Error indexing document: {response.DebugInformation}");
        }
    }

    public async Task UpdateAsync<T>(string id, T document) where T : class
    {
        var response = await _elasticClient.UpdateAsync<T, T>(id, u => u.Doc(document));
        if (!response.IsValidResponse)
        {
            throw new Exception($"Error updating document: {response.DebugInformation}");
        }
    }

    public async Task DeleteAsync<T>(string id) where T : class
    {
        var response = await _elasticClient.DeleteAsync<T>(id);
        if (!response.IsValidResponse)
        {
            throw new Exception($"Error deleting document: {response.DebugInformation}");
        }
    }

    public async Task<bool> IndexExistsAsync(string indexName)
    {
        var response = await _elasticClient.Indices.ExistsAsync(indexName);
        return response.Exists;
    }

    public async Task CreateIndexAsync(string indexName)
    {
        var response = await _elasticClient.Indices.CreateAsync(indexName);
        if (!response.IsValidResponse)
        {
            throw new Exception($"Error creating index: {response.DebugInformation}");
        }
    }

    public async Task DeleteIndexAsync(string indexName)
    {
        var response = await _elasticClient.Indices.DeleteAsync(indexName);
        if (!response.IsValidResponse)
        {
            throw new Exception($"Error deleting index: {response.DebugInformation}");
        }
    }

    public async Task BulkIndexAsync<T>(IEnumerable<T> documents) where T : class
    {
        var bulkResponse = await _elasticClient.BulkAsync(b => b.IndexMany(documents));
        if (!bulkResponse.IsValidResponse)
        {
            throw new Exception($"Error bulk indexing documents: {bulkResponse.DebugInformation}");
        }
    }

    public async Task BulkDeleteAsync<T>(IEnumerable<string> ids) where T : class
    {
        var bulkResponse = await _elasticClient.BulkAsync(b => b.DeleteMany(ids));
        if (!bulkResponse.IsValidResponse)
        {
            throw new Exception($"Error bulk deleting documents: {bulkResponse.DebugInformation}");
        }
    }

    public async Task<long> CountAsync<T>(string query) where T : class
    {
        var countResponse = await _elasticClient.CountAsync<T>(c => c.Query(q => q.QueryString(d => d.Query(query))));
        return countResponse.Count;
    }
}