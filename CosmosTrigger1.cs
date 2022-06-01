using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public class CosmosTrigger1
    {
        private readonly ILogger _logger;

        public CosmosTrigger1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CosmosTrigger1>();
        }

        [Function("CosmosTrigger1")]
        [CosmosDBOutput(databaseName: "%OutputDatabase%", containerName: "%OutputContainerName%", Connection = "CosmosDBConnectionString", PartitionKey = "/Number", CreateIfNotExists = true)]
        public object? Run([CosmosDBTrigger(
            databaseName: "%InputDatabase%",
            containerName: "%InputContainerName%",
            CreateLeaseContainerIfNotExists = true,
            Connection = "CosmosDBConnectionString",
            LeaseContainerName = "leases")] IReadOnlyList<MyDocument> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + input.Count);
                _logger.LogInformation("First document Id: " + input[0].id);
            }

        
            return JsonConvert.SerializeObject(input);
        }
    }

    public class MyDocument
    {
        [JsonProperty("id")]
        public string? id { get; set; }

        public string? Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
