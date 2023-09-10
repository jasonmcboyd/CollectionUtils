using CollectionUtils.Test.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CollectionUtils.Test.CommandBuilders
{
  internal abstract class CommandBuilder<T> where T : CommandBuilder<T>
  {
    public CommandBuilder(string command)
    {
      AddCommand(command);
    }

    public string? PipedCommand { get; set; }

    private List<CommandPart> CommandParts { get; } = new List<CommandPart>();

    private string CommandPartToString(CommandPart commandPart)
    {
      if (!commandPart.IncludeParameterName)
        return commandPart.Value ?? throw new ArgumentException("Command part cannot have no paramter and no value.");
      else if (string.IsNullOrEmpty(commandPart.Value))
        return "-" + commandPart.Parameter;
      else
        return $"-{commandPart.Parameter} {commandPart.Value}";
    }

    private T AddCommand(string command) => AddCommandParameter("", command, false);

    protected T AddCommandParameter(string parameter, string? value, bool includeParameterName = true)
    {
      CommandParts.Add(new CommandPart(parameter, value, includeParameterName));

      return (T)this;
    }

    protected T AddCommandSwitch(string parameter) => AddCommandParameter(parameter, null);

    public override string ToString()
    {
      var result =
        CommandParts
        .Select(commandPart => CommandPartToString(commandPart))
        .JoinStrings(" ");

      return
        string.IsNullOrEmpty(PipedCommand)
        ? result
        : $"{PipedCommand} | {result}";
    }

    private struct CommandPart
    {
      public CommandPart(
        string parameter,
        string? value,
        bool includeParameterName = true)
      {
        Parameter = parameter;
        Value = value;
        IncludeParameterName = includeParameterName;
      }

      public string Parameter { get; }
      public string? Value { get; }
      public bool IncludeParameterName { get; }
    }
  }
}
