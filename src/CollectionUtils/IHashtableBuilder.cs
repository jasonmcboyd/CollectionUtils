using System;
using System.Collections;

namespace CollectionUtils
{
  internal interface IHashtableBuilder : IDisposable
  {
    void AddObject(object obj);
    void AddObjects(object[] objects);
    Hashtable GetHashtable();
  }
}