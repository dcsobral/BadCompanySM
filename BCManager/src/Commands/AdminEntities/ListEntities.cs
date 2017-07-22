using System;
using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class ListEntities : BCCommandAbstract
  {
    public override void Process()
    {
      if (_options.ContainsKey("details"))
      {
        if (_params.Count == 1)
        {
          int n = int.MinValue;
          int.TryParse(_params[0], out n);
          Entity entity = GameManager.Instance.World.Entities.dict[n];
          if (entity == null)
          {
            SdtdConsole.Instance.Output("Entity id not found.");
            return;
          }
          displayEntity(entity);
        }
      }
      else
      {
        if (_params.Count == 1)
        {
          displaySummary(_params[0]);
        }
        else
        {
          displaySummary(null);
          SendOutput("Total of " + GameManager.Instance.World.Entities.Count + " entities in the world");
        }
      }
    }

    private void displaySummary(string filter)
    {
      Dictionary<string, List<Entity>> list = new Dictionary<string, List<Entity>>();
      for (int i = GameManager.Instance.World.Entities.list.Count - 1; i >= 0; i--)
      {
        Entity entity = GameManager.Instance.World.Entities.list[i];
        if (!_options.ContainsKey("all"))
        {
          if (_options.ContainsKey("alive") && entity.IsDead())
          {
            continue;
          }
          if (_options.ContainsKey("dead") && !entity.IsDead())
          {
            continue;
          }
        }
        if (list.ContainsKey(entity.GetType().ToString()))
        {
          list[entity.GetType().ToString()].Add(entity);
        }
        else
        {
          list.Add(entity.GetType().ToString(), new List<Entity>());
          list[entity.GetType().ToString()].Add(entity);
        }
      }
      foreach (string type in list.Keys)
      {
        if (filter == null || type == filter)
        {
          SendOutput("Entities of type " + type + ":" + list[type].Count);
          foreach (Entity e in list[type])
          {
            displayEntity(e);
          }
        }
      }
    }
    private void displayEntity(Entity entity)
    {
      EntityAlive entityAlive = null;
      EntityPlayer entityPlayer = null;
      EntityNPC entityNPC = null;
      EntityZombie entityZombie = null;
      if (entity is EntityAlive)
      {
        entityAlive = (EntityAlive)entity;
      }
      if (entity is EntityPlayer)
      {
        entityPlayer = (EntityPlayer)entity;
      }
      if (entity is EntityNPC)
      {
        entityNPC = (EntityNPC)entity;
      }
      if (entity is EntityZombie)
      {
        entityZombie = (EntityZombie)entity;
      }

      string entitydata = " [";
      entitydata += string.Empty;
      entitydata += "id=";
      entitydata += entity.entityId;
      entitydata += ", ";
      entitydata += entity.ToString();
      entitydata += ", Pos=";
      entitydata += entity.GetPosition();
      entitydata += ", Rot=";
      entitydata += entity.rotation;
      entitydata += ", Lifetime=";
      entitydata += ((entity.lifetime != float.MaxValue) ? entity.lifetime.ToString("0.0") : "Max");
      entitydata += ", Remote=";
      entitydata += entity.isEntityRemote;
      entitydata += ", Dead=";
      entitydata += entity.IsDead();
      if (entityAlive != null)
      {
        entitydata += ", CreationTimeSinceLevelLoad=" + entityAlive.CreationTimeSinceLevelLoad;
        entitydata += ", MaxHealth=" + entityAlive.GetMaxHealth();
        entitydata += ", Health=" + entityAlive.Health;
      }
      if (entityZombie != null)
      {
        entitydata += ", IsScoutZombie=" + entityZombie.IsScoutZombie;
      }
      entitydata += "]";
      SendOutput(entitydata);
    }
  }
}
