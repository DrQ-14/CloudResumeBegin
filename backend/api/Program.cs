using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddSingleton(sp =>
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDbConnection");

            return new CosmosClient(connectionString);
        });
    })
    .Build();

host.Run();
