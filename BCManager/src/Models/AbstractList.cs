using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class AbstractList
  {
    public Dictionary<string, string> options = new Dictionary<string, string>();

    public AbstractList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public AbstractList(PlayerInfo _pInfo, Dictionary<string, string> _options)
    {
      options = _options;
      Load(_pInfo);
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

    public virtual void Load(PlayerInfo _pInfo)
    {
      //string postype = GetPosType();
    }

    public virtual string Display(string sep = " ")
    {
      return string.Empty;
    }
  }
}
