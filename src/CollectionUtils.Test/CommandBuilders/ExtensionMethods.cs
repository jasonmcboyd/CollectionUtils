namespace CollectionUtils.Test.CommandBuilders
{
  internal static class ExtensionMethods
  {
    public static T Pipe<T>(this string pipedCommand) where T : CommandBuilder<T>, new()
    {
      var result = new T();

      result.PipedCommand = pipedCommand;

      return result;
    }

    public static TResult Pipe<TPipedCommand, TResult>(this TPipedCommand pipedCommand)
      where TPipedCommand : CommandBuilder<TPipedCommand>
      where TResult : CommandBuilder<TResult>, new()
    {
      var result = new TResult();

      result.PipedCommand = pipedCommand.ToString();

      return result;
    }
  }
}
