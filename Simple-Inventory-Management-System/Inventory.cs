using System.Diagnostics.CodeAnalysis;

class Inventory {

  [SetsRequiredMembers]
  public Inventory()
  {

  } 
  List<Product> Products { get; } = [];
  public void Add(Product product) {
    Products.Add(product);
  }
}