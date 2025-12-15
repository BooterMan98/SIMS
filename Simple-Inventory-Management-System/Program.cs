// See https://aka.ms/new-console-template for more information
using System.Diagnostics.CodeAnalysis;
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
    case Command.add:
    {
       addProduct(commandArgs);
       break;
    }
    case Command.view:
      {
        ViewInventory();
        break;
      }
    case Command.exit:
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
  Product product;
  if ( commandArgs.ArgumentsMissing == MissingArgs.none)
  {
    product = new Product(commandArgs.Name!, (int)commandArgs.Price!, (int)commandArgs.Quantity!);
    inventory.Add(product);
    return;
  }
  
  string name;
  int price;
  int quantity;
  string? input;
  if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.name))
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
  if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.price))
  {
    do {
      Console.WriteLine("Enter price:");
      input = Console.ReadLine();
    } while (!int.TryParse(input, out price));
  } else
  {
    price = (int)commandArgs.Price!;
  }
  if (commandArgs.ArgumentsMissing.HasFlag(MissingArgs.quantity))
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