using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntity
  {
    private Dictionary<string, string> options = new Dictionary<string, string>();
    private Dictionary<string, object> _bin = new Dictionary<string, object>();
    public enum Filters
    {
      EntityId,
      Type,
      Name,
      Position,
      Rotation,
      Lifetime,
      IsAlive,

      CreationTime,
      MaxHealth,
      Health,
      IsScout,
      IsFeral,
      IsSleeper,
      IsDecoy,
      IsSleeping
    }
    public static class StrFilters
    {
      public static string EntityId = "id";
      public static string Type = "type";
      public static string Name = "name";
      public static string Position = "pos";
      public static string Rotation = "rot";
      public static string Lifetime = "lifetime";
      public static string IsAlive = "isalive";

      public static string CreationTime = "creationtime";
      public static string MaxHealth = "maxhealth";
      public static string Health = "health";
      public static string IsScout = "isscout";
      public static string IsFeral = "isferal";
      public static string IsSleeper = "issleeper";
      public static string IsDecoy = "isdecoy";
      public static string IsSleeping = "issleeping";
    }

    private List<string> _filter = new List<string>();

    #region Properties
    public int EntityId;
    public string Type;
    public string Name;

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
    public BCMVector3i Rotation;
    public double? Lifetime;
    public bool IsAlive;
    public double CreationTime;

    public int MaxHealth;
    public int Health;
    public bool IsScout;
    public bool IsFeral;
    public bool IsSleeper;
    public bool IsDecoy;
    public bool IsSleeping;

    public double SpeedModifier;
    public double SpeedDay;
    public double SpeedNight;
    #endregion;


    private bool useInt = false;
    private List<string> strFilter = new List<string>();
    private List<int> intFilter = new List<int>();

    public BCMEntity(Entity _entity, Dictionary<string, string> _options)
    {
      options = _options;
      Load(_entity);
    }

    public Dictionary<string, object> Data ()
    {
      return _bin;
    }

    public void Load(Entity _entity)
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

    private void GetStats(Entity _entity)
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

        //TYPE
        if ((useInt && intFilter.Contains((int)Filters.Type)) || (!useInt && strFilter.Contains(StrFilters.Type)))
        {
          if (_entity != null)
          {
            Type = _entity.GetType().ToString();
          }
          _bin.Add("Type", Type);
        }

        //NAME
        if ((useInt && intFilter.Contains((int)Filters.Name)) || (!useInt && strFilter.Contains(StrFilters.Name)))
        {
          if (_entity != null)
          {
            var _ec = EntityClass.list[_entity.entityClass];
            Name = _ec.entityClassName;
          }
          _bin.Add("Name", Name);
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

        //ROTATION
        if ((useInt && intFilter.Contains((int)Filters.Rotation)) || (!useInt && strFilter.Contains(StrFilters.Rotation)))
        {
          if (_entity != null)
          {
            Rotation = new BCMVector3i(_entity.rotation);
          }
          _bin.Add("Rotation", GetVectorObj(Rotation));
        }

        //LIFETIME
        if ((useInt && intFilter.Contains((int)Filters.Lifetime)) || (!useInt && strFilter.Contains(StrFilters.Lifetime)))
        {
          if (_entity != null)
          {
            Lifetime = ((_entity.lifetime != float.MaxValue) ? (double?)_entity.lifetime : (double?)null);
          }
          _bin.Add("Lifetime", Lifetime);
        }

        var _ea = _entity as EntityAlive;
        if (_ea != null)
        {
          //CREATIONTIME
          if ((useInt && intFilter.Contains((int)Filters.CreationTime)) || (!useInt && strFilter.Contains(StrFilters.CreationTime)))
          {
            if (_entity != null)
            {
              CreationTime = Math.Round(_ea.CreationTimeSinceLevelLoad, 1);
            }
            _bin.Add("CreationTime", CreationTime);
          }

          //MAXHEALTH
          if ((useInt && intFilter.Contains((int)Filters.MaxHealth)) || (!useInt && strFilter.Contains(StrFilters.MaxHealth)))
          {
            if (_entity != null)
            {
              MaxHealth = _ea.GetMaxHealth();
            }
            _bin.Add("MaxHealth", MaxHealth);
          }

          //HEALTH
          if ((useInt && intFilter.Contains((int)Filters.Health)) || (!useInt && strFilter.Contains(StrFilters.Health)))
          {
            if (_entity != null)
            {
              Health = _ea.Health;
            }
            _bin.Add("Health", Health);
          }


          //ISALIVE
          if ((useInt && intFilter.Contains((int)Filters.IsAlive)) || (!useInt && strFilter.Contains(StrFilters.IsAlive)))
          {
            if (_entity != null)
            {
              IsAlive = !_entity.IsDead();
            }
            _bin.Add("IsAlive", IsAlive);
          }

          //ISSCOUT
          if ((useInt && intFilter.Contains((int)Filters.IsScout)) || (!useInt && strFilter.Contains(StrFilters.IsScout)))
          {
            if (_entity != null)
            {
              IsScout = _ea.IsScoutZombie;
            }
            _bin.Add("IsScout", IsScout);
          }

          //ISFERAL
          if ((useInt && intFilter.Contains((int)Filters.IsFeral)) || (!useInt && strFilter.Contains(StrFilters.IsFeral)))
          {
            if (_entity != null)
            {
              IsFeral = _ea.isFeral;
            }
            _bin.Add("IsFeral", IsFeral);
          }

          //ISSLEEPER
          if ((useInt && intFilter.Contains((int)Filters.IsSleeper)) || (!useInt && strFilter.Contains(StrFilters.IsSleeper)))
          {
            if (_entity != null)
            {
              IsSleeper = _ea.IsSleeper;
            }
            _bin.Add("IsSleeper", IsSleeper);
          }

          //ISDECOY
          if ((useInt && intFilter.Contains((int)Filters.IsDecoy)) || (!useInt && strFilter.Contains(StrFilters.IsDecoy)))
          {
            if (_entity != null)
            {
              IsDecoy = _ea.IsSleeperDecoy;
            }
            _bin.Add("IsDecoy", IsDecoy);
          }

          //ISSLEEPING
          if ((useInt && intFilter.Contains((int)Filters.IsSleeping)) || (!useInt && strFilter.Contains(StrFilters.IsSleeping)))
          {
            if (_entity != null)
            {
              IsSleeping = _ea.IsSleeping;
            }
            _bin.Add("IsSleeping", IsSleeping);
          }
        }
      }
      else
      {
        if (_entity != null)
        {
          EntityId = _entity.entityId;
          _bin.Add("EntityId", EntityId);

          Type = _entity.GetType().ToString();
          _bin.Add("Type", Type);

          var _ec = EntityClass.list[_entity.entityClass];
          Name = _ec.entityClassName;
          _bin.Add("Name", Name);

          Position = new BCMVector3i(_entity.position);
          _bin.Add("Position", GetVectorObj(Position));
          Rotation = new BCMVector3i(_entity.rotation);
          _bin.Add("Rotation", GetVectorObj(Rotation));

          Lifetime = ((_entity.lifetime != float.MaxValue) ? (double?)_entity.lifetime : (double?)null);
          _bin.Add("Lifetime", Lifetime);

          var _ea = _entity as EntityAlive;
          if (_ea != null)
          {
            CreationTime = Math.Round(_ea.CreationTimeSinceLevelLoad, 1);
            _bin.Add("CreationTime", CreationTime);

            MaxHealth = _ea.GetMaxHealth();
            _bin.Add("MaxHealth", MaxHealth);

            Health = _ea.Health;
            _bin.Add("Health", Health);

            IsAlive = !_entity.IsDead();
            _bin.Add("IsAlive", IsAlive);

            IsScout = _ea.IsScoutZombie;
            _bin.Add("IsScout", IsScout);

            IsFeral = _ea.isFeral;
            _bin.Add("IsFeral", IsFeral);

            IsSleeper = _ea.IsSleeper;
            _bin.Add("IsSleeper", IsSleeper);

            IsDecoy = _ea.IsSleeperDecoy;
            _bin.Add("IsDecoy", IsDecoy);

            IsSleeping = _ea.IsSleeping;
            _bin.Add("IsSleeping", IsSleeping);
          }
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
