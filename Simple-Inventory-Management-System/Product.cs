using System.Diagnostics.CodeAnalysis;

[method: SetsRequiredMembers]
class Product(string name, int price, int quantity)
{
  public required string Name { get; set; } = name;
  public required int Price { get; set; } = price;
  public required int Quantity { get; set; } = quantity;
}
