using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.CDN.Domain.Interfaces;
using Udemy.Common.ModelBinder;

namespace Udemy.CDN.API.Controllers;

[Route("/v{version:apiVersion}/blob")]
[ApiController]
[ApiVersion("1.0")]
public class BlobController(IMinioService minioService) : ControllerBase
{
    private readonly IMinioService _minioService = minioService;

    [HttpGet("{blobId}")]
    public async Task<IActionResult> GetBlob(string blobId, [FromQuery] string bucket = "udemy.default")
    {
        var result = await _minioService.DownloadFileAsync(bucket, blobId);

        var contentType = await _minioService.GetFileContentTypeAsync(bucket, blobId);

        if(string.IsNullOrEmpty(contentType)) contentType = "application/octet-stream";

        return new FileStreamResult(result, contentType);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("upload")]
    public async Task<IResult> UploadBlob([FromBody] UploadBlobRequest request, [FromQuery] string bucket = "udemy.default")
    {
        var randomId = Guid.NewGuid();
        var blobId = $"{request.Name}-{randomId}";
        var result = await _minioService.UploadFileAsync(bucket, request.Data, blobId, request.ContentType);
        return TypedResults.Redirect($"{blobId}");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("make")]
    public async Task<IResult> MakeBucket([FromQuery] string bucket)
    {
        var result = await _minioService.MakeBucketAsync(bucket);
        return TypedResults.Created();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{bucket}")]
    public async Task<IResult> DeleteBucket(string bucket)
    {
        await _minioService.DeleteBucketAsync(bucket);
        return TypedResults.NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{blobId}")]
    public async Task<IResult> DeleteBlob(string blobId, [FromQuery] string bucket = "udemy.default")
    {
        await _minioService.DeleteFileAsync(bucket, blobId);
        return TypedResults.NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<IResult> ListBlobs(EndpointFilter filter, [FromQuery] string bucket = "udemy.default")
    {
        var result = await _minioService.ListObjectsAsync(bucket, filter);

        return TypedResults.Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("search/{bucket}")]
    public async Task<IResult> ListBlobs(string bucket, EndpointFilter filter, [FromQuery] string prefix = "")
    {
        var result = await _minioService.ListObjectsByPrefixAsync(bucket, prefix, filter);

        return TypedResults.Ok(result);
    }

    [Authorize]
    [HttpPost("avatar")]
    public async Task<IResult> UploadAvatar([FromBody] UploadBlobRequest request, UserId userId)
    {
        var name = $"{userId}-avatar";
        var result = await _minioService.UploadFileAsync("udemy.avatars", request.Data, name, request.ContentType);
        
        return TypedResults.Redirect($"{name}");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("avatar/{userId:guid}")]
    public async Task<IResult> UploadAvatar([FromBody] UploadBlobRequest request, Guid userId)
    {
        var name = $"{userId}-avatar";
        var result = await _minioService.UploadFileAsync("udemy.avatars", request.Data, name, request.ContentType);

        return TypedResults.Redirect($"{name}");
    }
}

public record UploadBlobRequest(string Name, string ContentType, byte[] Data);