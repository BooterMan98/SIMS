using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

class Inventory {

  [SetsRequiredMembers]
  public Inventory()
  {

  } 
  public List<Product> Products { get; } = [];
  public void Add(Product product) {
    Products.Add(product);
  }

  public bool IsProductAvailable(string name)
  {
    Predicate<Product> hasSameName = product => { return product.Name == name; };
    var productIndex =  Products.FindIndex(hasSameName);
    return (productIndex == -1) ? false : true;
  }

  public bool Edit(CommandArgs commandArgs)
  {
    if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.name)) return false;

    var productToEdit = Products.Find(product => product.Name == commandArgs.Name);

    var noArgsToEdit = MissingArgs.newName | MissingArgs.price | MissingArgs.quantity;
    if (commandArgs.ArgumentsMissing == noArgsToEdit | productToEdit == null) return false;
    
    if (!commandArgs.ArgumentsMissing.HasFlag(MissingArgs.newName))
    {
      productToEdit!.Name = commandArgs.NewName!;
    }

    if (!commandArgs.ArgumentsMissing.HasFlag(MissingArgs.price))
    {
      productToEdit!.Price = (int)commandArgs.Price!;
    }
    if (!commandArgs.ArgumentsMissing.HasFlag(MissingArgs.quantity))
    {
      productToEdit!.Quantity = (int)commandArgs.Quantity!;
    }
    return true;
  }
}