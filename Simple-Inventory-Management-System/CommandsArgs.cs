using System.Diagnostics.CodeAnalysis;

readonly struct CommandArgs
{
[SetsRequiredMembers]
  public CommandArgs (string commandStr)
  {
    var commandStrArgs = commandStr.Split(' ');

    Action = GetCommandFromString(commandStrArgs[0]);

    var argsCount = commandStrArgs.Count();
    var missingArgs = MissingArgs.name 
    | MissingArgs.price 
    | MissingArgs.quantity 
    | MissingArgs.newName;

    if (argsCount >= 2)
    {
      string potentialName = commandStrArgs[1];
      Name = potentialName;
      missingArgs ^= MissingArgs.name;
    }
    if (argsCount >= 3)
    {
      if (int.TryParse(commandStrArgs[2], out int potentialPrice))
      {
        Price = potentialPrice;
        missingArgs ^= MissingArgs.price;
      }
    }
    if (argsCount >= 4)
    {
      if (int.TryParse(commandStrArgs[3], out int potentialQuantity))
      {
        Quantity = potentialQuantity;
        missingArgs ^= MissingArgs.quantity;
      }
    }
    if (argsCount >= 5)
    {
      string potentialNewName = commandStrArgs[4];
      NewName = potentialNewName;
      missingArgs ^= MissingArgs.newName;
    }


    ArgumentsMissing = missingArgs;

    switch (Action) {
      case Command.Add:
        {
          break;
        }
      case Command.Exit or Command.Unknown: 
        {
          break;
        }

    }

  }


  public required Command Action { get; init; }
  public required string? Name { get; init; }
  public required int? Price { get; init; }
  public required int? Quantity { get; init; }
  public string? NewName { get; init; }
  public MissingArgs ArgumentsMissing { get; init; }


  private static Command GetCommandFromString (string commandStr)
  {
    string lowerCaseCommand = commandStr.ToLower();

    if (lowerCaseCommand == "add") return Command.Add;
    if (lowerCaseCommand == "view") return Command.View;
    if (lowerCaseCommand == "edit") return Command.Edit;
    if (lowerCaseCommand == "delete") return Command.Delete;
    if (lowerCaseCommand == "find") return Command.Find;
    if (lowerCaseCommand == "exit") return Command.Exit;
    return Command.Unknown;
    

  }

}