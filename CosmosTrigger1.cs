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
        [CosmosDBOutput(databaseName: "%OutputDatabase%", containerName: "%OutputContainerName%", Connection = "CosmosDBConnectionString", PartitionKey = "/id", CreateIfNotExists = true)]
        public object? Run([CosmosDBTrigger(
            databaseName: "%InputDatabase%",
            containerName: "%InputContainerName%",
            CreateLeaseContainerIfNotExists = true,
            Connection = "CosmosDBConnectionString",
            LeaseContainerName = "leases")] IReadOnlyList<Order> input,
            FunctionContext context)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents count: " + input.Count);
               _logger.LogInformation($"document : {JsonConvert.SerializeObject(input[0])}");
            }
            return input;
        }
    }

    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public string? id { get; set; }
        public string? customer { get; set; }
        public IList<OrderDetail>? detail { get; set; }
        public bool delete { get; set; }

        public class OrderDetail
        {
            public decimal unitprice { get; set; }
            public string? productName { get; set; }
            public int quantity { get; set; }
            public decimal discount { get; set; }
            public string? vendor { get; set; }
        }
    }
}
