using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMItemClass
  {
    private Dictionary<string, string> options = new Dictionary<string, string>();
    private Dictionary<string, object> _bin = new Dictionary<string, object>();
    public enum Filters
    {
      Id,
      Name,
      LocalizedName,
      Properties
    }
    public static class StrFilters
    {
      public static string Id = "id";
      public static string Name = "name";
      public static string LocalizedName = "local";
      public static string Properties = "props";
    }

    private List<string> _filter = new List<string>();

    #region Properties
    public int Id;
    public string Name;
    public string LocalizedName;
    public Dictionary<string, string> Properties = new Dictionary<string, string>();
    #endregion;

    private bool useInt = false;
    private List<string> strFilter = new List<string>();
    private List<int> intFilter = new List<int>();
    
    public BCMItemClass(ItemClass _item, Dictionary<string, string> _options)
    {
      options = _options;
      Load(_item);
    }

    public Dictionary<string, object> Data ()
    {
      return _bin;
    }

    public void Load(ItemClass _item)
    {
      if (isOption("filter"))
      {
        strFilter = OptionValue("filter").Split(',').ToList();
        if (strFilter.Count > 0)
        {
          intFilter = (from o in strFilter.Where((o) => { int d; return int.TryParse(o, out d); }) select int.Parse(o)).ToList();
          if (intFilter.Count == strFilter.Count)
          {
            useInt = true;
          }
        }
      }

      GetItemInfo(_item);

    }

    private void GetItemInfo(ItemClass _item)
    {
      if (isOption("filter"))
      {
        //ID
        if ((useInt && intFilter.Contains((int)Filters.Id)) || (!useInt && strFilter.Contains(StrFilters.Id)))
        {
          Id = _item.Id;
          _bin.Add("Id", Id);
        }

        //NAME
        if ((useInt && intFilter.Contains((int)Filters.Name)) || (!useInt && strFilter.Contains(StrFilters.Name)))
        {
          Name = _item.Name;
          _bin.Add("Name", Name);
        }

        //LOCAL NAME
        if ((useInt && intFilter.Contains((int)Filters.LocalizedName)) || (!useInt && strFilter.Contains(StrFilters.LocalizedName)))
        {
          LocalizedName = _item.localizedName;
          _bin.Add("LocalizedName", LocalizedName);
        }

        //PROPERTIES
        if ((useInt && intFilter.Contains((int)Filters.Properties)) || (!useInt && strFilter.Contains(StrFilters.Properties)))
        {
          foreach (string current in _item.Properties.Values.Keys)
          {
            if (_item.Properties.Values.ContainsKey(current))
            {
              Properties.Add(current, _item.Properties.Values[current]);
            }
          }
          _bin.Add("Properties", Properties);
        }


      }
      else
      {
        Id = _item.Id;
        Name = _item.Name;
        LocalizedName = _item.localizedName;
        //properties
        foreach (string current in _item.Properties.Values.Keys)
        {
          if (_item.Properties.Values.ContainsKey(current))
          {
            Properties.Add(current, _item.Properties.Values[current]);
          }
        }

        _bin.Add("Id", Id);
        _bin.Add("Name", Name);
        _bin.Add("LocalizedName", LocalizedName);
        _bin.Add("Properties", Properties);
      }
    }

    public bool isOption(string key, bool isOr = true)
    {
      var _o = !isOr;
      var keys = key.Split(' ');
      if (keys.Length > 1)
      {
        foreach (var k in keys)
        {
          if (isOr)
          {
            _o |= options.ContainsKey(k);
          }
          else
          {
            _o &= options.ContainsKey(k);
          }
        }
        return _o;
      }
      else
      {
        return options.ContainsKey(key);
      }
    }

    public string OptionValue(string key)
    {
      if (options.ContainsKey(key))
      {
        return options[key];
      }
      return "";
    }

    public string GetPosType()
    {
      var postype = "";
      if (options.ContainsKey("worldpos"))
      {
        postype = "worldpos";
      }
      if (options.ContainsKey("csvpos"))
      {
        postype = "csvpos";
      }

      return postype;
    }

  }
}
