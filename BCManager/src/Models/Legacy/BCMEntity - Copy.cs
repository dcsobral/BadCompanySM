using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntity
  {
    private readonly Dictionary<string, string> _options;
    private readonly Dictionary<string, object> _bin = new Dictionary<string, object>();
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
        x = 0;
        y = 0;
        z = 0;
      }
      public BCMVector3i(Vector3 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
        z = Mathf.RoundToInt(v.z);
      }
      public BCMVector3i(Vector3i v)
      {
        x = v.x;
        y = v.y;
        z = v.z;
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


    private bool _useInt;
    private List<string> _strFilter = new List<string>();
    private List<int> _intFilter = new List<int>();

    public BCMEntity(Entity entity, Dictionary<string, string> options)
    {
      _options = options;
      Load(entity);
    }

    public Dictionary<string, object> Data()
    {
      return _bin;
    }

    public void Load(Entity entity)
    {
      if (IsOption("filter"))
      {
        _strFilter = OptionValue("filter").ToLower().Split(',').ToList();
        if (_strFilter.Count > 0)
        {
          _intFilter = _strFilter.Where(o => int.TryParse(o, out int _)).Select(int.Parse).ToList();
          if (_intFilter.Count == _strFilter.Count)
          {
            _useInt = true;
          }
        }
      }

      GetStats(entity);
    }

    private void GetStats(Entity entity)
    {
      if (IsOption("filter"))
      {
        if (entity == null) return;

        if (_useInt)
        {
          if (_intFilter.Contains((int)Filters.EntityId))
          {
            EntityId = entity.entityId;
            _bin.Add("EntityId", EntityId);
          }
          if (_intFilter.Contains((int)Filters.Type))
          {
            Type = entity.GetType().ToString();
            _bin.Add("Type", Type);
          }
          if (_intFilter.Contains((int)Filters.Name))
          {
            var ec = EntityClass.list[entity.entityClass];
            Name = ec.entityClassName;
            _bin.Add("Name", Name);
          }
          if (_intFilter.Contains((int)Filters.Position))
          {
            Position = new BCMVector3i(entity.position);
            _bin.Add("Position", GetVectorObj(Position));
          }
          if (_intFilter.Contains((int)Filters.Rotation))
          {
            Rotation = new BCMVector3i(entity.rotation);
            _bin.Add("Rotation", GetVectorObj(Rotation));
          }
          if (_intFilter.Contains((int)Filters.Lifetime))
          {
            Lifetime = entity.lifetime < float.MaxValue ? entity.lifetime : (double?)null;
            _bin.Add("Lifetime", Lifetime);
          }

          var entityAlive = entity as EntityAlive;
          if (entityAlive == null) return;

          if (_intFilter.Contains((int)Filters.CreationTime))
          {
            CreationTime = Math.Round(entityAlive.CreationTimeSinceLevelLoad, 1);
            _bin.Add("CreationTime", CreationTime);
          }
          if (_intFilter.Contains((int)Filters.MaxHealth))
          {
            MaxHealth = entityAlive.GetMaxHealth();
            _bin.Add("MaxHealth", MaxHealth);
          }
          if (_intFilter.Contains((int)Filters.Health))
          {
            Health = entityAlive.Health;
            _bin.Add("Health", Health);
          }
          if (_intFilter.Contains((int)Filters.IsAlive))
          {
            IsAlive = !entityAlive.IsDead();
            _bin.Add("IsAlive", IsAlive);
          }
          if (_intFilter.Contains((int)Filters.IsScout))
          {
            IsScout = entityAlive.IsScoutZombie;
            _bin.Add("IsScout", IsScout);
          }
          if (_intFilter.Contains((int)Filters.IsFeral))
          {
            IsFeral = entityAlive.isFeral;
            _bin.Add("IsFeral", IsFeral);
          }
          if (_intFilter.Contains((int)Filters.IsSleeper))
          {
            IsSleeper = entityAlive.IsSleeper;
            _bin.Add("IsSleeper", IsSleeper);
          }
          if (_intFilter.Contains((int)Filters.IsDecoy))
          {
            IsDecoy = entityAlive.IsSleeperDecoy;
            _bin.Add("IsDecoy", IsDecoy);

          }
          if (_intFilter.Contains((int)Filters.IsSleeping))
          {
            IsSleeping = entityAlive.IsSleeping;
            _bin.Add("IsSleeping", IsSleeping);
          }

        }
        else
        {
          if (_strFilter.Contains(StrFilters.EntityId))
          {
            EntityId = entity.entityId;
            _bin.Add("EntityId", EntityId);
          }

          if (_strFilter.Contains(StrFilters.Type))
          {
            Type = entity.GetType().ToString();
            _bin.Add("Type", Type);
          }
          if (_strFilter.Contains(StrFilters.Name))
          {
            var ec = EntityClass.list[entity.entityClass];
            Name = ec.entityClassName;
            _bin.Add("Name", Name);
          }
          if (_strFilter.Contains(StrFilters.Position))
          {
            Position = new BCMVector3i(entity.position);
            _bin.Add("Position", GetVectorObj(Position));
          }
          if (_strFilter.Contains(StrFilters.Rotation))
          {
            Rotation = new BCMVector3i(entity.rotation);
            _bin.Add("Rotation", GetVectorObj(Rotation));
          }
          if (_strFilter.Contains(StrFilters.Lifetime))
          {
            Lifetime = entity.lifetime < float.MaxValue ? entity.lifetime : (double?)null;
            _bin.Add("Lifetime", Lifetime);
          }

          var entityAlive = entity as EntityAlive;
          if (entityAlive == null) return;

          if (_strFilter.Contains(StrFilters.CreationTime))
          {
            CreationTime = Math.Round(entityAlive.CreationTimeSinceLevelLoad, 1);
            _bin.Add("CreationTime", CreationTime);
          }
          if (_strFilter.Contains(StrFilters.MaxHealth))
          {
            MaxHealth = entityAlive.GetMaxHealth();
            _bin.Add("MaxHealth", MaxHealth);
          }
          if (_strFilter.Contains(StrFilters.Health))
          {
            Health = entityAlive.Health;
            _bin.Add("Health", Health);
          }
          if (_strFilter.Contains(StrFilters.IsAlive))
          {
            IsAlive = !entityAlive.IsDead();
            _bin.Add("IsAlive", IsAlive);
          }
          if (_strFilter.Contains(StrFilters.IsScout))
          {
            IsScout = entityAlive.IsScoutZombie;
            _bin.Add("IsScout", IsScout);
          }
          if (_strFilter.Contains(StrFilters.IsFeral))
          {
            IsFeral = entityAlive.isFeral;
            _bin.Add("IsFeral", IsFeral);
          }
          if (_strFilter.Contains(StrFilters.IsSleeper))
          {
            IsSleeper = entityAlive.IsSleeper;
            _bin.Add("IsSleeper", IsSleeper);
          }
          if (_strFilter.Contains(StrFilters.IsDecoy))
          {
            IsDecoy = entityAlive.IsSleeperDecoy;
            _bin.Add("IsDecoy", IsDecoy);

          }
          if (_strFilter.Contains(StrFilters.IsSleeping))
          {
            IsSleeping = entityAlive.IsSleeping;
            _bin.Add("IsSleeping", IsSleeping);
          }

        }
      }
      else
      {
        if (entity == null) return;

        EntityId = entity.entityId;
        _bin.Add("EntityId", EntityId);

        Type = entity.GetType().ToString();
        _bin.Add("Type", Type);

        var entityClass = EntityClass.list[entity.entityClass];
        Name = entityClass.entityClassName;
        _bin.Add("Name", Name);

        Position = new BCMVector3i(entity.position);
        _bin.Add("Position", GetVectorObj(Position));
        Rotation = new BCMVector3i(entity.rotation);
        _bin.Add("Rotation", GetVectorObj(Rotation));

        Lifetime = entity.lifetime < float.MaxValue ? entity.lifetime : (double?)null;
        _bin.Add("Lifetime", Lifetime);

        var entityAlive = entity as EntityAlive;
        if (entityAlive == null) return;

        CreationTime = Math.Round(entityAlive.CreationTimeSinceLevelLoad, 1);
        _bin.Add("CreationTime", CreationTime);

        MaxHealth = entityAlive.GetMaxHealth();
        _bin.Add("MaxHealth", MaxHealth);

        Health = entityAlive.Health;
        _bin.Add("Health", Health);

        IsAlive = !entityAlive.IsDead();
        _bin.Add("IsAlive", IsAlive);

        IsScout = entityAlive.IsScoutZombie;
        _bin.Add("IsScout", IsScout);

        IsFeral = entityAlive.isFeral;
        _bin.Add("IsFeral", IsFeral);

        IsSleeper = entityAlive.IsSleeper;
        _bin.Add("IsSleeper", IsSleeper);

        IsDecoy = entityAlive.IsSleeperDecoy;
        _bin.Add("IsDecoy", IsDecoy);

        IsSleeping = entityAlive.IsSleeping;
        _bin.Add("IsSleeping", IsSleeping);
      }
    }

    private object GetVectorObj(BCMVector3i p)
    {
      if (_options.ContainsKey("strpos"))
      {
        return p.x + " " + p.y + " " + p.z;
      }
      if (_options.ContainsKey("worldpos"))
      {
        return GameUtils.WorldPosToStr(new Vector3(p.x, p.y, p.z), " ");
      }
      if (_options.ContainsKey("csvpos"))
      {
        return new[] { p.x, p.y, p.z };
      }

      //vectors
      return p;
    }

    private bool IsOption(string key, bool isOr = true)
    {
      var o = !isOr;
      var keys = key.Split(' ');
      if (keys.Length <= 1) return _options.ContainsKey(key);

      foreach (var k in keys)
      {
        if (isOr)
        {
          o |= _options.ContainsKey(k);
        }
        else
        {
          o &= _options.ContainsKey(k);
        }
      }

      return o;
    }

    private string OptionValue(string key) => _options.ContainsKey(key) ? _options[key] : string.Empty;
  }
}
