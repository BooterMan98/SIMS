// See https://aka.ms/new-console-template for more information
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.VisualBasic;

Console.WriteLine("Runing Simple Inventory Management System!");
Inventory inventory = new Inventory();

Console.Title = "Simple Inventory Managment System";

while (true)
{
  var line = askforCommandString();
  var lineArgs = new CommandArgs(line);
  executeCommand(lineArgs);
}


string askforCommandString() 
{
  string? commandStr = null;

  while (String.IsNullOrEmpty(commandStr))
  {
    Console.WriteLine("""
    Enter one of the following commands to do something with the IMS:
    add <Name> <Price> <Quantity> - Add a product to the invetory.
    edit <name> <Price> <Quantity> <newName> - Edit an already existing product on the inventory
    delete <Name> - Delete a product from the inventory if it exists
    view - Display all products information on a list. 
    exit - Exit the program (you can also press ctrl-c).
    """);

    commandStr = Console.ReadLine();
    if (commandStr != null)
    {
      commandStr = commandStr.Trim();
    }

  }
  return commandStr!;
}

void executeCommand(CommandArgs commandArgs)
{
 switch (commandArgs.Action) {
    case Command.Add:
    {
       addProduct(commandArgs);
       break;
    }
    case Command.View:
      {
        ViewInventory();
        break;
      }
    case Command.Edit:
      {
        editProduct(commandArgs);
        break;
      }
    case Command.Delete:
      {
        deleteProduct(commandArgs);
        break;
      }
      case Command.Find:
      {
        findProduct(commandArgs);
        break;
      }
    case Command.Exit:
      {
        System.Environment.Exit(0);
        break;
      }
    default: break;
  } 
}

// It creates a product from the command arguments to add to the inventory.
// If there is an argument missing, it asks for it
void addProduct(CommandArgs commandArgs)
{
  if (commandArgs.Action != Command.Add || commandArgs.IsInvalid) return;
  Product product;
  if ( commandArgs.ArgumentsMissing == MissingArgs.None)
  {
    product = new Product(commandArgs.Name!, (int)commandArgs.Price!, (int)commandArgs.Quantity!);
    inventory.Add(product);
    return;
  }
  
  string name;
  int price;
  int quantity;
  string? input;
  if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Name))
  {
    do {
      Console.WriteLine("Enter Name:");
      input = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(input));
    name = input!;
  } else
  {
    name = commandArgs.Name!;
  }
  if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Price))
  {
    do {
      Console.WriteLine("Enter price:");
      input = Console.ReadLine();
    } while (!int.TryParse(input, out price));
  } else
  {
    price = (int)commandArgs.Price!;
  }
  if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.Quantity))
  {
    do 
    {
      Console.WriteLine("Enter quantity:");
      input = Console.ReadLine();
    } while (!int.TryParse(input, out quantity));
  } else
  {
    quantity = (int)commandArgs.Quantity!;
  }
  product = new Product(name, price, quantity);
  inventory.Add(product);
  Console.WriteLine("PThe product has been added to the invetory");
}

void editProduct(CommandArgs commandArgs)
{
  if (commandArgs.Action != Command.Edit || commandArgs.IsInvalid) return;

  if(!inventory.IsProductAvailable(commandArgs.Name!)) {
    Console.WriteLine("Product not found, maybe it was already deleted");
    return;
  };
  bool productModified = inventory.Edit(commandArgs.Name!, commandArgs.Price, commandArgs.Quantity, commandArgs.NewName);
  if (productModified)
  {
    Console.WriteLine("Product modified");
  } else
  {
    Console.WriteLine("For some reason, the product has not been modified");
  }
}

void deleteProduct(CommandArgs commandArgs)
{
  if (commandArgs.Action != Command.Delete || commandArgs.IsInvalid) return;

  if(!inventory.IsProductAvailable(commandArgs.Name!)) {
    Console.WriteLine("Product not found, maybe it was already deleted");
    return;
  };
  
  bool productDeleted =  inventory.Delete(commandArgs.Name!);
  if (productDeleted)
  {
    Console.WriteLine($"The product {commandArgs.Name} has been deleted");
  } else
  {
    Console.WriteLine("For some reason, the product couldn't be deleted.");
  }

}

void findProduct(CommandArgs commandArgs)
{
  if (commandArgs.Action != Command.Find && commandArgs.IsInvalid) return;

  var product = inventory.GetProduct(commandArgs.Name!);

  if (product == null)
  {
    Console.WriteLine("Product not found.");
    Console.WriteLine("---");
    return;
  }

  string productRowTemplate = "{0,-50}|{1,15:C}|{2,10}";
  string productRowMessage = string.Format(productRowTemplate, product.Name, product.Price, product.Quantity);
  Console.WriteLine("---");
  Console.WriteLine(productRowMessage);
  Console.WriteLine("---");

}

void ViewInventory()
{
  string headerRow = string.Format("{0,-50}|{1,15}|{2,10}", "Name", "Price", "Quantity");
  string productRowTemplate = "{0,-50}|{1,15:C}|{2,10}";
  Console.WriteLine(headerRow);
  Console.WriteLine("---");

  foreach (var product in inventory.Products)
  {
    string productRowMessage = string.Format(productRowTemplate, product.Name, product.Price, product.Quantity);
    Console.WriteLine(productRowMessage);
  }
    Console.WriteLine("---");

}