using CollectionUtils.Exceptions;
using System.Collections;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class KeySelector
  {
    public KeySelector(KeyField[] keyFields)
    {
      _KeyFields = keyFields;
    }

    private readonly KeyField[] _KeyFields;
    
    public object GetKey(object obj) =>
      _KeyFields.Length == 1
      ? GetKeyAsObject(obj)
      : GetKeyAsHashtable(obj);

    private object GetKeyAsObject(object obj)
    {
      var key = PropertyGetter.GetProperty(obj, _KeyFields[0]) ?? throw new NullKeyException(obj);

      if (key is PSObject psObject)
        key = psObject.BaseObject;

      return key;
    }

    private object GetKeyAsHashtable(object obj)
    {
      var result = new Hashtable();

      for (int i = 0; i < _KeyFields.Length; i++)
      {
        var keyField = _KeyFields[i];

        var propertyValue =
          PropertyGetter.GetProperty(obj, keyField)
          ?? throw new NullKeyException(obj);

        result.Add(keyField.Property, propertyValue);
      }

      return result;
    }
  }
}
