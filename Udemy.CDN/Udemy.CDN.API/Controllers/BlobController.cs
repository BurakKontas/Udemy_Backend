using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;

namespace Udemy.CDN.API.Controllers;

[Route("/v{version:apiVersion}/blob")]
[ApiController]
[ApiVersion("1.0")]
public class BlobController : ControllerBase
{
    [HttpGet("{blobId:guid}")]
    public async Task<IResult> GetBlob(Guid blobId)
    {
        //var result = await _blobService.GetByIdAsync(blobId);
        //return TypedResults.Ok(result);

        return TypedResults.Empty;
    }

    [HttpPost("upload")]
    public async Task<IResult> UploadBlob([FromBody] UploadBlobRequest request)
    {
        //var result = await _blobService.UploadAsync(request);
        //return TypedResults.Redirect($"get/{result}");

        return TypedResults.Empty;
    }

    [HttpDelete("{blobId:guid}")]
    public async Task<IResult> DeleteBlob(Guid blobId)
    {
        //await _blobService.DeleteAsync(blobId);
        return TypedResults.NoContent();
    }

    [HttpGet("download/{blobId:guid}")]
    public async Task<IResult> DownloadBlob(Guid blobId)
    {
        //var result = await _blobService.DownloadAsync(blobId);
        //return TypedResults.Ok(result);

        return TypedResults.Empty;
    }

    [HttpGet("list")]
    public async Task<IResult> ListBlobs(EndpointFilter filter)
    {
        //var result = await _blobService.ListAsync();
        //return TypedResults.Ok(result);

        return TypedResults.Empty;
    }

    [HttpGet("search")]
    public async Task<IResult> ListBlobs([FromQuery] string prefix, EndpointFilter filter)
    {
        //var result = await _blobService.ListAsync(container);
        //return TypedResults.Ok(result);

        return TypedResults.Empty;
    }
}

public record UploadBlobRequest(string Name, string ContentType, byte[] Data);