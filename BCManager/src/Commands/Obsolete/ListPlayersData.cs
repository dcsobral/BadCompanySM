using System;
using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class ListPlayersData : BCCommandAbstract
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

        }
        else
        {
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
      EntityPlayer entityPlayer = null;
      if (entity is EntityPlayer)
      {
        entityPlayer = (EntityPlayer)entity;
        if (entityPlayer != null)
        {
          bool first = true;
          string entitydata = " [";

          entitydata += "Name=";
          entitydata += entityPlayer.EntityName;
          ClientInfo ci = ConsoleHelper.ParseParamIdOrName(entity.entityId.ToString());
          if (ci != null)
          {
            string steamid = ci.playerId;
            entitydata += ", steamid=";
            entitydata += steamid;
          }
          entitydata += ", id=";
          entitydata += entityPlayer.entityId;
          entitydata += ", Type=";
          entitydata += entityPlayer.GetType().Name;
          entitydata += ", MaxHealth=" + entityPlayer.GetMaxHealth();
          entitydata += ", Health=" + entityPlayer.Health;
          entitydata += ", Stamina=" + entityPlayer.Stamina;
          entitydata += ", SpeedModifier=" + entityPlayer.Stats.SpeedModifier.Value;

          entitydata += "\n  Level=" + entityPlayer.GetLevel();
          entitydata += ", LevelProgress=" + (entityPlayer.GetLevelProgressPercentage()*100).ToString("0.00") + "%";
          entitydata += ", ExpToNextLevel=" + entityPlayer.ExpToNextLevel;
          entitydata += ", ExpForNextLevel=" + entityPlayer.GetExpForNextLevel();
          entitydata += ", ItemsCrafted=" + entityPlayer.totalItemsCrafted;

          entitydata += "\n  Gamestage=" + entityPlayer.gameStage;
          entitydata += ", KilledPlayers=" + entityPlayer.KilledPlayers;
          entitydata += ", KilledZombies=" + entityPlayer.KilledZombies;
          entitydata += ", Deaths=" + entityPlayer.Died;
          //entitydata += ", currentLife=" + entityPlayer.currentLife;
          entitydata += ", CurrentLife=" + entityPlayer.currentLife.ToString("0.0") + " mins";
          //entitydata += ", longestLife=" + entityPlayer.longestLife;
          entitydata += ", LongestLife=" + entityPlayer.longestLife.ToString("0.0") + " mins";
          entitydata += ", Score=" + entityPlayer.Score;
          
          entitydata += "\n  RentedVendor=" + entityPlayer.RentedVMPosition;
          entitydata += ", RentedVendorExpire=" + entityPlayer.RentalEndTime;
          entitydata += ", Pos=" + entityPlayer.GetPosition();
          entitydata += ", Rot=" + entityPlayer.rotation;
          entitydata += ", Lifetime=" + ((entityPlayer.lifetime != 3.40282347E+38f) ? entityPlayer.lifetime.ToString("0.0") : "Max");
          entitydata += ", Remote=" + entityPlayer.isEntityRemote;
          entitydata += ", Dead=" + entityPlayer.IsDead();
          //entityPlayer.GetDroppedBackpackPosition();

          entitydata += "\n  CreationTimeSinceLevelLoad=" + entityPlayer.CreationTimeSinceLevelLoad;
          entitydata += ", WorldTimeBorn=" + entityPlayer.WorldTimeBorn;
          entitydata += ", pingToServer=" + entityPlayer.pingToServer;
          entitydata += ", onGround=" + entityPlayer.onGround;
          entitydata += ", IsMale=" + entityPlayer.IsMale;
          entitydata += ", IsSafeZoneActive=" + entityPlayer.IsSafeZoneActive();
          entitydata += ", IsStuck=" + entityPlayer.IsStuck;

          // SPAWN POINTS
          entitydata += "  \nSpawnpoints={";
          for (int i = entityPlayer.SpawnPoints.Count - 1; i >= 0; i--)
          {
            entitydata += "[Bed:" + entityPlayer.SpawnPoints[i] + "]";
          }
          entitydata += "}";

          // BUFFS
          entitydata += "  \nBuffs={";
          first = true;
          foreach (MultiBuff b in entityPlayer.Stats.Buffs)
          {
            if (!first) { entitydata += ", "; } else { first = false; }
            //BuffTimerDuration btd = (BuffTimerDuration)b.Timer;
            //entitydata += b.Name + "(" + b.MultiBuffClass.Id + ")" + ": " + btd.TimeLeft + " - " + (btd.TimeFraction * 100).ToString("0.0") + "%";
            entitydata += b.Name + "(" + b.MultiBuffClass.Id + ")" + ":" + (b.Timer.TimeFraction * 100).ToString("0.0") + "%";
          }
          entitydata += "}";

          // SKILLS
          entitydata += "  \nSkills={";
          first = true;
          foreach (Skill s in entityPlayer.Skills.GetAllSkills())
          {
            // Note: don't use s.TitleKey as it will break the command if there is no localisation for the skill
            if (!first) { entitydata += ", "; } else { first = false; }
            entitydata += s.Name + ":" + s.Level + " +" + (s.PercentThisLevel * 100).ToString("0.0") + "%";
          }
          entitydata += "}";

          //// NOTIFICATIONS
          //entitydata += "  \nNotifications={";
          //first = true;
          //foreach (EntityUINotification n in entityPlayer.Stats.Notifications)
          //{
          //  if (!first) { entitydata += ", "; } else { first = false; }
          //  entitydata += n.Buff.Name + "(" + n.Buff.ClassId + ")" + ": " + (n.Buff.Timer.TimeFraction * 100).ToString("0.0") + "%";
          //}
          //entitydata += "}";

          //// WAYPOINTS
          //// TODO: requires data from save file?
          //entitydata += ", Waypoints={";
          //foreach (Waypoint wp in entityPlayer.Waypoints.List)
          //{
          //  entitydata += "[wp:" + wp.pos + "]";
          //}
          //entitydata += "}";

          // QUESTS
          // TODO: Implement system to cache quest data to persistent file
          entitydata += ", Quests={";
          //foreach (Quest q in entityPlayer.QuestJournal.quests)
          foreach (Quest q in entityPlayer.QuestJournal.Clone().quests)
            {
              entitydata += "[q:" + q.ID + "(" + q.CurrentState + ")]";
          }
          entitydata += "}";

          entitydata += "]";

          //Log.Out(entitydata);
          SdtdConsole.Instance.Output(entitydata);
        }
      }
    }
  }
}
