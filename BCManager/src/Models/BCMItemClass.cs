using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMItemClass : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Name = "name";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;

    #endregion

    #region Properties
    public int Id;
    public string Name;
    #endregion;

    public BCMItemClass(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var itemClass = obj as ItemClass;
      if (itemClass == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(itemClass);
              break;
            case StrFilters.Name:
              GetName(itemClass);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(itemClass);
        GetName(itemClass);

      }
    }

    private void GetName(ItemClass itemClass) => Bin.Add("Name", Name = itemClass.Name);

    private void GetId(ItemClass itemClass) => Bin.Add("Id", Id = itemClass.Id);
  }
}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace BCM.Models
//{
//  [Serializable]
//  public class BCMItemClass : BCMAbstract
//  {
//    #region Filters
//    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
//    {
//      { 0,  "name" },
//    };
//    public static Dictionary<int, string> FilterMap
//    {
//      get => _filterMap;
//      set => _filterMap = value;
//    }

//    public static class StrFilters
//    {
//      public static string Name = "name";
//    }
//    #endregion

//    #region Properties
//    public string Name;
//    #endregion;

//    public BCMItemClass(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
//    {
//    }

//    public override void GetData(object obj)
//    {

//    }

//    //public override void GetData(object _obj)
//    //{
//    //  var _item = _obj as ItemClass;
//    //  if (IsOption("filter"))
//    //  {
//    //    //Type
//    //    if ((UseInt && IntFilter.Contains((int)Filters.Type)) || (!UseInt && StrFilter.Contains(StrFilters.Type)))
//    //    {
//    //      Type = _item.Id;
//    //      Bin.Add("Type", Type);
//    //    }

//    //    //NAME
//    //    if ((UseInt && IntFilter.Contains((int)Filters.Name)) || (!UseInt && StrFilter.Contains(StrFilters.Name)))
//    //    {
//    //      Name = _item.Name;
//    //      Bin.Add("Name", Name);
//    //    }

//    //    if (_item.IsBlock())
//    //    {

//    //    }
//    //    else
//    //    {
//    //      //LOCAL NAME
//    //      if ((UseInt && IntFilter.Contains((int)Filters.LocalizedName)) || (!UseInt && StrFilter.Contains(StrFilters.LocalizedName)))
//    //      {
//    //        LocalizedName = _item.localizedName;
//    //        Bin.Add("LocalizedName", LocalizedName);
//    //      }

//    //      ////PROPERTIES
//    //      //if ((useInt && intFilter.Contains((int)Filters.Properties)) || (!useInt && strFilter.Contains(StrFilters.Properties)))
//    //      //{
//    //      //  foreach (string current in _item.Properties.Values.Keys)
//    //      //  {
//    //      //    if (_item.Properties.Values.ContainsKey(current))
//    //      //    {
//    //      //      Properties.Add(current, _item.Properties.Values[current]);
//    //      //    }
//    //      //  }
//    //      //  _bin.Add("Properties", Properties);
//    //      //}
//    //    }


//    //  }
//    //  else
//    //  {
//    //    Type = _item.Id;
//    //    Name = _item.Name;
//    //    if (_item.IsBlock())
//    //    {

//    //      ActionSkillExp = _item.ActionSkillExp;
//    //      CraftingTool = _item.bCraftingTool;
//    //      CanDrop = _item.CanDrop();
//    //      CraftComponentExp = _item.CraftComponentExp;
//    //      CraftComponentTime = _item.CraftComponentTime;
//    //      CraftingSkillExp = _item.CraftingSkillExp;
//    //      CraftingSkillGroup = _item.CraftingSkillGroup;
//    //      DescriptionKey = _item.DescriptionKey;
//    //      EconomicBundleSize = _item.EconomicBundleSize;
//    //      EconomicValue = _item.EconomicValue;
//    //      GetIconName = _item.GetIconName();
//    //      GetItemDescriptionKey = _item.GetItemDescriptionKey();
//    //      Weight = _item.GetWeight();
//    //      HoldType = _item.HoldType.Value;
//    //      IsResourceUnit = _item.IsResourceUnit;
//    //      LootExp = _item.LootExp;
//    //      MadeOfMaterial = _item.MadeOfMaterial.id;
//    //      MeltTimePerUnit = _item.MeltTimePerUnit;
//    //      RepairExpMultiplier = _item.RepairExpMultiplier;
//    //      SellableToTrader = _item.SellableToTrader;
//    //      CanStack = _item.CanStack();
//    //      Stacknumber = _item.Stacknumber.Value;

