using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Example.DurableFunctions;

public static class DurableFunctionsActivity
{

    [Function(nameof(Hello))]
    public static string Hello([ActivityTrigger] FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger(nameof(Hello));
        logger.LogInformation("DurableFunctionsActivity Hello says Hello!");
        return $"Hello!";
    }

    [Function(nameof(ReturnDataSize))]
    public static string ReturnDataSize([ActivityTrigger] FunctionContext executionContext, object context)
    {
        ILogger logger = executionContext.GetLogger(nameof(ReturnDataSize));
        logger.LogInformation("DurableFunctionsActivity ReturnDataSize Started");

        var ret = context.ToString();
        if (null == ret) { ret = "null"; };

        return ret;
    }
}