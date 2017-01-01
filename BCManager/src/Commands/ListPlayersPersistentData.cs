using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using BCM.PersistentData;
using BCM.Models;
using System.Reflection;

namespace BCM.Commands
{
  public class ListPlayersPersistentData : BCCommandAbstract
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
          // specific player
          string _steamId = "";
          if (GetEntity.GetBySearch(_params[0], out _steamId, "CON"))
          {
            string output = playerData(_steamId);
            Log.Out(output);
            SdtdConsole.Instance.Output(output);
          }
        }
        else
        {
          // specific players
          List<string> players = GetEntity.GetStoredPlayers();
          foreach (string steamId in players)
          {
            string output = playerData(steamId);
            Log.Out(output);
            SdtdConsole.Instance.Output(output);
          }
          SdtdConsole.Instance.Output("Total of " + players.Count + " player data files");
        }
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    private string playerData(string _steamId)
    {
      EntityPlayer _playerLive = null;
      string output = "\n";
      bool first = true;
      PlayerDataFile _pdf = new PlayerDataFile();

      try
      {
        _pdf.Load(GameUtils.GetPlayerDataDir(), _steamId);
      }
      catch
      {
        Log.Out("" + Config.ModPrefix + " Player Data not found for SteamId: " + _steamId);
        SdtdConsole.Instance.Output("Player Data not found for SteamId: " + _steamId);
      }
      Player _ppd = PersistentContainer.Instance.Players[_steamId, false];
      ClientInfo _ci = ConsoleHelper.ParseParamIdOrName(_pdf.id.ToString());

      //CLIENT INFO
      output += "Name:" + (_ci != null ? _ci.playerName : _ppd != null ? _ppd.Name : string.Empty) + "\n";
      output += "SteamId:" + _steamId + "\n";
      output += "EntityId:" + (_ci != null ? _ci.entityId.ToString() : _ppd != null ? _ppd.EntityID.ToString() : string.Empty) + "\n";
      output += "IP:" + (_ci != null ? _ci.ip.ToString() : _ppd != null ? _ppd.IP.ToString() : string.Empty) + "\n";
      //todo: add last ping to persistent data
      output += "Ping:" + (_ci != null ? _ci.ping.ToString() : "Offline") + "\n";
      if (_ci != null)
      {
        _playerLive = (EntityPlayer)GameManager.Instance.World.Entities.dict[_ci.entityId];
      }

      long totalPlayTime = (_ppd != null ? _ppd.TotalPlayTime : 0);
      if (_playerLive == null)
      {
        output += "LastOnline:" + (_ppd != null ? _ppd.LastOnline.ToString("yyyy-MM-dd HH:mm") : "") + "\n";
        output += "LastPosition:" + (_ppd != null ? GameUtils.WorldPosToStr(_ppd.LastPosition.ToVector3(), " ") : "") + "\n";
        //output += "LastPosition:" + (_ppd != null ? _ppd.LastPosition.x + " " + _ppd.LastPosition.y + " " + _ppd.LastPosition.z : "") + "\n";
        //todo: add lastrotation to persistent data
        output += "TotalPlayTime:" + totalPlayTime + "\n";
      }
      else if (_playerLive != null)
      {
        output += "SessionPlayTime:" + ((Time.timeSinceLevelLoad - _playerLive.CreationTimeSinceLevelLoad) / 60).ToString("0.0") + "(mins)\n";
        output += "TotalPlayTime:" + (totalPlayTime / 60).ToString("0.0") + "(mins)\n";
        output += "Position=" + _playerLive.position.x.ToString("0.0") + " " + _playerLive.position.y.ToString("0.0") + " " + _playerLive.position.z.ToString("0.0") + "\n";
        output += "Rotation=" + _playerLive.rotation + "\n";
      }

      //STATS
      output += "Wellness:" + (_playerLive != null ? ((_playerLive.Stats.Wellness.Value == 100 && _playerLive.Stats.Wellness.Value != _pdf.ecd.stats.Wellness.Value) ? _pdf.ecd.stats.Wellness.Value : _playerLive.Stats.Wellness.Value) : _pdf.ecd.stats.Wellness.Value) + "\n";
      output += "Health:" + (_playerLive != null ? ((_playerLive.Health == 100 && _playerLive.Health != _pdf.ecd.stats.Health.Value) ? _pdf.ecd.stats.Health.Value : _playerLive.Health) : _pdf.ecd.stats.Health.Value) + "\n";
      output += "Stamina:" + (_playerLive != null ? ((_playerLive.Stamina == 100 && _playerLive.Stamina != _pdf.ecd.stats.Stamina.Value) ? _pdf.ecd.stats.Stamina.Value : _playerLive.Stamina) : _pdf.ecd.stats.Stamina.Value) + "\n";
      output += "Food:" + (int)(_pdf.food.GetLifeLevelFraction() * 100) + "\n";
      output += "Drink:" + (int)(_pdf.drink.GetLifeLevelFraction() * 100) + "\n";
      output += "CoreTemp:" + _pdf.ecd.stats.CoreTemp.Value + "\n";
      output += "SpeedModifier:" + (_playerLive != null ? ((_playerLive.Stats.SpeedModifier.Value == 1 && _playerLive.Stats.SpeedModifier.Value != _pdf.ecd.stats.SpeedModifier.Value) ? _pdf.ecd.stats.SpeedModifier.Value : _playerLive.Stats.SpeedModifier.Value) : _pdf.ecd.stats.SpeedModifier.Value) + "\n";

      output += "Level:" + (_playerLive != null ? _playerLive.GetLevel() : _pdf.level) + "\n";
      output += "LevelProgress=" + (_playerLive != null ? (_playerLive.GetLevelProgressPercentage() * 100).ToString("0.00") : (_pdf.experience / Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pdf.level + 1)), int.MaxValue) * 100).ToString("0.00") + "%") + "\n";
      output += "ExpToNextLevel:" + (_playerLive != null ? _playerLive.ExpToNextLevel : (int)_pdf.experience) + "\n";
      output += "ExpForNextLevel:" + (_playerLive != null ? _playerLive.GetExpForNextLevel() : (int)Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pdf.level + 1)), int.MaxValue)) + "\n";
      output += "ItemsCrafted:" + (_playerLive != null ? _playerLive.totalItemsCrafted : _pdf.totalItemsCrafted) + "\n";

      //add gamestage to persistent data
      output += "Gamestage:" + (_playerLive != null ? _playerLive.gameStage.ToString() : "") + "\n";

      output += "KilledPlayers:" + (_playerLive != null ? _playerLive.KilledPlayers : _pdf.playerKills) + "\n";
      output += "KilledZombies:" + (_playerLive != null ? _playerLive.KilledZombies : _pdf.zombieKills) + "\n";
      output += "Deaths:" + (_playerLive != null ? _playerLive.Died : _pdf.deaths) + "\n";
      output += "CurrentLife:" + (_playerLive != null ? _playerLive.currentLife.ToString("0.0") + "(mins)" : _pdf.currentLife.ToString("0.0") + "(mins)") + "\n";
      output += "LongestLife:" + (_playerLive != null ? _playerLive.longestLife.ToString("0.0") + "(mins)" : _pdf.longestLife.ToString("0.0") + "(mins)") + "\n";
      output += "Score:" + (_playerLive != null ? _playerLive.Score : _pdf.score) + "\n";

      if (_playerLive != null) { 
        output += "onGround=" + _playerLive.onGround + "\n";
        output += "IsStuck=" + _playerLive.IsStuck + "\n";
        output += "IsSafeZoneActive=" + _playerLive.IsSafeZoneActive() + "\n";
        output += "Remote=" + _playerLive.isEntityRemote + "\n";
        output += "Dead=" + _playerLive.IsDead() + "\n";
        //add LastZombieAttackTime to persistent data
        output += "TimeSinceLastZombieAttack=" + ((GameManager.Instance.World.worldTime - _playerLive.LastZombieAttackTime) / 600).ToString("0.0") + "(mins)\n";
      }

      output += "RentedVendor:" + (_playerLive != null ? _playerLive.RentedVMPosition : _pdf.rentedVMPosition) + "\n";
      output += "RentedVendorExpire:" + (_playerLive != null ? _playerLive.RentalEndTime : _pdf.rentalEndTime) + "\n";

      List<Buff> _buffs = null;
      List<Skill> _skills = null;
      if (_playerLive != null)
      {
        // todo: merge with saved buffs?
        _buffs = _playerLive.Stats.Buffs;
        _skills = _playerLive.Skills.GetAllSkills();
      }
      else
      {
        _buffs = _pdf.ecd.stats.Buffs;
        //_skills = _playerSaved.Skills.GetAllSkills();
        // todo: add skills to persistent data
        _skills = null;
      }

      // BUFFS
      output += "Buffs={\n";
      first = true;
      foreach (MultiBuff b in _buffs)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += " " + b.Name + "(" + b.MultiBuffClass.Id + ")" + ":" + (b.MultiBuffClass.FDuration * b.Timer.TimeFraction) + "(" + (b.Timer.TimeFraction * 100).ToString("0.0") + "%)";
      }
      output += "\n}\n";

      // SKILLS
      output += "SkillPoints:" + _pdf.skillPoints + "\n";
      if (_skills != null)
      {
        output += "Skills={\n";
        first = true;
        foreach (Skill s in _skills)
        {
          // Note: don't use s.TitleKey as it will break the command if there is no localisation for the skill
          if (!first) { output += ", "; } else { first = false; }
          output += " " + s.Name + ":" + s.Level + " +" + (s.PercentThisLevel * 100).ToString("0.0") + "%";
        }
        output += "\n}\n";
      }

      // SPAWN POINTS
      output += "Spawnpoints(saved)={\n";
      first = true;
      for (int i = _pdf.spawnPoints.Count - 1; i >= 0; i--)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += " Bed:" + _pdf.spawnPoints[i];
      }
      output += "\n}\n";

      // QUESTS
      QuestList ql = new QuestList(_pdf);
      output += ql.DisplayQuests();

      //output += "Quests(saved)={\n";
      //first = true;
      //foreach (Quest q in _pdf.questJournal.Clone().quests)
      //{
      //  //QuestClass qc = new QuestClass(q.ID);
      //  var qc = QuestClass.s_Quests[q.ID];
      //  if (!first) { output += ",\n"; } else { first = false; }
      //  output += " " + qc.Name + "(" + q.ID + "):" + q.CurrentState;
      //}
      //output += "\n}\n";

      // WAYPOINTS
      output += "Waypoints(saved)={\n";
      first = true;
      foreach (Waypoint wp in _pdf.waypoints.List)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += wp.name + ":" + wp.pos;
      }
      output += "\n}\n";

      // favoriteRecipeList
      // todo: filter duplicate entrys for recipes with multiple versions?
      output += "FavoriteRecipe(saved)={\n";
      first = true;
      foreach (string fr in _pdf.favoriteRecipeList)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += fr;
      }
      output += "\n}\n";

      // unlockedRecipeList
      // todo: filter duplicate entrys for recipes with multiple versions
      output += "UnlockedRecipe(saved)={\n";
      first = true;
      foreach (string ur in _pdf.unlockedRecipeList)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += ur;
      }
      output += "\n}\n";



      //output += "RecipeQueueItems:" + "\n";
      //foreach (RecipeQueueItem rqi in _pdf.craftingData.RecipeQueueItems)
      //{
      //  if (rqi.IsCrafting)
      //  {
      //    output += "    " + rqi.Recipe.GetName() + ": (" + rqi.Multiplier + ")" + rqi.CraftingTimeLeft + "\n";
      //  }
      //}


      //output += "inventory:" + "\n";
      //foreach (ItemStack inv in _pdf.inventory)
      //{
      //  output += "    inv:" + inv + "\n";
      //}


      //output += "equipment:" + "\n" + "\n";
      //foreach (ItemValue its in _pdf.equipment.GetItems())
      //{
      //  output += "    its:" + its + "\n";
      //}





      return output;
    }
  }
}
