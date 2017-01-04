using System;
using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class ListEntities : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        if (_params.Count != 0 && _params.Count != 1)
        {
          SdtdConsole.Instance.Output("Wrong number of arguments, expected 0 or 1, found " + _params.Count + ".");
          return;
        }

        if (_params.Count == 1)
        {
          //ClientInfo ci = ConsoleHelper.ParseParamIdOrName(_params[0]);
          //if (ci == null)
          //{
          //  SdtdConsole.Instance.Output("Playername or entity id not found.");
          //  return;
          //}

          int n = int.MinValue;
          int.TryParse(_params[0], out n);
          Entity entity = GameManager.Instance.World.Entities.dict[n]; //ci.entityId
          if (entity == null)
          {
            SdtdConsole.Instance.Output("Entity id not found.");
            return;
          }
          printEntityData(entity);

        } else {
          for (int i = GameManager.Instance.World.Entities.list.Count - 1; i >= 0; i--)
          {
            Entity entity = GameManager.Instance.World.Entities.list[i];
            printEntityData(entity);
          }
          SdtdConsole.Instance.Output("Total of " + GameManager.Instance.World.Entities.Count + " entities in the world");
        }
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    private void printEntityData(Entity entity)
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
      entitydata += ((entity.lifetime != 3.40282347E+38f) ? entity.lifetime.ToString("0.0") : "Max");
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
      if (entityPlayer != null)
      {
        entitydata += ", Stamina=" + entityPlayer.Stamina;
        entitydata += ", Ping=" + entityPlayer.pingToServer;

        entitydata += ", Level=" + entityPlayer.GetLevel();
        entitydata += ", LevelProgress=" + entityPlayer.GetLevelProgressPercentage();
        entitydata += ", ExpToNextLevel=" + entityPlayer.ExpToNextLevel;
        entitydata += ", ExpForNextLevel=" + entityPlayer.GetExpForNextLevel();
        entitydata += ", ItemsCrafted=" + entityPlayer.totalItemsCrafted;

        entitydata += ", Gamestage=" + entityPlayer.gameStage;
        entitydata += ", KilledPlayers=" + entityPlayer.KilledPlayers;
        entitydata += ", KilledZombies=" + entityPlayer.KilledZombies;
        entitydata += ", Score=" + entityPlayer.Score;
        entitydata += ", CurrentLife=" + entityPlayer.currentLife;
        entitydata += ", LongestLife=" + entityPlayer.longestLife;

        entitydata += ", onGround=" + entityPlayer.onGround;
        entitydata += ", IsStuck=" + entityPlayer.IsStuck;

      }
      if (entityZombie != null)
      {
        entitydata += ", IsScoutZombie=" + entityZombie.IsScoutZombie;
      }
      entitydata += "]";
      SdtdConsole.Instance.Output(entitydata);
    }
  }
}
