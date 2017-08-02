using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMItemClass
  {
    private Dictionary<string, string> options = new Dictionary<string, string>();
    private Dictionary<string, object> _bin = new Dictionary<string, object>();
    public enum Filters
    {
      Id,
      Name,
      LocalizedName,
      Properties
    }
    public static class StrFilters
    {
      public static string Id = "id";
      public static string Name = "name";
      public static string LocalizedName = "local";
      public static string Properties = "props";
    }

    private List<string> _filter = new List<string>();

    #region Properties
    public int Id;
    public string Name;
    public string LocalizedName;
    public Dictionary<string, string> Properties = new Dictionary<string, string>();
    #endregion;

    private bool useInt = false;
    private List<string> strFilter = new List<string>();
    private List<int> intFilter = new List<int>();
    
    public BCMItemClass(ItemClass _item, Dictionary<string, string> _options)
    {
      options = _options;
      Load(_item);
    }

    public Dictionary<string, object> Data ()
    {
      return _bin;
    }

    public void Load(ItemClass _item)
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

      GetItemInfo(_item);

    }

    private void GetItemInfo(ItemClass _item)
    {
      if (isOption("filter"))
      {
        //ID
        if ((useInt && intFilter.Contains((int)Filters.Id)) || (!useInt && strFilter.Contains(StrFilters.Id)))
        {
          Id = _item.Id;
          _bin.Add("Id", Id);
        }

        //NAME
        if ((useInt && intFilter.Contains((int)Filters.Name)) || (!useInt && strFilter.Contains(StrFilters.Name)))
        {
          Name = _item.Name;
          _bin.Add("Name", Name);
        }

        //LOCAL NAME
        if ((useInt && intFilter.Contains((int)Filters.LocalizedName)) || (!useInt && strFilter.Contains(StrFilters.LocalizedName)))
        {
          LocalizedName = _item.localizedName;
          _bin.Add("LocalizedName", LocalizedName);
        }

        //PROPERTIES
        if ((useInt && intFilter.Contains((int)Filters.Properties)) || (!useInt && strFilter.Contains(StrFilters.Properties)))
        {
          foreach (string current in _item.Properties.Values.Keys)
          {
            if (_item.Properties.Values.ContainsKey(current))
            {
              Properties.Add(current, _item.Properties.Values[current]);
            }
          }
          _bin.Add("Properties", Properties);
        }


      }
      else
      {
        Id = _item.Id;
        Name = _item.Name;
        LocalizedName = _item.localizedName;
        //properties
        foreach (string current in _item.Properties.Values.Keys)
        {
          if (_item.Properties.Values.ContainsKey(current))
          {
            Properties.Add(current, _item.Properties.Values[current]);
          }
        }

        _bin.Add("Id", Id);
        _bin.Add("Name", Name);
        _bin.Add("LocalizedName", LocalizedName);
        _bin.Add("Properties", Properties);
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
      var postype = "";
      if (options.ContainsKey("worldpos"))
      {
        postype = "worldpos";
      }
      if (options.ContainsKey("csvpos"))
      {
        postype = "csvpos";
      }

      return postype;
    }

  }
}



//private string getColor(Color _c)
//{
//  return "RGBA(" + _c.r.ToString() + ", " + _c.g.ToString() + ", " + _c.a.ToString() + ", " + _c.a.ToString() + ")";
//}
//public virtual Dictionary<string, string> jsonObject()
//{
//  Dictionary<string, string> data = new Dictionary<string, string>();

//  for (var i = 0; i <= ItemClass.list.Length - 1; i++)
//  {
//    if (ItemClass.list[i] != null)
//    {
//      var ic = ItemClass.list[i];
//      Dictionary<string, string> details = new Dictionary<string, string>();

//      details.Add("Id", ic.Id.ToString());
//      details.Add("Name", ic.Name);
//      details.Add("localizedName", (ic.localizedName != null ? ic.localizedName : ""));
//      details.Add("IsBlock", ic.IsBlock().ToString());

