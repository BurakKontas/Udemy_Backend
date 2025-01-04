using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    public async Task<IActionResult> GetBlob(string blobId, [FromQuery] string? bucket = "udemy.default")
    {
        var fileStream = await _minioService.DownloadFileAsync(bucket!, blobId);
        var contentType = await _minioService.GetFileContentTypeAsync(bucket!, blobId);

        if (fileStream.Length == 0)
            return NotFound();

        if (string.IsNullOrEmpty(contentType))
            return NotFound();

        return new FileStreamResult(fileStream, contentType);
    }

    [HttpGet("download/{blobId}")]
    public async Task<IActionResult> DownloadFile(string blobId, [FromQuery] string? bucket = "udemy.default")
    {
        var fileStream = await _minioService.DownloadFileAsync(bucket!, blobId);
        var contentType = await _minioService.GetFileContentTypeAsync(bucket!, blobId);

        var fileExtension = contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "application/pdf" => ".pdf",
            "application/zip" => ".zip",
            _ => Path.GetExtension(blobId)
        };

        var fileName = $"{blobId}{fileExtension}";

        if (fileStream.Length == 0)
            return NotFound();

        if (string.IsNullOrEmpty(contentType))
            return NotFound();

        return File(fileStream, contentType, fileName);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("upload")]
    public async Task<IResult> UploadBlob([FromBody] UploadBlobRequest request, [FromQuery] string? bucket = "udemy.default")
    {
        var randomId = Guid.NewGuid();
        var blobId = $"{request.Name}-{randomId}";
        var result = await _minioService.UploadFileAsync(bucket!, request.Data, blobId, request.ContentType);
        if(result) return TypedResults.Ok(blobId);
        return TypedResults.Conflict();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("make")]
    public async Task<IResult> MakeBucket([FromQuery] string? bucket = "udemy.default")
    {
        var result = await _minioService.MakeBucketAsync(bucket!);
        if(result) return TypedResults.Created();
        return TypedResults.Conflict();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{bucket}")]
    public async Task<IResult> DeleteBucket(string bucket)
    {
        var result = await _minioService.DeleteBucketAsync(bucket);
        if(result) return TypedResults.NoContent();
        return TypedResults.Conflict();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{blobId}")]
    public async Task<IResult> DeleteBlob(string blobId, [FromQuery] string? bucket = "udemy.default")
    {
        var result = await _minioService.DeleteFileAsync(bucket!, blobId);
        if(result) return TypedResults.NoContent();
        return TypedResults.Conflict();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<IResult> ListBlobs(EndpointFilter filter, [FromQuery] string? bucket = "udemy.default")
    {
        var result = await _minioService.ListObjectsAsync(bucket!, filter);

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

    [Authorize(Roles = "Admin")]
    [HttpGet("list/bucket")]
    public async Task<IResult> ListBuckets(EndpointFilter filter)
    {
        var result = await _minioService.ListBucketsAsync(filter);

        return TypedResults.Ok(result);
    }
}

public record UploadBlobRequest(string Name, string ContentType, [property: JsonConverter(typeof(Base64Converter))] byte[] Data);

public class Base64Converter : JsonConverter<byte[]>
{
    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var base64String = reader.GetString();
        var cleanBase64 = base64String!.Contains(",")
            ? base64String.Split(',')[1]
            : base64String;
        return Convert.FromBase64String(cleanBase64);
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Convert.ToBase64String(value));
    }
}