using System;
using System.Text.Json;

namespace CollectionUtils.Exceptions
{
  public class DuplicateKeyException : Exception
  {
    public DuplicateKeyException(object key, object obj) : base($"Item has already been added:\r\n\tKey\r\n:{JsonSerializer.Serialize(key)}\r\n\t:Object:\r\n{JsonSerializer.Serialize(obj)}")
    {
    }
  }
}
