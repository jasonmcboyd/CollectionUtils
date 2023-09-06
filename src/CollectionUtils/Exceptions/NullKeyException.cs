using System;
using System.Text.Json;

namespace CollectionUtils.Exceptions
{
  internal class NullKeyException : Exception
  {
    public NullKeyException(object obj) : base($"Key is null for object:\r\n{JsonSerializer.Serialize(obj)}")
    {
    }
  }
}
