using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

class Inventory {

  [SetsRequiredMembers]
  public Inventory()
  {

  } 

  private readonly List<Product> products = [];
  public Product[] Products => [.. products];
  public void Add(Product product) {
    products.Add(product);
  }

  public bool IsProductAvailable(string name)
  {
    Predicate<Product> hasSameName = product => { return product.Name == name; };
    var productIndex =  products.FindIndex(hasSameName);
    return (productIndex == -1) ? false : true;
  }

  public bool Edit(string name, int? price, int? quantity, string? newName)
  {
    var productToEdit = GetProduct(name);

    if (productToEdit is null) return false;
    
    if (price.HasValue)
    {
      productToEdit.Price = (int)price;
    }

    if (quantity.HasValue)
    {
      productToEdit.Quantity = (int)quantity;
    }
    
    if (!string.IsNullOrWhiteSpace(newName))
    {
      productToEdit.Name = newName;
    }

    return true;
  }

  public bool Delete(string name)
  {
    var elementToDeleteIndex = products.FindIndex(product => product.Name == name);
    if (elementToDeleteIndex == -1) return false;

    products.RemoveAt(elementToDeleteIndex);
    return true;
  }

  public Product? GetProduct(string name)
  {
    if (!IsProductAvailable(name)) return null;

    return products.Find(product => product.Name == name);
  }
}