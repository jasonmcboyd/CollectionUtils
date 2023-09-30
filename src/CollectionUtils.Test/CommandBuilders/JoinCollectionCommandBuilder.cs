using CollectionUtils.PSCmdlets;
using CollectionUtils.Test.Utils;
using System;

namespace CollectionUtils.Test.CommandBuilders
{
  internal class JoinCollectionCommandBuilder : CommandBuilder<JoinCollectionCommandBuilder>
  {
    public JoinCollectionCommandBuilder() : base("Join-Collection")
    {
    }

    public JoinCollectionCommandBuilder Left(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.Left), value, includeParameterName);

    public JoinCollectionCommandBuilder Right(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.Right), value, includeParameterName);

    public JoinCollectionCommandBuilder Key(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.Key), value, includeParameterName);

    public JoinCollectionCommandBuilder LeftKey(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.LeftKey), value, includeParameterName);

    public JoinCollectionCommandBuilder RightKey(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.RightKey), value, includeParameterName);

    public JoinCollectionCommandBuilder Comparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.Comparer), value, includeParameterName);

    public JoinCollectionCommandBuilder DefaultStringComparer(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.DefaultStringComparer), value, includeParameterName);

    public JoinCollectionCommandBuilder GroupJoinStrategy(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.GroupJoinStrategy), value, includeParameterName);

    public JoinCollectionCommandBuilder CrossJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.CrossJoin));

    public JoinCollectionCommandBuilder ZipJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.ZipJoin));

    public JoinCollectionCommandBuilder InnerJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.InnerJoin));

    public JoinCollectionCommandBuilder LeftJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.LeftJoin));

    public JoinCollectionCommandBuilder OuterJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.OuterJoin));

    public JoinCollectionCommandBuilder RightJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.RightJoin));

    public JoinCollectionCommandBuilder KeyedJoin(KeyedJoinType keyedJoinType)
    {
      switch (keyedJoinType)
      {
        case KeyedJoinType.Inner:
          return InnerJoin();
        case KeyedJoinType.Left:
          return LeftJoin();
        case KeyedJoinType.Outer:
          return OuterJoin();
        case KeyedJoinType.Right:
          return RightJoin();
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
