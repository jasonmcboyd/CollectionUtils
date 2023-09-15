using CollectionUtils.PSCmdlets;

namespace CollectionUtils.Test.CommandBuilders
{
  internal class ConvertToHashtableCommandBuilder : CommandBuilder<ConvertToHashtableCommandBuilder>
  {
    public ConvertToHashtableCommandBuilder() : base("ConvertTo-Hashtable")
    {
    }

    public ConvertToHashtableCommandBuilder Key(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(ConvertToHashtablePSCmdlet.Key), value, includeParameterName);

    public ConvertToHashtableCommandBuilder InputObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(ConvertToHashtablePSCmdlet.InputObject), value, includeParameterName);

    public ConvertToHashtableCommandBuilder Comparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(ConvertToHashtablePSCmdlet.Comparer), value, includeParameterName);

    public ConvertToHashtableCommandBuilder DefaultStringComparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(ConvertToHashtablePSCmdlet.DefaultStringComparer), value, includeParameterName);

    public ConvertToHashtableCommandBuilder AsLookup() =>
      AddCommandSwitch(nameof(ConvertToHashtablePSCmdlet.AsLookup));
  }
}
