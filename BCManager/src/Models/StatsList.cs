using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class StatsList
  {
    private Dictionary<string, string> stats = new Dictionary<string, string>();

    public StatsList()
    {
    }

    public StatsList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      stats.Add("Wellness", (_pInfo.EP != null ? ((_pInfo.EP.Stats.Wellness.Value == 100 && _pInfo.EP.Stats.Wellness.Value != _pInfo.PDF.ecd.stats.Wellness.Value) ? _pInfo.PDF.ecd.stats.Wellness.Value : _pInfo.EP.Stats.Wellness.Value) : _pInfo.PDF.ecd.stats.Wellness.Value).ToString());
      stats.Add("Health", (_pInfo.EP != null ? ((_pInfo.EP.Health == 100 && _pInfo.EP.Health != _pInfo.PDF.ecd.stats.Health.Value) ? _pInfo.PDF.ecd.stats.Health.Value : _pInfo.EP.Health) : _pInfo.PDF.ecd.stats.Health.Value).ToString());
      stats.Add("Stamina", (_pInfo.EP != null ? ((_pInfo.EP.Stamina == 100 && _pInfo.EP.Stamina != _pInfo.PDF.ecd.stats.Stamina.Value) ? _pInfo.PDF.ecd.stats.Stamina.Value : _pInfo.EP.Stamina) : _pInfo.PDF.ecd.stats.Stamina.Value).ToString());
      stats.Add("Food", (_pInfo.PDF.food.GetLifeLevelFraction() * 100).ToString("0"));
      stats.Add("Drink", (_pInfo.PDF.drink.GetLifeLevelFraction() * 100).ToString("0"));
      stats.Add("CoreTemp", _pInfo.PDF.ecd.stats.CoreTemp.Value.ToString());
      stats.Add("SpeedModifier", (_pInfo.EP != null ? ((_pInfo.EP.Stats.SpeedModifier.Value == 1 && _pInfo.EP.Stats.SpeedModifier.Value != _pInfo.PDF.ecd.stats.SpeedModifier.Value) ? _pInfo.PDF.ecd.stats.SpeedModifier.Value : _pInfo.EP.Stats.SpeedModifier.Value) : _pInfo.PDF.ecd.stats.SpeedModifier.Value).ToString());
      stats.Add("Archetype", _pInfo.PDF.ecd.playerProfile.Archetype.ToString());
      stats.Add("DistanceWalked", _pInfo.PDF.distanceWalked.ToString());
      stats.Add("DroppedBackpack", (_pInfo.PDF.droppedBackpackPosition != Vector3i.zero ? GameUtils.WorldPosToStr(_pInfo.PDF.droppedBackpackPosition.ToVector3(), " ") : "None"));

      stats.Add("Level", (_pInfo.EP != null ? _pInfo.EP.GetLevel() : _pInfo.PDF.level).ToString());
      stats.Add("LevelProgress", (_pInfo.EP != null ? (_pInfo.EP.GetLevelProgressPercentage() * 100).ToString("0.00") + "%" : (_pInfo.PDF.experience / Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pInfo.PDF.level + 1)), int.MaxValue) * 100).ToString("0.00") + "%"));
      stats.Add("ExpToNextLevel", (_pInfo.EP != null ? _pInfo.EP.ExpToNextLevel : (int)_pInfo.PDF.experience).ToString());
      stats.Add("ExpForNextLevel", (_pInfo.EP != null ? _pInfo.EP.GetExpForNextLevel() : (int)Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pInfo.PDF.level + 1)), int.MaxValue)).ToString());

      // todo: add gamestage to persistent data
      stats.Add("Gamestage", (_pInfo.EP != null ? _pInfo.EP.gameStage.ToString() : ""));
      stats.Add("KilledPlayers", (_pInfo.EP != null ? _pInfo.EP.KilledPlayers : _pInfo.PDF.playerKills).ToString());
      stats.Add("KilledZombies", (_pInfo.EP != null ? _pInfo.EP.KilledZombies : _pInfo.PDF.zombieKills).ToString());
      stats.Add("Deaths", (_pInfo.EP != null ? _pInfo.EP.Died : _pInfo.PDF.deaths).ToString());
      stats.Add("CurrentLife", (_pInfo.EP != null ? _pInfo.EP.currentLife.ToString("0.0") : _pInfo.PDF.currentLife.ToString("0.0")));
      stats.Add("LongestLife", (_pInfo.EP != null ? _pInfo.EP.longestLife.ToString("0.0") : _pInfo.PDF.longestLife.ToString("0.0")));
      stats.Add("Score", (_pInfo.EP != null ? _pInfo.EP.Score : _pInfo.PDF.score).ToString());
      stats.Add("ItemsCrafted", (_pInfo.EP != null ? _pInfo.EP.totalItemsCrafted : _pInfo.PDF.totalItemsCrafted).ToString());

      if (_pInfo.EP != null)
      {
        stats.Add("onGround", _pInfo.EP.onGround.ToString());
        stats.Add("IsStuck", _pInfo.EP.IsStuck.ToString());
        stats.Add("IsSafeZoneActive", _pInfo.EP.IsSafeZoneActive().ToString());
        stats.Add("Remote", _pInfo.EP.isEntityRemote.ToString());
        stats.Add("Dead", _pInfo.EP.IsDead().ToString());
        //todo: add LastZombieAttackTime to persistent data
        stats.Add("TimeSinceLastZombieAttacked", ((GameManager.Instance.World.worldTime - _pInfo.EP.LastZombieAttackTime) / 600).ToString("0.0"));
      }

      stats.Add("RentedVendor", (_pInfo.EP != null ? _pInfo.EP.RentedVMPosition : _pInfo.PDF.rentedVMPosition).ToString());
      stats.Add("RentedVendorExpire", (_pInfo.EP != null ? _pInfo.EP.RentalEndTime : _pInfo.PDF.rentalEndTime).ToString());
    }

    public string Display()
    {
      string output = "";
      foreach (KeyValuePair<string, string> kvp in stats)
      {
        output += kvp.Key + ":" + kvp.Value + "\n";
      }
      return output;
    }
  }
}
