﻿using CollectionUtils.JoinCommandHandlers;
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

    public JoinCollectionCommandBuilder KeyCollisionPreference(string value, bool includeParameterName = true) =>
      AddCommandParameter(nameof(JoinCollectionPsCmdlet.KeyCollisionPreference), value, includeParameterName);

    public JoinCollectionCommandBuilder CrossJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.CrossJoin));

    public JoinCollectionCommandBuilder ZipJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.ZipJoin));

    public JoinCollectionCommandBuilder DisjunctJoin() =>
      AddCommandSwitch(nameof(JoinCollectionPsCmdlet.DisjunctJoin));

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
      return keyedJoinType switch
      {
        KeyedJoinType.Disjunct => DisjunctJoin(),
        KeyedJoinType.Inner => InnerJoin(),
        KeyedJoinType.Left => LeftJoin(),
        KeyedJoinType.Outer => OuterJoin(),
        KeyedJoinType.Right => RightJoin(),
        _ => throw new InvalidOperationException(),
      };
    }
  }
}
