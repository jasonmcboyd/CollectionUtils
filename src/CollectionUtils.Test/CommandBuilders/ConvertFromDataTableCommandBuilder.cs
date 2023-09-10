using CollectionUtils.PSCmdlets;

namespace CollectionUtils.Test.CommandBuilders
{
  internal class ConvertFromDataTableCommandBuilder : CommandBuilder<ConvertFromDataTableCommandBuilder>
  {
    public ConvertFromDataTableCommandBuilder() : base("ConvertFrom-DataTable")
    {
    }

    public static ConvertFromDataTableCommandBuilder Command() => new ConvertFromDataTableCommandBuilder();

    public ConvertFromDataTableCommandBuilder Key(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(ConvertFromDataTablePSCmdlet.Table), value, includeParameterName);

    public ConvertFromDataTableCommandBuilder InputObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(ConvertFromDataTablePSCmdlet.Row), value, includeParameterName);
  }
}
