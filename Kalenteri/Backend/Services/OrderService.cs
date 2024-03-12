using Kalenteri.Backend.Models;
using Microsoft.Azure.Cosmos;

namespace Kalenteri.Backend.Services;

public static class OrderServices
{
    
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
        .Build();
    private static readonly string? EndpointUrl = Configuration["ConnectionStrings:EndpointUrl"];
    private static readonly string? PrimaryKey = Configuration["ConnectionStrings:PrimaryKey"];
    private static readonly string? DatabaseId = Configuration["ConnectionStrings:DatabaseId"];
    private static readonly string? ContainerId = Configuration["ConnectionStrings:ContainerId"];
    
    public static Order? GetData(string identifier)
    {
        Container container = CreateContainer();
        return container.GetItemLinqQueryable<Order>(true)
            .Where(o => o.Identifier == identifier)
            .ToList().FirstOrDefault();
    }


    public static Task<ItemResponse<Order>> UpdateData(Order order)
    {
        Container container = CreateContainer();
        return container.UpsertItemAsync(order);
    }

    private static Container CreateContainer()
    {
        return new CosmosClient(EndpointUrl, PrimaryKey)
            .GetDatabase(DatabaseId).GetContainer(ContainerId);
    }
    
}
