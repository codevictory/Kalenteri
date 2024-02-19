
namespace Kalenteri.Backend.Models;

public class Order
{
    public string Identifier { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<Box> Boxes { get; set; } = new List<Box>();
}

public class Box
{
    public DateTime Delivery { get; set; }
    public DateTime? PickUp { get; set; }
}