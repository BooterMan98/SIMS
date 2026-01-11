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

  public bool Edit(CommandArgs commandArgs)
  {
    if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Name)) return false;

    var productToEdit = products.Find(product => product.Name == commandArgs.Name);

    var noArgsToEdit = MissingArgs.NewName | MissingArgs.Price | MissingArgs.Quantity;
    if (commandArgs.ArgumentsMissing == noArgsToEdit | productToEdit == null) return false;
    
    if (!commandArgs.ArgumentsMissing.HasFlag(MissingArgs.NewName))
    {
      productToEdit!.Name = commandArgs.NewName!;
    }

    if (!commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Price))
    {
      productToEdit!.Price = (int)commandArgs.Price!;
    }
    if (!commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Quantity))
    {
      productToEdit!.Quantity = (int)commandArgs.Quantity!;
    }
    return true;
  }

  public bool Delete(CommandArgs commandArgs)
  {
    if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Name)) return false;

    var elementToDeleteIndex = products.FindIndex(product => product.Name == commandArgs.Name);
    if (elementToDeleteIndex == -1) return false;

    products.RemoveAt(elementToDeleteIndex);
    return true;
  }

  public Product? GetProduct(CommandArgs commandArgs)
  {
    if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Name)) return null;

    if (!IsProductAvailable(commandArgs.Name!)) return null;

    return products.Find(product => product.Name == commandArgs.Name!);
  }
}