//    //    }
//    //    else
//    //    {
//    //      LocalizedName = _item.localizedName;
//    //      //properties
//    //      foreach (string current in _item.Properties.Values.Keys)
//    //      {
//    //        if (_item.Properties.Values.ContainsKey(current))
//    //        {
//    //          Properties.Add(current, _item.Properties.Values[current]);
//    //        }
//    //      }
//    //    }

//    //    Bin.Add("Type", Type);
//    //    Bin.Add("Name", Name);
//    //    if (_item.IsBlock())
//    //    {
//    //      Bin.Add("ActionSkillExp", ActionSkillExp);
//    //      Bin.Add("CraftingSkillExp", CraftingSkillExp);
//    //      Bin.Add("CraftingSkillGroup", CraftingSkillGroup);
//    //      Bin.Add("RepairExpMultiplier", RepairExpMultiplier);
//    //      Bin.Add("LootExp", LootExp);
//    //      Bin.Add("CraftComponentExp", CraftComponentExp);
//    //      Bin.Add("CraftComponentTime", CraftComponentTime);

//    //      Bin.Add("Weight", Weight);

//    //      Bin.Add("HoldType", HoldType);
//    //      Bin.Add("MadeOfMaterial", MadeOfMaterial);
//    //      Bin.Add("MeltTimePerUnit", MeltTimePerUnit);

//    //      Bin.Add("EconomicBundleSize", EconomicBundleSize);
//    //      Bin.Add("EconomicValue", EconomicValue);

//    //      Bin.Add("CraftingTool", CraftingTool);

//    //      Bin.Add("CanStack", CanStack);
//    //      Bin.Add("Stacknumber", Stacknumber);
//    //      Bin.Add("CanDrop", CanDrop);
//    //      Bin.Add("IsResourceUnit", IsResourceUnit);
//    //      Bin.Add("SellableToTrader", SellableToTrader);

//    //      Bin.Add("DescriptionKey", DescriptionKey);
//    //      Bin.Add("GetItemDescriptionKey", GetItemDescriptionKey);
//    //      Bin.Add("GetIconName", GetIconName);

//    //    }
//    //    else
//    //    {
//    //      Bin.Add("LocalizedName", LocalizedName);
//    //      Bin.Add("Properties", Properties);
//    //    }
//    //  }
//    //}
//  }
//}




////public virtual Dictionary<string, string> jsonObject()
////{
////  Dictionary<string, string> data = new Dictionary<string, string>();

////  for (var i = 0; i <= ItemClass.list.Length - 1; i++)
////  {
////    if (ItemClass.list[i] != null)
////    {
////      var ic = ItemClass.list[i];
////      Dictionary<string, string> details = new Dictionary<string, string>();

////      details.Add("Id", ic.Id.ToString());
////      details.Add("Name", ic.Name);
////      details.Add("localizedName", (ic.localizedName != null ? ic.localizedName : ""));
////      details.Add("IsBlock", ic.IsBlock().ToString());

