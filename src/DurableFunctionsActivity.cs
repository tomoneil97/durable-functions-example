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
    public static string ReturnDataSize([ActivityTrigger] FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger(nameof(ReturnDataSize));
        logger.LogInformation("DurableFunctionsActivity ReturnDataSize Started");

        string? guid = executionContext.BindingContext.BindingData.First(t => t.Key == "executionContext").Value.ToString();

        BlobClient blobClient = new BlobClient();
        FilePartJson filePartJson = blobClient.BlobDownload(guid);
        byte[] dataByteArray = Encoding.ASCII.GetBytes(filePartJson.Data);
        long byteSize = dataByteArray.Length;

        var ret = "The file " + filePartJson.FileName + " is " + ((byteSize / 1024) / 1024).ToString() + " MB large." ;
        return ret;
    }
}