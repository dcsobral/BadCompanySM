using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  [Serializable]
  public class BCMAbstract
  {
    [NonSerialized]
    private Dictionary<string, string> _options;
    [NonSerialized]
    private Dictionary<string, object> _bin;
    [NonSerialized]
    private string _typeStr;
    [NonSerialized]
    private List<string> _strFilter;

    [NotNull]
    protected Dictionary<string, string> Options
    {
      get => _options;
      private set => _options = value;
    }

    protected Dictionary<string, object> Bin
    {
      get => _bin ?? (_bin = new Dictionary<string, object>());
      set => _bin = value;
    }

    protected string TypeStr
    {
      get => _typeStr;
      private set => _typeStr = value;
    }

    [NotNull]
    protected List<string> StrFilter
    {
      get => _strFilter ?? (_strFilter = new List<string>());
      private set => _strFilter = value;
    }

    protected BCMAbstract(object obj, string typeStr, [NotNull] Dictionary<string, string> options, [CanBeNull] List<string> filters)
    {
      TypeStr = typeStr;
      Options = options;
      StrFilter = filters ?? new List<string>();
      // ReSharper disable once VirtualMemberCallInConstructor
      GetData(obj);
    }

    public Dictionary<string, object> Data() => Bin;

    protected virtual void GetData(object obj)
    {
      //Use the GetData override method in the derived class to populate the Bin property
    }

    internal bool IsOption(string key, bool isOr = true)
    {
      var keys = key.Split(' ');
      if (keys.Length <= 1) return Options.ContainsKey(key);

      var o = !isOr;
      foreach (var k in keys)
      {
        if (isOr)
          o |= Options.ContainsKey(k);
        else
          o &= Options.ContainsKey(k);
      }

      return o;
    }

    //private string OptionValue(string key) => Options.ContainsKey(key) ? Options[key] : string.Empty;
  }
}
