using System;
using System.Collections;
using System.Management.Automation;

namespace CollectionUtils
{
  internal interface IHashtableBuilder : IDisposable
  {
    int Count { get; }

    void AddObject(PSObject obj);
    void AddObjects(PSObject[] objects);
    Hashtable GetHashtable();
  }
}