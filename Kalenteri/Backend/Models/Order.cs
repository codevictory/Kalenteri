
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Kalenteri.Backend.Models;

[Alias("kalenteri_order")] 
public class Order
{
    public Guid id { get; set; } = Guid.Empty;
    public string Identifier { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    [Reference]
    public List<Box> Boxes { get; set; } = new List<Box>();
}

[Alias("kalenteri_box")]
public class Box
{
    public Guid id { get; set; } = Guid.Empty;

    [Alias("order_id")] 
    public Guid OrderId { get; set; }
    
    [Alias("delivery_time")] 
    public DateTime Delivery { get; set; }
    
    [Alias("pickup_time")] 
    public DateTime? PickUp { get; set; }
}