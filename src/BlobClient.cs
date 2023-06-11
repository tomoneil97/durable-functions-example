using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using HttpMultipartParser;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace Example.DurableFunctions;

public class BlobClient
{
    string? containerName = "uploads";
    string? Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    public Guid BlobUpload(HttpRequestData data)
    {

        Guid guid = Guid.NewGuid();
        var blobClient = new BlobContainerClient(Connection, containerName);
        blobClient.CreateIfNotExists();
        var blob = blobClient.GetBlobClient(guid.ToString());

        var parsedFormBody = MultipartFormDataParser.ParseAsync(data.Body);
        FilePart filePart = parsedFormBody.Result.Files[0];
        var file = new FilePartJson(filePart);
        var fileJson = JsonSerializer.Serialize(file);

        blob.Upload(new MemoryStream(Encoding.UTF8.GetBytes(fileJson)));

        return guid;
    }

    public FilePartJson BlobDownload(string guid) {
        var blobClient = new BlobContainerClient(Connection, containerName);
        var blob = blobClient.GetBlobClient(guid);
        var blobContent = blob.DownloadContent().Value.Content;
        FilePartJson? filePart = JsonSerializer.Deserialize<FilePartJson>(blobContent);
        return filePart;
    }
}

public sealed class objGuid {
    public string guid = "";

    public objGuid(Guid guid) {
        this.guid = guid.ToString();
    }
}   

public sealed class FilePartJson
{
    public string Name { get; set; }
    public string FileName { get; set; }
    public string Data { get; set; }
    public string ContentType { get; set; }
    public string ContentDisposition { get; set; }
    public IReadOnlyDictionary<string, string> AdditionalProperties { get; set; }
    public FilePartJson(FilePart fp)
    {
        this.Name = fp.Name;
        this.FileName = fp.FileName;
        this.ContentType = fp.ContentType;
        this.ContentDisposition = fp.ContentDisposition;
        this.AdditionalProperties = fp.AdditionalProperties;

        StreamReader reader = new StreamReader(fp.Data);
        Data = reader.ReadToEnd();
    }

    [JsonConstructor]
    public FilePartJson (string Name, string FileName, string Data, string ContentType, string ContentDisposition, IReadOnlyDictionary<string, string> AdditionalProperties) {
        this.Name = Name;
        this.FileName = FileName;
        this.Data = Data;
        this.ContentType = ContentType;
        this.ContentDisposition = ContentDisposition;
        this.AdditionalProperties = AdditionalProperties;
    }
}