////      if (ic.IsBlock())
////      {
////        details.Add("ActionSkillExp", ic.ActionSkillExp.ToString());
////        //details.Add("ActionSkillGroup", ic.ActionSkillGroup);
////        //details.Add("ActivateObject", (ic.ActivateObject != null ? ic.ActivateObject.Value : ""));
////        details.Add("AutoCalcEcoVal", ic.AutoCalcEcoVal().ToString());
////        details.Add("AutoCalcWeight", ic.AutoCalcWeight().ToString());
////        details.Add("bCraftingTool", ic.bCraftingTool.ToString());
////        //details.Add("bCrosshairUpAfterShot", ic.bCrosshairUpAfterShot.ToString());
////        //details.Add("bShowCrosshairOnAiming", ic.bShowCrosshairOnAiming.ToString());
////        details.Add("CanDrop", ic.CanDrop().ToString());
////        //details.Add("CanHold", ic.CanHold().ToString());
////        details.Add("CraftComponentExp", ic.CraftComponentExp.ToString());
////        details.Add("CraftComponentTime", ic.CraftComponentTime.ToString());
////        details.Add("CraftingSkillExp", ic.CraftingSkillExp.ToString());
////        details.Add("CraftingSkillGroup", ic.CraftingSkillGroup);
////        //details.Add("CritChance", ic.CritChance.Value.ToString());
////        //details.Add("CustomIconTint", getColor(ic.CustomIconTint));
////        details.Add("DescriptionKey", ic.DescriptionKey);
////        details.Add("EconomicBundleSize", ic.EconomicBundleSize.ToString());
////        details.Add("EconomicValue", ic.EconomicValue.ToString());
////        //details.Add("Encumbrance", ic.Encumbrance.ToString());
////        //details.Add("EquipSlot", ic.EquipSlot.ToString());
////        details.Add("GetIconName", ic.GetIconName());
////        //details.Add("GetIconTint", getColor(ic.GetIconTint()));
////        details.Add("GetItemDescriptionKey", ic.GetItemDescriptionKey());
////        //details.Add("GetItemName", ic.GetItemName());
////        details.Add("GetWeight", ic.GetWeight().ToString());
////        //details.Add("Weight", (ic.Weight != null ? ic.Weight.Value.ToString() : "null"));
////        //details.Add("GetType", ic.GetType().ToString());
////        //details.Add("GetQualityFromWeapon", ic.GetQualityFromWeapon.ToString());
////        //details.Add("HasAttributes", ic.HasAttributes.ToString());
////        //details.Add("HasParts", ic.HasParts.ToString());
////        //details.Add("HasQuality", ic.HasQuality.ToString());
////        details.Add("HoldType", ic.HoldType.Value.ToString());
////        //details.Add("Insulation", ic.Insulation.ToString());
////        //details.Add("IsGun", ic.IsGun().ToString());
////        //details.Add("IsLightSource", ic.IsLightSource().ToString());
////        details.Add("IsResourceUnit", ic.IsResourceUnit.ToString());
////        details.Add("LootExp", ic.LootExp.ToString());//always 1?
////        details.Add("MadeOfMaterial", ic.MadeOfMaterial.id);
////        //details.Add("MaxUseTimes", ic.MaxUseTimes.Value.ToString());
////        //details.Add("MaxUseTimesBreaksAfter", ic.MaxUseTimesBreaksAfter.Value.ToString());
////        details.Add("MeltTimePerUnit", ic.MeltTimePerUnit.ToString());
////        details.Add("RepairExpMultiplier", ic.RepairExpMultiplier.ToString());
////        //details.Add("SoundHolster", ic.SoundHolster);
////        //details.Add("SoundUnholster", ic.SoundUnholster);
////        //details.Add("SoundJammed", (ic.SoundJammed != null ? ic.SoundJammed.Value : "null"));
////        details.Add("SellableToTrader", ic.SellableToTrader.ToString());
////        details.Add("CanStack", ic.CanStack().ToString());
////        details.Add("Stacknumber", ic.Stacknumber.Value.ToString());
////        //details.Add("UserHidden", ic.UserHidden.Value.ToString());
////        //details.Add("UsableUnderwater", ic.UsableUnderwater.ToString());
////        //details.Add("WaterProof", ic.WaterProof.ToString());
////        //details.Add("LightSource", (ic.LightSource != null ? ic.LightSource.Value.ToString() : ""));
////        //details.Add("HandMeshFile", (ic.HandMeshFile != null ? ic.HandMeshFile : ""));
////        //details.Add("MeshFile", (ic.MeshFile != null ? ic.MeshFile : ""));
////        //details.Add("VehicleSlotType", (ic.VehicleSlotType != null ? ic.VehicleSlotType : "null"));
////        //details.Add("ThrowableDecoy", (ic.ThrowableDecoy != null ? ic.ThrowableDecoy.Value.ToString() : "null"));
////        //details.Add("PartTypes_Barrel", (ic.PartTypes != null ? ic.PartTypes.Value.Barrel.Value.Id.ToString() : "null"));
////        //details.Add("PartTypes_Pump", (ic.PartTypes != null ? ic.PartTypes.Value.Pump.Value.Id.ToString() : "null"));
////        //details.Add("PartTypes_Receiver", (ic.PartTypes != null ? ic.PartTypes.Value.Receiver.Value.Id.ToString() : "null"));
////        //details.Add("PartTypes_Stock", (ic.PartTypes != null ? ic.PartTypes.Value.Stock.Value.Id.ToString() : "null"));
////        //details.Add("PickupJournalEntry", (ic.PickupJournalEntry != null ? ic.PickupJournalEntry : "null"));
////        //details.Add("DropMeshFile", (ic.DropMeshFile != null ? ic.DropMeshFile : "null"));
////        //details.Add("RepairAmount", (ic.RepairAmount != null ? ic.RepairAmount.Value.ToString() : "null"));
////        //details.Add("RepairTime", (ic.RepairTime != null ? ic.RepairTime.Value.ToString() : "null"));
////      } else
////      {
////        //DYNAMIC PROPERTIES
////        Dictionary<string, string> properties = new Dictionary<string, string>();
////        foreach (string current in ic.Properties.Values.Keys)
////        {
////          if (ic.Properties.Values.ContainsKey(current))
////          {
////            properties.Add(current, ic.Properties.Values[current].ToString());
////          }
////        }
////        string jsonProperties = BCUtils.toJson(properties);
////        details.Add("Properties", jsonProperties);
////      }

