using Kalenteri.Backend.Models;
using Microsoft.Azure.Cosmos;

namespace Kalenteri.Backend.Services;

public class OrderServices
{
    private const string EndpointUrl = "https://kalentericosmosdbtest.documents.azure.com:443/";
    private const string PrimaryKey = "ON6nEfIrVtiQduJfdh9Bc0Xp6dPHCnAWdmmsvnnxvNRKj2PzH7IaPmGnQrwSJBrmBntNJCz5M8ymACDbeQsQ2w==";
    private const string DatabaseId = "kalenteri";
    private const string ContainerId = "cont1";

    private static Order _order = null; 
    
    public static async Task<Order> getData(String identifier)
    {
        if (_order == null)
        {
            CosmosClient cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
            Database database = cosmosClient.GetDatabase(DatabaseId);
            Container container = database.GetContainer(ContainerId);
              
            var order = new Order();
            var query = new QueryDefinition("SELECT * FROM o WHERE o.identifier= @id")
                .WithParameter("@id", identifier);

            var iterator = container.GetItemQueryIterator<dynamic>(query);

            while (iterator.HasMoreResults)
            {
                FeedResponse<dynamic> response = await iterator.ReadNextAsync();
                foreach (var item in response)
                {
                    order.Identifier = item.identifier;
                    order.Location = item.location;
                    foreach (var box in item.boxes)
                    {
                        order.Boxes.Add(new Box {Delivery = box.delivery, PickUp = box.pickup});
                    }
                }
            }

            _order = order;
        }

        return _order;
    }
}
