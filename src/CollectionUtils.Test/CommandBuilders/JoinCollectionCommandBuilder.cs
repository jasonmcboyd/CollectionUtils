using CollectionUtils.PSCmdlets;

namespace CollectionUtils.Test.CommandBuilders
{
  internal class JoinCollectionCommandBuilder : CommandBuilder<JoinCollectionCommandBuilder>
  {
    public JoinCollectionCommandBuilder() : base("Join-Collection")
    {
    }

    public static JoinCollectionCommandBuilder Command() => new JoinCollectionCommandBuilder();

    public JoinCollectionCommandBuilder LeftObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.LeftCollection), value, includeParameterName);

    public JoinCollectionCommandBuilder RightObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.RightCollection), value, includeParameterName);

    public JoinCollectionCommandBuilder Key(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.Key), value, includeParameterName);

    public JoinCollectionCommandBuilder LeftKey(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.LeftKey), value, includeParameterName);

    public JoinCollectionCommandBuilder RightKey(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.RightKey), value, includeParameterName);

    public JoinCollectionCommandBuilder InputObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.RightCollection), value, includeParameterName);

    public JoinCollectionCommandBuilder JoinType(JoinType value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.JoinType), value.ToString(), includeParameterName);

    public JoinCollectionCommandBuilder Comparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.Comparer), value, includeParameterName);

    public JoinCollectionCommandBuilder DefaultStringComparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.DefaultStringComparer), value, includeParameterName);

    public JoinCollectionCommandBuilder Flatten(string value, bool includeParameterName = true) =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.Flatten));
  }
}
