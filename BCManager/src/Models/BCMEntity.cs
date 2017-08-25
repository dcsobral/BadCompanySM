using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntity : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string EntityId = "id";
      public const string Type = "type";
      public const string Name = "name";
      public const string Position = "pos";
      public const string Rotation = "rot";
      public const string Lifetime = "lifetime";
      public const string IsAlive = "isalive";

      public const string CreationTime = "creationtime";
      public const string MaxHealth = "maxhealth";
      public const string Health = "health";
      public const string IsScout = "isscout";
      public const string IsFeral = "isferal";
      public const string IsSleeper = "issleeper";
      public const string IsDecoy = "isdecoy";
      public const string IsSleeping = "issleeping";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.EntityId },
      { 1,  StrFilters.Type },
      { 2,  StrFilters.Name },
      { 3,  StrFilters.Position },
      { 4,  StrFilters.Rotation },
      { 5,  StrFilters.Lifetime },
      { 6,  StrFilters.IsAlive },
      { 7,  StrFilters.CreationTime },
      { 8,  StrFilters.MaxHealth },
      { 9,  StrFilters.Health },
      { 10,  StrFilters.IsScout },
      { 11,  StrFilters.IsFeral  },
      { 12,  StrFilters.IsSleeper },
      { 13,  StrFilters.IsDecoy },
      { 14,  StrFilters.IsSleeping }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;

    #endregion

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

    public BCMEntity(object obj, Dictionary<string, string> options, List<string> filters) : base(obj, "Entity", options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var entity = obj as Entity;
      if (entity == null) return;
      var entityAlive = entity as EntityAlive;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.EntityId:
              GetEntityId(entity);
              break;
            case StrFilters.Type:
              GetType(entity);
              break;
            case StrFilters.Name:
              GetName(entity);
              break;
            case StrFilters.Position:
              GetPosition(entity);
              break;
            case StrFilters.Rotation:
              GetRotation(entity);
              break;
            case StrFilters.Lifetime:
              GetLifetime(entity);
              break;
            case StrFilters.CreationTime:
              if (entityAlive == null) break;
              GetCreationTime(entityAlive);
              break;
            case StrFilters.MaxHealth:
              if (entityAlive == null) break;
              GetMaxHealth(entityAlive);
              break;
            case StrFilters.Health:
              if (entityAlive == null) break;
              GetHealth(entityAlive);
              break;
            case StrFilters.IsAlive:
              if (entityAlive == null) break;
              GetIsAlive(entityAlive);
              break;
            case StrFilters.IsScout:
              if (entityAlive == null) break;
              GetIsScout(entityAlive);
              break;
            case StrFilters.IsFeral:
              if (entityAlive == null) break;
              GetIsFeral(entityAlive);
              break;
            case StrFilters.IsSleeper:
              if (entityAlive == null) break;
              GetIsSleeper(entityAlive);
              break;
            case StrFilters.IsDecoy:
              if (entityAlive == null) break;
              GetIsDecoy(entityAlive);
              break;
            case StrFilters.IsSleeping:
              if (entityAlive == null) break;
              GetIsSleeping(entityAlive);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetEntityId(entity);
        GetType(entity);
        GetName(entity);
        GetPosition(entity);
        GetRotation(entity);
        GetLifetime(entity);

        if (entityAlive == null) return;

        GetCreationTime(entityAlive);
        GetMaxHealth(entityAlive);
        GetHealth(entityAlive);
        GetIsAlive(entityAlive);
        GetIsScout(entityAlive);
        GetIsFeral(entityAlive);
        GetIsSleeper(entityAlive);
        GetIsDecoy(entityAlive);
        GetIsSleeping(entityAlive);
      }
    }

    private void GetIsSleeping(EntityAlive entityAlive) => Bin.Add("IsSleeping", IsSleeping = entityAlive.IsSleeping);

    private void GetIsDecoy(EntityAlive entityAlive) => Bin.Add("IsDecoy", IsDecoy = entityAlive.IsSleeperDecoy);

    private void GetIsSleeper(EntityAlive entityAlive) => Bin.Add("IsSleeper", IsSleeper = entityAlive.IsSleeper);

    private void GetIsFeral(EntityAlive entityAlive) => Bin.Add("IsFeral", IsFeral = entityAlive.isFeral);

    private void GetIsScout(EntityAlive entityAlive) => Bin.Add("IsScout", IsScout = entityAlive.IsScoutZombie);

    private void GetIsAlive(EntityAlive entityAlive) => Bin.Add("IsAlive", IsAlive = !entityAlive.IsDead());

    private void GetHealth(EntityAlive entityAlive) => Bin.Add("Health", Health = entityAlive.Health);

    private void GetMaxHealth(EntityAlive entityAlive) => Bin.Add("MaxHealth", MaxHealth = entityAlive.GetMaxHealth());

    private void GetCreationTime(EntityAlive entityAlive) => Bin.Add("CreationTime", CreationTime = Math.Round(entityAlive.CreationTimeSinceLevelLoad, 1));

    private void GetLifetime(Entity entity) => Bin.Add("Lifetime", Lifetime = entity.lifetime < float.MaxValue ? entity.lifetime : (double?)null);

    private void GetRotation(Entity entity) => Bin.Add("Rotation", GetVectorObj(Rotation = new BCMVector3i(entity.rotation)));

    private void GetPosition(Entity entity) => Bin.Add("Position", GetVectorObj(Position = new BCMVector3i(entity.position)));

    private void GetName(Entity entity) => Bin.Add("Name", Name = EntityClass.list[entity.entityClass]?.entityClassName);

    private void GetType(Entity entity) => Bin.Add("Type", Type = entity.GetType().ToString());

    private void GetEntityId(Entity entity) => Bin.Add("EntityId", EntityId = entity.entityId);

    private object GetVectorObj(BCMVector3i p)
    {
      if (Options.ContainsKey("strpos"))
      {
        return p.x + " " + p.y + " " + p.z;
      }
      if (Options.ContainsKey("worldpos"))
      {
        return GameUtils.WorldPosToStr(new Vector3(p.x, p.y, p.z), " ");
      }
      if (Options.ContainsKey("csvpos"))
      {
        return new[] { p.x, p.y, p.z };
      }
      return p;//vectors
    }
  }
}
