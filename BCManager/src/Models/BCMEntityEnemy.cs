using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntityEnemy
  {
    private Dictionary<string, string> options = new Dictionary<string, string>();
    private Dictionary<string, object> _bin = new Dictionary<string, object>();
    public enum Filters
    {
      EntityId,
      Position
    }
    public static class StrFilters
    {
      public static string EntityId = "entityid";
      public static string Position = "position";
    }

    private List<string> _filter = new List<string>();

    #region Properties
    public int EntityId;

    public class BCMVector3i
    {
      public int x;
      public int y;
      public int z;
      public BCMVector3i()
      {
        this.x = 0;
        this.y = 0;
        this.z = 0;
      }
      public BCMVector3i(Vector3 v)
      {
        this.x = Mathf.RoundToInt(v.x);
        this.y = Mathf.RoundToInt(v.y);
        this.z = Mathf.RoundToInt(v.z);
      }
      public BCMVector3i(Vector3i v)
      {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
      }
    }
    public BCMVector3i Position;
    #endregion;


    private bool useInt = false;
    private List<string> strFilter = new List<string>();
    private List<int> intFilter = new List<int>();

    public BCMEntityEnemy(EntityEnemy _entity, Dictionary<string, string> _options)
    {
      options = _options;
      Load(_entity);
    }

    public Dictionary<string, object> Data ()
    {
      return _bin;
    }

    public void Load(EntityEnemy _entity)
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

      GetStats(_entity);
    }


    private void GetStats(EntityEnemy _entity)
    {
      if (isOption("filter"))
      {
        //ENTITYID
        if ((useInt && intFilter.Contains((int)Filters.EntityId)) || (!useInt && strFilter.Contains(StrFilters.EntityId)))
        {
          if (_entity != null)
          {
            EntityId = _entity.entityId;
          }
          _bin.Add("EntityId", EntityId);
        }
        //POSITION
        if ((useInt && intFilter.Contains((int)Filters.Position)) || (!useInt && strFilter.Contains(StrFilters.Position)))
        {
          if (_entity != null)
          {
            Position = new BCMVector3i(_entity.position);
          }
          _bin.Add("Position", GetVectorObj(Position));
        }

      }
      else
      {
        if (_entity != null)
        {
          EntityId = _entity.entityId;
          Position = new BCMVector3i(_entity.position);

          _bin.Add("EntityId", EntityId);
          _bin.Add("Position", GetVectorObj(Position));
        }
      }
    }

    private object GetVectorObj(BCMVector3i p)
    {
      if (options.ContainsKey("strpos"))
      {
        return p.x.ToString() + " " + p.y.ToString() + " " + p.z.ToString();
      }
      else if (options.ContainsKey("worldpos"))
      {
        return GameUtils.WorldPosToStr(new Vector3(p.x, p.y, p.z), " ");
      }
      else if (options.ContainsKey("csvpos"))
      {
        return new int[3] { p.x, p.y, p.z };
      }
      else
      {
        //vectors
        return p;
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
      var postype = "strpos";
      if (options.ContainsKey("worldpos"))
      {
        postype = "worldpos";
      }
      if (options.ContainsKey("csvpos"))
      {
        postype = "csvpos";
      }
      if (options.ContainsKey("vectors"))
      {
        postype = "vectors";
      }

      return postype;
    }

  }
}