////      //List<string> actions = new List<string>();
////      //foreach(ItemAction _action in ic.Actions)
////      //{
////      //  if (_action != null)
////      //  {
////      //    Dictionary<string, string> action = new Dictionary<string, string>();

////      //    action.Add("ActionIdx", (_action.ActionIdx != null ? _action.ActionIdx.ToString() : ""));

////      //    foreach (string current in _action.Properties.Values.Keys)
////      //    {
////      //      if (_action.Properties.Values.ContainsKey(current))
////      //      {
////      //        action.Add("Properties." + current, _action.Properties.Values[current].ToString());
////      //        //if (_action.Properties.Params1.ContainsKey(current))
////      //        //{
////      //        //  text += string.Format("param1=\"{0}\" ", _action.Properties.Params1[current]);
////      //        //}
////      //        //if (_action.Properties.Params2.ContainsKey(current))
////      //        //{
////      //        //  text += string.Format("param2=\"{0}\" ", _action.Properties.Params2[current]);
////      //        //}
////      //      }
////      //    }

////      //    string jsonAction = BCUtils.GenerateJson(action);
////      //    actions.Add(jsonAction);
////      //  }
////      //}

////      //string jsonActions = BCUtils.GenerateJson(actions);
////      //details.Add("Actions", jsonActions);

////      //details.Add("Armor_Absorbtion", ic.Armor.Absorbtion.Length.ToString());
////      //details.Add("Attributes", ic.Attributes.Count.ToString());

////      //details.Add(ic.Armor.ToString());//.
////      //details.Add(ic.Attributes.ToString());//.
////      //details.Add(ic.Explosion.ToString());//.
////      //details.Add("Attachments", ic.Attachments.ToString());//.
////      //details.Add(ic.Groups.ToString());//.
////      //details.Add("Part", ic.Part.ToString());//.
////      //details.Add(ic.PartParentId.ToString());//.
////      //details.Add(ic.Smell.ToString());//.
////      //details.Add(ic.UMA.ToString());//.
////      //details.Add(ic.UmaSlotData.ToString());//.
////      //details.Add(ic.RepairTools.ToString());//.


////      var jsonDetails = BCUtils.toJson(details);

////      data.Add(ItemClass.list[i].Id.ToString(), jsonDetails);
////    }
////  }

////  return data;
////}
