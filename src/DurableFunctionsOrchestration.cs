using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using HttpMultipartParser;

namespace Example.DurableFunctions;

public static class DurableFunctionsOrchestration
{
    [Function(nameof(funcDurableFunctionsOrchestration))]
    public static async Task<string> funcDurableFunctionsOrchestration([OrchestrationTrigger] TaskOrchestrationContext context, string guid )
    {
        string result = "";
        result += await context.CallActivityAsync<string>(nameof(DurableFunctionsActivity.Hello)) + " ";
        result += await context.CallActivityAsync<string>(nameof(DurableFunctionsActivity.ReturnDataSize), guid) + " ";
        return result;
    }
}