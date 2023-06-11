using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

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
    public static string ReturnDataSize([ActivityTrigger] FunctionContext executionContext, string fileJson)
    {
        ILogger logger = executionContext.GetLogger(nameof(ReturnDataSize));
        logger.LogInformation("DurableFunctionsActivity ReturnDataSize Started");

        var filePart = JsonSerializer.Deserialize<FilePartJson>(fileJson);
        byte[] dataByteArray = Encoding.ASCII.GetBytes(filePart.Data);
        long byteSize = dataByteArray.Length;

        var ret = "The file " + filePart.FileName + " is " + ((byteSize / 1024) / 1024).ToString() + " MB large." ;
        return ret;
    }
}