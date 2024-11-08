using Microsoft.Azure.Functions.Worker.Http;

namespace Example.DurableFunctions;
public interface IBlobClient
{
    Guid BlobUpload(HttpRequestData data);
    FilePartJson BlobDownload(string guid);
}
