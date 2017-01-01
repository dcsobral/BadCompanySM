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
            string output = displayPlayers(_steamId);
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
            string output = displayPlayers(steamId);
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

    private string displayPlayers(string _steamId)
    {
      EntityPlayer _pl = null;
      string output = "\n";
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
      Player _pcd = PersistentContainer.Instance.Players[_steamId, false];
      ClientInfo _ci = ConsoleHelper.ParseParamIdOrName(_pdf.id.ToString());

      //_pdf.ecd.entityData
      //PersistentPlayerData _ppd = GameManager.Instance.persistentPlayers.GetPlayerData(_steamId);

      //CLIENT INFO
      output += "Name:" + (_ci != null ? _ci.playerName : _pcd != null ? _pcd.Name : string.Empty) + "\n";
      output += "SteamId:" + _steamId + "\n";
      output += "EntityId:" + _pdf.id + "\n";
      //output += "EntityId:" + (_ci != null ? _ci.entityId.ToString() : _ppd != null ? _ppd.EntityID.ToString() : string.Empty) + "\n";

      output += "IP:" + (_ci != null ? _ci.ip.ToString() : _pcd != null ? _pcd.IP.ToString() : string.Empty) + "\n";
      //todo: add last ping to persistent data
      output += "Ping:" + (_ci != null ? _ci.ping.ToString() : "Offline") + "\n";
      if (_ci != null)
      {
        _pl = (EntityPlayer)GameManager.Instance.World.Entities.dict[_ci.entityId];
      }

      long totalPlayTime = (_pcd != null ? _pcd.TotalPlayTime : 0);
      if (_pl == null)
      {
        output += "LastOnline:" + (_pcd != null ? _pcd.LastOnline.ToString("yyyy-MM-dd HH:mm") : "") + "\n";
        output += "LastPosition:" + (_pcd != null ? GameUtils.WorldPosToStr(_pcd.LastPosition.ToVector3(), " ") : "") + "\n";
        //todo: add lastrotation to persistent data
        output += "TotalPlayTime:" + totalPlayTime + "\n";
      }
      else if (_pl != null)
      {
        output += "SessionPlayTime:" + ((Time.timeSinceLevelLoad - _pl.CreationTimeSinceLevelLoad) / 60).ToString("0.0") + "(mins)\n";
        output += "TotalPlayTime:" + (totalPlayTime / 60).ToString("0.0") + "(mins)\n";
        output += "Position=" + _pl.position.x.ToString("0.0") + " " + _pl.position.y.ToString("0.0") + " " + _pl.position.z.ToString("0.0") + "\n";
        output += "Rotation=" + _pl.rotation + "\n";
      }

      //STATS
      output += "Wellness:" + (_pl != null ? ((_pl.Stats.Wellness.Value == 100 && _pl.Stats.Wellness.Value != _pdf.ecd.stats.Wellness.Value) ? _pdf.ecd.stats.Wellness.Value : _pl.Stats.Wellness.Value) : _pdf.ecd.stats.Wellness.Value) + "\n";
      output += "Health:" + (_pl != null ? ((_pl.Health == 100 && _pl.Health != _pdf.ecd.stats.Health.Value) ? _pdf.ecd.stats.Health.Value : _pl.Health) : _pdf.ecd.stats.Health.Value) + "\n";
      output += "Stamina:" + (_pl != null ? ((_pl.Stamina == 100 && _pl.Stamina != _pdf.ecd.stats.Stamina.Value) ? _pdf.ecd.stats.Stamina.Value : _pl.Stamina) : _pdf.ecd.stats.Stamina.Value) + "\n";
      output += "Food:" + (int)(_pdf.food.GetLifeLevelFraction() * 100) + "\n";
      output += "Drink:" + (int)(_pdf.drink.GetLifeLevelFraction() * 100) + "\n";
      output += "CoreTemp:" + _pdf.ecd.stats.CoreTemp.Value + "\n";
      output += "SpeedModifier:" + (_pl != null ? ((_pl.Stats.SpeedModifier.Value == 1 && _pl.Stats.SpeedModifier.Value != _pdf.ecd.stats.SpeedModifier.Value) ? _pdf.ecd.stats.SpeedModifier.Value : _pl.Stats.SpeedModifier.Value) : _pdf.ecd.stats.SpeedModifier.Value) + "\n";
      output += "Archetype:" + _pdf.ecd.playerProfile.Archetype + "\n";
      output += "DistanceWalked:" + _pdf.distanceWalked + "\n";
      output += "DroppedBackpack:" + (_pdf.droppedBackpackPosition != Vector3i.zero ? GameUtils.WorldPosToStr(_pdf.droppedBackpackPosition.ToVector3(), " ") : "None") + "\n";

      output += "Level:" + (_pl != null ? _pl.GetLevel() : _pdf.level) + "\n";
      output += "LevelProgress=" + (_pl != null ? (_pl.GetLevelProgressPercentage() * 100).ToString("0.00") : (_pdf.experience / Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pdf.level + 1)), int.MaxValue) * 100).ToString("0.00") + "%") + "\n";
      output += "ExpToNextLevel:" + (_pl != null ? _pl.ExpToNextLevel : (int)_pdf.experience) + "\n";
      output += "ExpForNextLevel:" + (_pl != null ? _pl.GetExpForNextLevel() : (int)Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pdf.level + 1)), int.MaxValue)) + "\n";

      //add gamestage to persistent data
      output += "Gamestage:" + (_pl != null ? _pl.gameStage.ToString() : "") + "\n";
      output += "KilledPlayers:" + (_pl != null ? _pl.KilledPlayers : _pdf.playerKills) + "\n";
      output += "KilledZombies:" + (_pl != null ? _pl.KilledZombies : _pdf.zombieKills) + "\n";
      output += "Deaths:" + (_pl != null ? _pl.Died : _pdf.deaths) + "\n";
      output += "CurrentLife:" + (_pl != null ? _pl.currentLife.ToString("0.0") + "(mins)" : _pdf.currentLife.ToString("0.0") + "(mins)") + "\n";
      output += "LongestLife:" + (_pl != null ? _pl.longestLife.ToString("0.0") + "(mins)" : _pdf.longestLife.ToString("0.0") + "(mins)") + "\n";
      output += "Score:" + (_pl != null ? _pl.Score : _pdf.score) + "\n";

      if (_pl != null)
      {
        output += "onGround=" + _pl.onGround + "\n";
        output += "IsStuck=" + _pl.IsStuck + "\n";
        output += "IsSafeZoneActive=" + _pl.IsSafeZoneActive() + "\n";
        output += "Remote=" + _pl.isEntityRemote + "\n";
        output += "Dead=" + _pl.IsDead() + "\n";
        //add LastZombieAttackTime to persistent data
        output += "TimeSinceLastZombieAttack=" + ((GameManager.Instance.World.worldTime - _pl.LastZombieAttackTime) / 600).ToString("0.0") + "(mins)\n";
      }

      output += "RentedVendor:" + (_pl != null ? _pl.RentedVMPosition : _pdf.rentedVMPosition) + "\n";
      output += "RentedVendorExpire:" + (_pl != null ? _pl.RentalEndTime : _pdf.rentalEndTime) + "\n";

      output += new BuffList(_pdf, _pl).Display();

      output += "SkillPoints:" + _pdf.skillPoints + "\n";
      output += new SkillList(_pdf, _pl).Display();

      output += new QuestList(_pdf).Display();

      output += new SpawnpointList(_pdf).Display();

      output += "MarkerPosition:" + (_pdf.markerPosition != Vector3i.zero ? GameUtils.WorldPosToStr(_pdf.markerPosition.ToVector3(), " ") : "None") + "\n";
      output += new WaypointList(_pdf).Display();

      output += new FavoriteRecipeList(_pdf).Display();
      output += new UnlockedRecipeList(_pdf).Display();
      output += "ItemsCrafted:" + (_pl != null ? _pl.totalItemsCrafted : _pdf.totalItemsCrafted) + "\n";
      output += new CraftingQueue(_pdf).Display();


      //TOOLBELT
      // todo: FIX *********************************  itemValue.type == 0 for offline section???
      output += "SelectedItem:";
      if (_pl != null)
      {
        output += _pl.inventory.holdingItemIdx.ToString() + "[" + _pl.inventory.holdingItem.Name + "(" + _pl.inventory.holdingItem.Id + ")]";
      }
      else
      {
        output += _pdf.selectedInventorySlot.ToString();
        ItemStack i = _pdf.inventory[_pdf.selectedInventorySlot];
        int xt = i.itemValue.type;
        if (xt != 0)
        {
          ItemClass ic = ItemClass.list[xt];
          if (xt > 4096)
          {
            xt = xt - 4096;
          }
          output += "[" + ic.Name + "(" + ic.Id + ")]";
        }
        output += "\n";
      }

      output += "Inventory={\n";
      int idx = 1;
      if (_pl != null)
      {
        foreach (ItemStack i in _pl.inventory.GetSlots())
        {
          ItemStack xi = i;
          if (i.itemValue.type == 0)
          {
            // get items from _pdf until they have been held at least once to force an update
            xi = _pdf.inventory[idx];
          }
          int xt = i.itemValue.type;
          if (xt != 0)
          {
            ItemClass ic = ItemClass.list[xt];
            if (xt > 4096)
            {
              xt = xt - 4096;
            }
            output += idx + ":" + ic.Name + "(" + xt + ")*" + xi.count + "\n";
          }
          else
          {
            output += idx + ":";
          }
          idx++;
        }
      } else
      {
        foreach (ItemStack i in _pdf.inventory)
        {
          int xt = i.itemValue.type;
          if (xt != 0)
          {
            ItemClass ic = ItemClass.list[xt];
            if (xt > 4096)
            {
              xt = xt - 4096;
            }
            output += idx + ":" + ic.Name + "(" + xt + ")*" + i.count + "\n";
          } else
          {
            output += idx + ":\n";
          }
          idx++;
        }
      }
      output += "\n}\n";

      //WORN ITEMS
      output += "Equipment={\n";
      if (_pl != null)
      {
        foreach (ItemValue iv in _pl.equipment.GetItems())
        {
          if (iv.type != 0)
          {
            ItemClass ic = ItemClass.list[iv.type];
            int xt = iv.type;
            if (xt > 4096)
            {
              xt = xt - 4096;
            }
            output += ic.EquipSlot + ":" + ic.Name + "(" + xt + ")\n";
          }
        }
      }
      else
      {
        foreach (ItemValue iv in _pdf.equipment.GetItems())
        {
          int xt = iv.type;
          if (xt != 0)
          {
            ItemClass ic = ItemClass.list[xt];
            if (xt > 4096)
            {
              xt = xt - 4096;
            }
            output += ic.EquipSlot + ":" + ic.Name + "(" + xt + ")\n";
          }
        }
      }
      output += "\n}\n";

      //BAG
      output += "Bag={\n";
      idx = 1;
      foreach (ItemStack i in _pdf.bag)
      {
        int xt = i.itemValue.type;
        if (xt != 0)
        {
          ItemClass ic = ItemClass.list[xt];
          if (xt > 4096)
          {
            xt = xt - 4096;
          }
          output += idx + ":" + ic.Name + "(" + xt + ")*" + i.count + "\n";
        }
        else
        {
          output += idx + ":\n";
        }
        idx++;
      }
      output += "\n}\n";

      


      //friends
      //tracked friends // _pdf.trackedFriendEntityIds //List<int>
      //claims (_ppd.LPBlocks)


      return output;
    }
  }
}
