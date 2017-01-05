//using System;
//using System.Collections.Generic;
//using System.Reflection;

//namespace BCM.Commands
//{
//  public class ListPlayersAdvancedStats : BCCommandAbstract
//  {
//    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
//    {
//      try
//      {
//        if (_params.Count != 0 && _params.Count != 1)
//        {
//          SdtdConsole.Instance.Output("Wrong number of arguments, expected 0 or 1, found " + _params.Count + ".");
//          return;
//        }

//        if (_params.Count == 1)
//        {
//          ClientInfo ci = ConsoleHelper.ParseParamIdOrName(_params[0]);
//          if (ci == null)
//          {
//            SdtdConsole.Instance.Output("Playername or entity id not found.");
//            return;
//          }

//          EntityPlayer player = GameManager.Instance.World.Players.dict[ci.entityId];
//          if (player == null)
//          {
//            SdtdConsole.Instance.Output("Playername or entity id not found.");
//            return;
//          }
//          printPlayersAdvancedStats(player);
//        }
//        else
//        {
//          List<EntityPlayer> players = GameManager.Instance.World.Players.list;
//          foreach (EntityPlayer player in players)
//          {
//            printPlayersAdvancedStats(player);
//          }
//        }
//      }
//      catch (Exception e)
//      {
//        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
//      }
//    }

//    private void printPlayersAdvancedStats(EntityPlayer player)
//    {
//      String entitydata = "Advanced Stats for " + player.EntityName + " (" + player.entityId + ")";
//      entitydata += " [";
//      entitydata += "MaxHealth=" + player.GetMaxHealth();
//      entitydata += ", Health=" + player.Health;
//      entitydata += ", Stamina=" + player.Stamina;
//      //entitydata += ", Food=" + player.foodStat.GetLifeLevel();
//      //entitydata += ", Drink=" + player.drinkStat.GetLifeLevel();
//      //entitydata += ", Gassiness=" + player.Gassiness;
//      //entitydata += ", Sickness=" + player.Sickness;
//      entitydata += ", position=" + player.position;
//      entitydata += ", rotation=" + player.rotation;
//      entitydata += ", pingToServer=" + player.pingToServer;

//      entitydata += ", Level=" + player.GetLevel();
//      entitydata += ", LevelProgressPercentage=" + player.GetLevelProgressPercentage();
//      entitydata += ", ExpToNextLevel=" + player.ExpToNextLevel;
//      entitydata += ", ExpForNextLevel=" + player.GetExpForNextLevel();
//      entitydata += ", totalItemsCrafted=" + player.totalItemsCrafted;

//      entitydata += ", gamestage=" + player.gameStage;
//      entitydata += ", KilledPlayers=" + player.KilledPlayers;
//      entitydata += ", KilledZombies=" + player.KilledZombies;
//      entitydata += ", currentLife=" + player.currentLife;
//      entitydata += ", longestLife=" + player.longestLife;
//      entitydata += ", Score=" + player.Score;

//      //entitydata +=  ", IsGodMode=" + player.IsGodMode; // not updating?
//      entitydata += ", onGround=" + player.onGround;
//      entitydata += ", IsMale=" + player.IsMale;
//      entitydata += ", IsSafeZoneActive=" + player.IsSafeZoneActive();
//      entitydata += ", IsStuck=" + player.IsStuck;
//      entitydata += ", moving=" + player.moving;
//      entitydata += ", MovementRunning=" + player.MovementRunning;
//      entitydata += ", LastZombieAttackTime=" + player.LastZombieAttackTime;
//      entitydata += "]";

//      SdtdConsole.Instance.Output(entitydata);
//      //Log.Out(playerAdvancedStats);
//    }
//  }
//}
