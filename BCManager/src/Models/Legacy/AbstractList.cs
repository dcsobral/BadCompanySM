//using System;
//using System.Collections.Generic;

//namespace BCM.Models
//{
//  [Serializable]
//  public class AbstractList
//  {
//    private Dictionary<string, string> options = new Dictionary<string, string>();

//    public AbstractList()
//    {
//    }

//    public AbstractList(PlayerInfo _pInfo)
//    {
//      Load(_pInfo);
//    }

//    public AbstractList(Dictionary<string, string> _options)
//    {
//      options = _options;
//      Load();
//    }

//    public AbstractList(PlayerInfo _pInfo, Dictionary<string, string> _options)
//    {
//      options = _options;
//      Load(_pInfo);
//    }

//    public bool isOption(string key, bool isOr = true)
//    {
//      var _o = !isOr;
//      var keys = key.Split(' ');
//      if (keys.Length > 1)
//      {
//        foreach(var k in keys)
//        {
//          if (isOr)
//          {
//            _o |= options.ContainsKey(k);
//          }
//          else
//          {
//            _o &= options.ContainsKey(k);
//          }
//        }
//        return _o;
//      }
//      else
//      {
//        return options.ContainsKey(key);
//      }
//    }

//    public string OptionValue(string key)
//    {
//      if (options.ContainsKey(key))
//      {
//        return options[key];
//      }
//      return "";
//    }

//    public string GetPosType()
//    {
//      var postype = "";
//      if (options.ContainsKey("worldpos"))
//      {
//        postype = "worldpos";
//      }
//      if (options.ContainsKey("csvpos"))
//      {
//        postype = "csvpos";
//      }

//      return postype;
//    }

//    public virtual void Load()
//    {
//    }

//    public virtual void Load(PlayerInfo _pInfo)
//    {
//    }

//    public virtual string Display(string sep = " ")
//    {
//      return string.Empty;
//    }
//  }
//}
