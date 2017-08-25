using System;
using System.Collections.Generic;

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

    public Dictionary<string, string> Options
    {
      get => _options;
      set => _options = value;
    }

    public Dictionary<string, object> Bin
    {
      get => _bin ?? (_bin = new Dictionary<string, object>());
      set => _bin = value;
    }

    public string TypeStr
    {
      get => _typeStr;
      set => _typeStr = value;
    }


    public List<string> StrFilter
    {
      get => _strFilter ?? (_strFilter = new List<string>());
      set => _strFilter = value;
    }


    public BCMAbstract(object obj, string typeStr, Dictionary<string, string> options, List<string> filters)
    {
      TypeStr = typeStr;
      Options = options;
      StrFilter = filters;
      // ReSharper disable once VirtualMemberCallInConstructor
      GetData(obj);
    }

    public Dictionary<string, object> Data() => Bin;

    public virtual void GetData(object obj)
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

    private string OptionValue(string key) => Options.ContainsKey(key) ? Options[key] : string.Empty;
  }
}
