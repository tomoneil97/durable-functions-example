using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using HttpMultipartParser;


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

        BlobClient blobClient = new BlobClient();
        Guid guid = blobClient.BlobUpload(req);
        string guidStr = guid.ToString();
        
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(DurableFunctionsOrchestration.funcDurableFunctionsOrchestration), guidStr);
        logger.LogInformation("Created new orchestration with instance ID = {instanceId}", instanceId);

        return client.CreateCheckStatusResponse(req, instanceId);
    }
}