//      if (ic.IsBlock())
//      {
//        details.Add("ActionSkillExp", ic.ActionSkillExp.ToString());
//        //details.Add("ActionSkillGroup", ic.ActionSkillGroup);
//        //details.Add("ActivateObject", (ic.ActivateObject != null ? ic.ActivateObject.Value : ""));
//        details.Add("AutoCalcEcoVal", ic.AutoCalcEcoVal().ToString());
//        details.Add("AutoCalcWeight", ic.AutoCalcWeight().ToString());
//        details.Add("bCraftingTool", ic.bCraftingTool.ToString());
//        //details.Add("bCrosshairUpAfterShot", ic.bCrosshairUpAfterShot.ToString());
//        //details.Add("bShowCrosshairOnAiming", ic.bShowCrosshairOnAiming.ToString());
//        details.Add("CanDrop", ic.CanDrop().ToString());
//        //details.Add("CanHold", ic.CanHold().ToString());
//        details.Add("CraftComponentExp", ic.CraftComponentExp.ToString());
//        details.Add("CraftComponentTime", ic.CraftComponentTime.ToString());
//        details.Add("CraftingSkillExp", ic.CraftingSkillExp.ToString());
//        details.Add("CraftingSkillGroup", ic.CraftingSkillGroup);
//        //details.Add("CritChance", ic.CritChance.Value.ToString());
//        //details.Add("CustomIconTint", getColor(ic.CustomIconTint));
//        details.Add("DescriptionKey", ic.DescriptionKey);
//        details.Add("EconomicBundleSize", ic.EconomicBundleSize.ToString());
//        details.Add("EconomicValue", ic.EconomicValue.ToString());
//        //details.Add("Encumbrance", ic.Encumbrance.ToString());
//        //details.Add("EquipSlot", ic.EquipSlot.ToString());
//        details.Add("GetIconName", ic.GetIconName());
//        //details.Add("GetIconTint", getColor(ic.GetIconTint()));
//        details.Add("GetItemDescriptionKey", ic.GetItemDescriptionKey());
//        //details.Add("GetItemName", ic.GetItemName());
//        details.Add("GetWeight", ic.GetWeight().ToString());
//        //details.Add("Weight", (ic.Weight != null ? ic.Weight.Value.ToString() : "null"));
//        //details.Add("GetType", ic.GetType().ToString());
//        //details.Add("GetQualityFromWeapon", ic.GetQualityFromWeapon.ToString());
//        //details.Add("HasAttributes", ic.HasAttributes.ToString());
//        //details.Add("HasParts", ic.HasParts.ToString());
//        //details.Add("HasQuality", ic.HasQuality.ToString());
//        details.Add("HoldType", ic.HoldType.Value.ToString());
//        //details.Add("Insulation", ic.Insulation.ToString());
//        //details.Add("IsGun", ic.IsGun().ToString());
//        //details.Add("IsLightSource", ic.IsLightSource().ToString());
//        details.Add("IsResourceUnit", ic.IsResourceUnit.ToString());
//        details.Add("LootExp", ic.LootExp.ToString());//always 1?
//        details.Add("MadeOfMaterial", ic.MadeOfMaterial.id);
//        //details.Add("MaxUseTimes", ic.MaxUseTimes.Value.ToString());
//        //details.Add("MaxUseTimesBreaksAfter", ic.MaxUseTimesBreaksAfter.Value.ToString());
//        details.Add("MeltTimePerUnit", ic.MeltTimePerUnit.ToString());
//        details.Add("RepairExpMultiplier", ic.RepairExpMultiplier.ToString());
//        //details.Add("SoundHolster", ic.SoundHolster);
//        //details.Add("SoundUnholster", ic.SoundUnholster);
//        //details.Add("SoundJammed", (ic.SoundJammed != null ? ic.SoundJammed.Value : "null"));
//        details.Add("SellableToTrader", ic.SellableToTrader.ToString());
//        details.Add("CanStack", ic.CanStack().ToString());
//        details.Add("Stacknumber", ic.Stacknumber.Value.ToString());
//        //details.Add("UserHidden", ic.UserHidden.Value.ToString());
//        //details.Add("UsableUnderwater", ic.UsableUnderwater.ToString());
//        //details.Add("WaterProof", ic.WaterProof.ToString());
//        //details.Add("LightSource", (ic.LightSource != null ? ic.LightSource.Value.ToString() : ""));
//        //details.Add("HandMeshFile", (ic.HandMeshFile != null ? ic.HandMeshFile : ""));
//        //details.Add("MeshFile", (ic.MeshFile != null ? ic.MeshFile : ""));
//        //details.Add("VehicleSlotType", (ic.VehicleSlotType != null ? ic.VehicleSlotType : "null"));
//        //details.Add("ThrowableDecoy", (ic.ThrowableDecoy != null ? ic.ThrowableDecoy.Value.ToString() : "null"));
//        //details.Add("PartTypes_Barrel", (ic.PartTypes != null ? ic.PartTypes.Value.Barrel.Value.Id.ToString() : "null"));
//        //details.Add("PartTypes_Pump", (ic.PartTypes != null ? ic.PartTypes.Value.Pump.Value.Id.ToString() : "null"));
//        //details.Add("PartTypes_Receiver", (ic.PartTypes != null ? ic.PartTypes.Value.Receiver.Value.Id.ToString() : "null"));
//        //details.Add("PartTypes_Stock", (ic.PartTypes != null ? ic.PartTypes.Value.Stock.Value.Id.ToString() : "null"));
//        //details.Add("PickupJournalEntry", (ic.PickupJournalEntry != null ? ic.PickupJournalEntry : "null"));
//        //details.Add("DropMeshFile", (ic.DropMeshFile != null ? ic.DropMeshFile : "null"));
//        //details.Add("RepairAmount", (ic.RepairAmount != null ? ic.RepairAmount.Value.ToString() : "null"));
//        //details.Add("RepairTime", (ic.RepairTime != null ? ic.RepairTime.Value.ToString() : "null"));
//      } else
//      {
//        //DYNAMIC PROPERTIES
//        Dictionary<string, string> properties = new Dictionary<string, string>();
//        foreach (string current in ic.Properties.Values.Keys)
//        {
//          if (ic.Properties.Values.ContainsKey(current))
//          {
//            properties.Add(current, ic.Properties.Values[current].ToString());
//          }
//        }
//        string jsonProperties = BCUtils.toJson(properties);
//        details.Add("Properties", jsonProperties);
//      }

