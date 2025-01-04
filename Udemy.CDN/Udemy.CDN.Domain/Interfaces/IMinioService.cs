using Udemy.Common.ModelBinder;

namespace Udemy.CDN.Domain.Interfaces;

public interface IMinioService
{
    Task<bool> BucketExistsAsync(string bucketName);
    Task<bool> MakeBucketAsync(string bucketName);
    Task<bool> UploadFileAsync(string bucketName, string filePath, string objectName, string contentType = "application/octet-stream");
    Task<bool> UploadFileAsync(string bucketName, byte[] fileData, string objectName, string contentType = "application/octet-stream");
    Task<Stream> DownloadFileAsync(string bucketName, string objectName);
    Task<List<string>> ListBucketsAsync(EndpointFilter filter);
    Task<List<string>> ListObjectsAsync(string bucketName, EndpointFilter filter);
    Task<bool> DeleteFileAsync(string bucketName, string objectName);
    Task<bool> DeleteFilesAsync(string bucketName, List<string> objectNames);
    Task<bool> DeleteBucketAsync(string bucketName);
    Task<string> GeneratePreSignedUrlAsync(string bucketName, string objectName, int expiryInSeconds);
    Task<IDictionary<string, string>> GetFileMetadataAsync(string bucketName, string objectName);
    Task<string> GetFileContentTypeAsync(string bucketName, string objectName);
    Task<List<string>> ListObjectsByPrefixAsync(string bucketName, string prefix, EndpointFilter filter);
}