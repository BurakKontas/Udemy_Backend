using System.Collections.ObjectModel;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;
using Udemy.CDN.Domain.Interfaces;
using Udemy.Common.ModelBinder;

namespace Udemy.CDN.Infrastructure;

public class MinioService(IMinioClient minioClient) : IMinioService
{
    private readonly IMinioClient _minioClient = minioClient;

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        var args = new BucketExistsArgs()
            .WithBucket(bucketName);

        return await _minioClient.BucketExistsAsync(args);
    }

    public async Task<bool> MakeBucketAsync(string bucketName)
    {
        var args = new MakeBucketArgs()
            .WithBucket(bucketName);

        await _minioClient.MakeBucketAsync(args);

        return true;
    }

    public async Task<bool> UploadFileAsync(string bucketName, string filePath, string objectName, string contentType = "application/octet-stream")
    {
        var args = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithFileName(filePath)
            .WithObject(objectName)
            .WithContentType(contentType);

        var response = await _minioClient.PutObjectAsync(args);

        if((int)response.ResponseStatusCode < 200 || (int)response.ResponseStatusCode > 300)
        {
            throw new Exception($"Error uploading file: {response.ResponseContent}");
        }

        return true;
    }

    public async Task<bool> UploadFileAsync(string bucketName, byte[] fileData, string objectName, string contentType = "application/octet-stream")
    {
        var args = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithContentType(contentType)
            .WithStreamData(new MemoryStream(fileData));

        var response = await _minioClient.PutObjectAsync(args);

        if ((int)response.ResponseStatusCode < 200 || (int)response.ResponseStatusCode >= 300)
        {
            throw new Exception($"Error uploading file: {response.ResponseContent}");
        }

        return true;
    }

    public async Task<Stream> DownloadFileAsync(string bucketName, string objectName)
    {
        var memoryStream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            });

        await _minioClient.GetObjectAsync(args);

        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public async Task<List<string>> ListBucketsAsync(EndpointFilter filter)
    {
        var bucketList = await _minioClient.ListBucketsAsync();

        // List<Bucket> -> Collection<Bucket>
        var bucketCollection = new Collection<Minio.DataModel.Bucket>(bucketList.Buckets);

        // Filtering by 'FilterBy' and 'FilterValue'
        if (!string.IsNullOrEmpty(filter.FilterBy) && !string.IsNullOrEmpty(filter.FilterValue))
        {
            bucketCollection = new Collection<Minio.DataModel.Bucket>(bucketCollection
                .Where(b => b.Name.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase))
                .ToList());
        }

        // Sorting by 'SortBy' and 'SortOrder'
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            if (filter.SortOrder == "asc")
            {
                bucketCollection = new Collection<Minio.DataModel.Bucket>(bucketCollection
                    .OrderBy(b => b.Name)
                    .ToList());
            }
            else if (filter.SortOrder == "desc")
            {
                bucketCollection = new Collection<Minio.DataModel.Bucket>(bucketCollection
                    .OrderByDescending(b => b.Name)
                    .ToList());
            }
        }

        // Apply pagination
        var paginatedBuckets = bucketCollection
            .Skip(filter.Start)
            .Take(filter.Limit)
            .Select(b => b.Name)
            .ToList();

        return paginatedBuckets;
    }

    public async Task<List<string>> ListObjectsAsync(string bucketName, EndpointFilter filter)
    {
        var args = new ListObjectsArgs()
            .WithBucket(bucketName)
            .WithRecursive(true);

        var objectNames = new List<string>();

        await foreach (var item in _minioClient.ListObjectsEnumAsync(args))
        {
            if (item != null)
            {
                objectNames.Add(item.Key);
            }
        }

        // Filtering by 'FilterBy' and 'FilterValue'
        if (!string.IsNullOrEmpty(filter.FilterBy) && !string.IsNullOrEmpty(filter.FilterValue))
        {
            objectNames = objectNames
                .Where(obj => obj.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Sorting by 'SortBy' and 'SortOrder'
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            if (filter.SortOrder == "asc")
            {
                objectNames = objectNames.OrderBy(obj => obj).ToList();
            }
            else if (filter.SortOrder == "desc")
            {
                objectNames = objectNames.OrderByDescending(obj => obj).ToList();
            }
        }

        // Apply pagination
        var paginatedObjects = objectNames
            .Skip(filter.Start)
            .Take(filter.Limit)
            .ToList();

        return paginatedObjects;
    }

    public async Task<bool> DeleteFileAsync(string bucketName, string objectName)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        await _minioClient.RemoveObjectAsync(args);
        return true;
    }

    public async Task<bool> DeleteFilesAsync(string bucketName, List<string> objectNames)
    {
        var args = new RemoveObjectsArgs()
            .WithBucket(bucketName)
            .WithObjects(objectNames);

        var deleteErrors = await _minioClient.RemoveObjectsAsync(args);

        if (deleteErrors != null && deleteErrors.Count > 0)
        {
            var errorMessages = deleteErrors.Select(e => $"Error deleting object: {e.Key}");
            throw new AggregateException(errorMessages.Select(msg => new Exception(msg)));
        }

        return true;
    }

    public async Task<bool> DeleteBucketAsync(string bucketName)
    {
        var args = new RemoveBucketArgs()
            .WithBucket(bucketName);

        await _minioClient.RemoveBucketAsync(args);
        return true;
    }

    public async Task<string> GeneratePreSignedUrlAsync(string bucketName, string objectName, int expiryInSeconds)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expiryInSeconds);

        return await _minioClient.PresignedGetObjectAsync(args);
    }

    public async Task<IDictionary<string, string>> GetFileMetadataAsync(string bucketName, string objectName)
    {
        var args = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        var metadata = await _minioClient.StatObjectAsync(args);
        return metadata.MetaData;
    }

    public async Task<string> GetFileContentTypeAsync(string bucketName, string objectName)
    {
        var args = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        var metadata = await _minioClient.StatObjectAsync(args);

        return metadata.ContentType;
    }

    public async Task<List<string>> ListObjectsByPrefixAsync(string bucketName, string prefix, EndpointFilter filter)
    {
        var args = new ListObjectsArgs()
            .WithBucket(bucketName)
            .WithPrefix(prefix)
            .WithRecursive(true);

        var objectNames = new List<string>();

        await foreach (var item in _minioClient.ListObjectsEnumAsync(args))
        {
            if (item != null)
            {
                objectNames.Add(item.Key);
            }
        }

        if (!string.IsNullOrEmpty(filter.FilterBy) && !string.IsNullOrEmpty(filter.FilterValue))
        {
            objectNames = objectNames
                .Where(name => name.Contains(filter.FilterValue))
                .ToList();
        }

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            objectNames = filter.SortOrder?.ToLower() == "desc"
                ? objectNames.OrderByDescending(name => name).ToList()
                : objectNames.OrderBy(name => name).ToList();
        }

        objectNames = objectNames
            .Skip(filter.Start)
            .Take(filter.Limit)
            .ToList();

        return objectNames;
    }

}