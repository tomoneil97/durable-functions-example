using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Example.DurableFunctions;

public static class DurableFunctionsClient {
    
    [Function(nameof(funcDurableFunctionsClient))]
    public static async Task<HttpResponseData> funcDurableFunctionsClient(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger(nameof(funcDurableFunctionsClient));

        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(DurableFunctionsOrchestration.funcDurableFunctionsOrchestration),req);
        logger.LogInformation("Created new orchestration with instance ID = {instanceId}", instanceId);

        return client.CreateCheckStatusResponse(req, instanceId);
    }
}