using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/// <summary>
/// Struct that contains all elements received from a command.
/// </summary>
readonly struct CommandArgs
{
[SetsRequiredMembers]
  public CommandArgs (string commandStr)
  {
    var commandStrArgs = commandStr.Split(' ');

    Action = GetCommandFromString(commandStrArgs[0]);
    ArgumentsMissing = GetMissingArgs(commandStrArgs);

    Name = GetStringFromArgs(commandStrArgs, MissingArgs.Name ,ArgumentsMissing);
    Price = GetIntFromArgs(commandStrArgs, MissingArgs.Price ,ArgumentsMissing);
    Quantity = GetIntFromArgs(commandStrArgs, MissingArgs.Quantity ,ArgumentsMissing);
    NewName = GetStringFromArgs(commandStrArgs, MissingArgs.NewName ,ArgumentsMissing);
  }

  public required Command Action { get; init; }
  public required string? Name { get; init; }
  public required int? Price { get; init; }
  public required int? Quantity { get; init; }
  public string? NewName { get; init; }
  /// <summary>
  /// Arguments not present in this command.
  /// </summary>
  public MissingArgs ArgumentsMissing { get; init; }

  public bool IsInvalid => Action switch
  {
    Command.Unknown => true,
    Command.Exit => false,
    Command.Add => !ArgumentsMissing.HasFlag(MissingArgs.NewName),
    Command.Edit => ArgumentsMissing.HasFlag(MissingArgs.Name) && (ArgumentsMissing & (ArgumentsMissing - 1)) == 0,
    Command.Delete => ArgumentsMissing.HasFlag(MissingArgs.Name),
    Command.Find => ArgumentsMissing.HasFlag(MissingArgs.Name),
    Command.View => true,
  };




  private static string? GetStringFromArgs(string[] strArgs, MissingArgs argToFind,MissingArgs missingArgs)
  {
    if (missingArgs.HasFlag(argToFind))
    {
      return null;
    }

    return argToFind switch
    {
      MissingArgs.Name => strArgs[1],
      MissingArgs.NewName => strArgs[4],
      _ => throw new ArgumentException(message: "Invalid argument for getting a string")
    };
  }

  private static int? GetIntFromArgs(string[] strArgs, MissingArgs argToFind,MissingArgs missingArgs)
  {
    if (missingArgs.HasFlag(argToFind))
    {
      return null;
    }
    string possibleInt;
    switch (argToFind)
    {
      case MissingArgs.Price: 
      {
        possibleInt = strArgs[2];
        break;
      }
      case MissingArgs.Quantity:
      {
        possibleInt = strArgs[3];
        break;
      }
      default: 
      {
        throw new ArgumentException(message: "Invalid argument for getting a string");
      }
    };

    return int.TryParse(possibleInt, out var result) ? result : null;

  }


  private static MissingArgs GetMissingArgs(string[] commandStrArgs)
  {
    var argsCount = commandStrArgs.Count();
    var missingArgs = MissingArgs.Name
    | MissingArgs.Price
    | MissingArgs.Quantity
    | MissingArgs.NewName;

    if (argsCount >= 2)
    {
      missingArgs ^= MissingArgs.Name;
    }
    if (argsCount >= 3)
    {
      missingArgs ^= MissingArgs.Price;
    }
    if (argsCount >= 4)
    {
      missingArgs ^= MissingArgs.Quantity;
    }
    if (argsCount >= 5)
    {
      missingArgs ^= MissingArgs.NewName;
    }

    return missingArgs;
  }



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