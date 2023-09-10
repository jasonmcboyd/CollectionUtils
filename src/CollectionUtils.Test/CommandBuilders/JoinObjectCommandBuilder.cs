using CollectionUtils.PSCmdlets;

namespace CollectionUtils.Test.CommandBuilders
{
  internal class JoinObjectCommandBuilder : CommandBuilder<JoinObjectCommandBuilder>
  {
    public JoinObjectCommandBuilder() : base("Join-Object")
    {
    }

    public static JoinObjectCommandBuilder Command() => new JoinObjectCommandBuilder();

    public JoinObjectCommandBuilder LeftObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.LeftObject), value, includeParameterName);

    public JoinObjectCommandBuilder RightObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.RightObject), value, includeParameterName);

    public JoinObjectCommandBuilder Key(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.Key), value, includeParameterName);

    public JoinObjectCommandBuilder LeftKey(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.LeftKey), value, includeParameterName);

    public JoinObjectCommandBuilder RightKey(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.RightKey), value, includeParameterName);

    public JoinObjectCommandBuilder InputObject(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.RightObject), value, includeParameterName);

    public JoinObjectCommandBuilder JoinType(JoinType value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.JoinType), value.ToString(), includeParameterName);

    public JoinObjectCommandBuilder Comparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.Comparer), value, includeParameterName);

    public JoinObjectCommandBuilder DefaultStringComparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinObjectPSCmdlet.DefaultStringComparer), value, includeParameterName);

    public JoinObjectCommandBuilder Flatten(string value, bool includeParameterName = true) =>
      AddCommandSwitch(nameof(JoinObjectPSCmdlet.Flatten));
  }
}
