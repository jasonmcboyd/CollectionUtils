using System.Linq.Expressions;

namespace CollectionUtils.Test.CommandBuilders
{
  internal static class PSBuilder
  {
    public static ConvertFromDataTableCommandBuilder ConvertFromDataTable() => new ConvertFromDataTableCommandBuilder();

    public static ConvertToHashtableCommandBuilder ConvertToHashTable() => new ConvertToHashtableCommandBuilder();

    public static JoinObjectCommandBuilder JoinObject() => new JoinObjectCommandBuilder();

    public static string KeyField(string property, string expression) =>
      $"@{{ {nameof(CollectionUtils.KeyField.Property)} = '{property}'; {nameof(CollectionUtils.KeyField.Expression)} = {expression} }}";
  }
}