//      //List<string> actions = new List<string>();
//      //foreach(ItemAction _action in ic.Actions)
//      //{
//      //  if (_action != null)
//      //  {
//      //    Dictionary<string, string> action = new Dictionary<string, string>();

//      //    action.Add("ActionIdx", (_action.ActionIdx != null ? _action.ActionIdx.ToString() : ""));

//      //    foreach (string current in _action.Properties.Values.Keys)
//      //    {
//      //      if (_action.Properties.Values.ContainsKey(current))
//      //      {
//      //        action.Add("Properties." + current, _action.Properties.Values[current].ToString());
//      //        //if (_action.Properties.Params1.ContainsKey(current))
//      //        //{
//      //        //  text += string.Format("param1=\"{0}\" ", _action.Properties.Params1[current]);
//      //        //}
//      //        //if (_action.Properties.Params2.ContainsKey(current))
//      //        //{
//      //        //  text += string.Format("param2=\"{0}\" ", _action.Properties.Params2[current]);
//      //        //}
//      //      }
//      //    }

//      //    string jsonAction = BCUtils.GenerateJson(action);
//      //    actions.Add(jsonAction);
//      //  }
//      //}

//      //string jsonActions = BCUtils.GenerateJson(actions);
//      //details.Add("Actions", jsonActions);

//      //details.Add("Armor_Absorbtion", ic.Armor.Absorbtion.Length.ToString());
//      //details.Add("Attributes", ic.Attributes.Count.ToString());

//      //details.Add(ic.Armor.ToString());//.
//      //details.Add(ic.Attributes.ToString());//.
//      //details.Add(ic.Explosion.ToString());//.
//      //details.Add("Attachments", ic.Attachments.ToString());//.
//      //details.Add(ic.Groups.ToString());//.
//      //details.Add("Part", ic.Part.ToString());//.
//      //details.Add(ic.PartParentId.ToString());//.
//      //details.Add(ic.Smell.ToString());//.
//      //details.Add(ic.UMA.ToString());//.
//      //details.Add(ic.UmaSlotData.ToString());//.
//      //details.Add(ic.RepairTools.ToString());//.


//      var jsonDetails = BCUtils.toJson(details);

//      data.Add(ItemClass.list[i].Id.ToString(), jsonDetails);
//    }
//  }

//  return data;
//}
