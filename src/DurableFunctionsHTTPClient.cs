using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using HttpMultipartParser;
using System.Text.Json;

namespace Example.DurableFunctions;

public static class DurableFunctionsClient
{

    [Function(nameof(funcDurableFunctionsClient))]
    public static async Task<HttpResponseData> funcDurableFunctionsClient(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger(nameof(funcDurableFunctionsClient));


        var testvalue = executionContext.BindingContext.BindingData["testFileUpload"];
        var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
        FilePart filePart = parsedFormBody.Result.Files[0];

        var file = new FilePartJson(filePart);

        var fileJson = JsonSerializer.Serialize(file);


        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(DurableFunctionsOrchestration.funcDurableFunctionsOrchestration), fileJson);
        logger.LogInformation("Created new orchestration with instance ID = {instanceId}", instanceId);

        return client.CreateCheckStatusResponse(req, instanceId);

        // catch {
        //     return req.CreateResponse(HttpStatusCode.InternalServerError);
        // }
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
    }

