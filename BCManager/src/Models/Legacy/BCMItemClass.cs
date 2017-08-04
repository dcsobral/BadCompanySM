//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Text;
//using UnityEngine;
//using XMLData.Item;

//namespace BCM.Models.Legacy
//{
//  [Serializable]
//  public class BCMItemClass
//  {
//    #region properties
//    public int id;
//    public string name;
//    public string localizedName;

//    public DataItem<string> customIcon;
//    public Color customIconTint;

//    [NonSerialized]
//    public ItemClass itemClass;

//    //public DynamicProperties Properties;
//    //public AIDirectorData.Smell Smell;
//    //public string MeshFile;
//    //public string DropMeshFile;
//    //public string HandMeshFile;
//    //public ItemAction[] Actions;
//    //public MaterialBlock MadeOfMaterial;
//    //public DataItem<int> HoldType = new DataItem<int>(0);
//    //public DataItem<int> Stacknumber = new DataItem<int>(500);
//    //public DataItem<int> MaxUseTimes = new DataItem<int>(0);
//    //public DataItem<bool> MaxUseTimesBreaksAfter = new DataItem<bool>(false);
//    //public DataItem<string> LightSource;
//    //public EnumEquipmentSlot EquipSlot;
//    //public DataItem<string> ActivateObject;
//    //public DataItem<bool> ThrowableDecoy;
//    //public ArmorClass Armor;
//    //public ItemData.DataItemArrayRepairTools RepairTools;
//    //public DataItem<int> RepairAmount;
//    //public DataItem<float> RepairTime;
//    //public DataItem<float> CritChance;
//    //public string[] Groups = new string[] { "Decor/Miscellaneous" };
//    //public DataItem<string> CustomIcon;
//    //public Color CustomIconTint;
//    //public DataItem<bool> UserHidden = new DataItem<bool>(false);
//    //public string VehicleSlotType;
//    //public bool GetQualityFromWeapon;
//    //public string DescriptionKey;
//    //public bool IsResourceUnit;
//    //public float MeltTimePerUnit;
//    //public string ActionSkillGroup = string.Empty;
//    //public string CraftingSkillGroup = string.Empty;
//    //public bool UsableUnderwater = true;
//    //public bool HoldingItemHidden;
//    //public List<int> PartParentId;
//    //public bool HasParts;
//    //public DataItem<PartsData> PartTypes;
//    //public ItemClass.EnumParts Part;
//    //public List<AttributeBase> Attributes;
//    //public bool HasAttributes;
//    //public bool HasQuality;
//    //public bool bCraftingTool;
//    //public float CraftComponentExp = 3f;
//    //public float CraftComponentTime = 1f;
//    //public float LootExp = 1f;
//    //public float EconomicValue;
//    //public int EconomicBundleSize = 1;
//    //public bool SellableToTrader = true;
//    //public int CraftingSkillExp = 10;
//    //public int ActionSkillExp = 10;
//    //public float RepairExpMultiplier = 10f;
//    //public float Insulation;
//    //public float WaterProof;
//    //public float Encumbrance;
//    //public string SoundUnholster = "weapon_unholster";
//    //public string SoundHolster = "weapon_holster";
//    //public bool bShowCrosshairOnAiming;
//    //public bool bCrosshairUpAfterShot;
//    //public string PickupJournalEntry = string.Empty;
//    //public UMASlot UmaSlotData;
//    #endregion

//    public BCMItemClass()
//    {
//      //this.bCraftingTool = false;
//      //this.Properties = new DynamicProperties();
//      //this.EquipSlot = EnumEquipmentSlot.Count;
//      //this.Part = ItemClass.EnumParts.None;
//      //this.Attributes = new List<AttributeBase>();
//      //this.Actions = new ItemAction[5];
//      //for (int i = 0; i < this.Actions.Length; i++)
//      //{
//      //  this.Actions[i] = null;
//      //}
//    }

//    public BCMItemClass(ItemClass itemClass)
//    {
//      if (itemClass != null && itemClass.Id != 0)
//      {
//        Parse(itemClass);
//      }
//    }

//    public void Parse(ItemClass _itemClass)
//    {
//      id = _itemClass.Id > 4096 ? _itemClass.Id - 4096 : _itemClass.Id;
//      name = _itemClass.Name;
//      localizedName = _itemClass.localizedName;
//      customIcon = _itemClass.CustomIcon;
//      customIconTint = _itemClass.CustomIconTint;
//      itemClass = _itemClass;
      
//      ////object
//      //Smell = itemClass.Smell;
//      //Actions = itemClass.Actions;
//      //Properties = itemClass.Properties;
//      //MadeOfMaterial = itemClass.MadeOfMaterial;
//      //Armor = itemClass.Armor;
//      //RepairTools = itemClass.RepairTools;
//      //CustomIconTint = itemClass.CustomIconTint;
//      //PartTypes = itemClass.PartTypes;
//      //Part = itemClass.Part;
//      //Attributes = itemClass.Attributes;
//      //UmaSlotData = itemClass.UmaSlotData;

//      ////DataItem
//      //HoldType = itemClass.HoldType;
//      //Stacknumber = itemClass.Stacknumber;
//      //MaxUseTimes = itemClass.MaxUseTimes;
//      //MaxUseTimesBreaksAfter = itemClass.MaxUseTimesBreaksAfter;
//      //LightSource = itemClass.LightSource;
//      //ActivateObject = itemClass.ActivateObject;
//      //ThrowableDecoy = itemClass.ThrowableDecoy;
//      //RepairAmount = itemClass.RepairAmount;
//      //RepairTime = itemClass.RepairTime;
//      //CritChance = itemClass.CritChance;
//      //CustomIcon = itemClass.CustomIcon;
//      //UserHidden = itemClass.UserHidden;

//      ////Enum/list
//      //EquipSlot = itemClass.EquipSlot;
//      //PartParentId = itemClass.PartParentId;

//      ////string
//      //localizedName = itemClass.localizedName;
//      //MeshFile = itemClass.MeshFile;
//      //DropMeshFile = itemClass.DropMeshFile;
//      //HandMeshFile = itemClass.HandMeshFile;
//      //Groups = itemClass.Groups; //[]
//      //VehicleSlotType = itemClass.VehicleSlotType;
//      //GetQualityFromWeapon = itemClass.GetQualityFromWeapon; //bool
//      //DescriptionKey = itemClass.DescriptionKey;
//      //IsResourceUnit = itemClass.IsResourceUnit; //bool
//      //MeltTimePerUnit = itemClass.MeltTimePerUnit; //float
//      //ActionSkillGroup = itemClass.ActionSkillGroup;
//      //CraftingSkillGroup = itemClass.CraftingSkillGroup;
//      //UsableUnderwater = itemClass.UsableUnderwater; //bool
//      //HoldingItemHidden = itemClass.HoldingItemHidden; //bool
//      //HasParts = itemClass.HasParts; //bool
//      //HasAttributes = itemClass.HasAttributes; //bool
//      //HasQuality = itemClass.HasQuality; //bool
//      //bCraftingTool = itemClass.bCraftingTool; //bool
//      //CraftComponentExp = itemClass.CraftComponentExp; //float
//      //CraftComponentTime = itemClass.CraftComponentTime; //float
//      //LootExp = itemClass.LootExp; //float
//      //EconomicValue = itemClass.EconomicValue; //float
//      //EconomicBundleSize = itemClass.EconomicBundleSize; //int
//      //SellableToTrader = itemClass.SellableToTrader; //bool
//      //CraftingSkillExp = itemClass.CraftingSkillExp; //int
//      //ActionSkillExp = itemClass.ActionSkillExp; //int
//      //RepairExpMultiplier = itemClass.RepairExpMultiplier; //float
//      //Insulation = itemClass.Insulation; //float
//      //WaterProof = itemClass.WaterProof; //float
//      //Encumbrance = itemClass.Encumbrance; //float
//      //SoundUnholster = itemClass.SoundUnholster;
//      //SoundHolster = itemClass.SoundHolster;
//      //bShowCrosshairOnAiming = itemClass.bShowCrosshairOnAiming; //bool
//      //bCrosshairUpAfterShot = itemClass.bCrosshairUpAfterShot; //bool
//      //PickupJournalEntry = itemClass.PickupJournalEntry;
//  }

//    public string GetJson()
//    {
//      StringBuilder strb = new StringBuilder();
//      strb.Append("{");

//      strb.Append(string.Format("\"{0}\"", "name"));
//      strb.Append(":");
//      strb.Append(string.Format("\"{0}\"", name));
//      strb.Append(",");

//      strb.Append(string.Format("\"{0}\"", "localizedName"));
//      strb.Append(":");
//      strb.Append(string.Format("\"{0}\"", localizedName));
//      strb.Append(",");

//      if (customIcon != null && customIcon.Value != null)
//      {
//        strb.Append(string.Format("\"{0}\"", "customIcon"));
//        strb.Append(":");
//        strb.Append(string.Format("\"{0}\"", customIcon.Value));
//        strb.Append(",");
//      }

//      if (customIconTint != null && !(customIconTint.r == 1 && customIconTint.g == 1 && customIconTint.b == 1))
//      {
//        strb.Append(string.Format("\"{0}\"", "customIconTint"));
//        strb.Append(":");
//        strb.Append(string.Format("\"{0}\"", BCUtils.ColorToHex(customIconTint)));
//        strb.Append(",");
//      }

//      strb.Append(string.Format("\"{0}\"", "id"));
//      strb.Append(":");
//      strb.Append(string.Format("\"{0}\"", id));

//      strb.Append("}");
//      return strb.ToString();
//    }
//  public string GetJsonLong()
//    {
//      StringBuilder strb = new StringBuilder();
//      strb.Append("{");

//      strb.Append(string.Format("\"{0}\"", "localizedName"));
//      strb.Append(":");
//      strb.Append(string.Format("\"{0}\"", localizedName));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "MeshFile"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", MeshFile));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "DropMeshFile"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", DropMeshFile));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "HandMeshFile"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", HandMeshFile));
//      //strb.Append(",");

//      ////Groups = itemClass.Groups; //[]
//      ////strb.Append(string.Format("\"{0}\"", "Groups"));
//      ////strb.Append(":");
//      ////strb.Append(string.Format("\"{0}\"", Groups));
//      ////strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "VehicleSlotType"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", VehicleSlotType));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "GetQualityFromWeapon"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", GetQualityFromWeapon));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "DescriptionKey"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", DescriptionKey));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "IsResourceUnit"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", IsResourceUnit));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "MeltTimePerUnit"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", MeltTimePerUnit));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "ActionSkillGroup"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", ActionSkillGroup));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "CraftingSkillGroup"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", CraftingSkillGroup));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "UsableUnderwater"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", UsableUnderwater));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "HoldingItemHidden"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", HoldingItemHidden));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "HasParts"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", HasParts));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "HasAttributes"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", HasAttributes));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "HasQuality"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", HasQuality));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "bCraftingTool"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", bCraftingTool));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "CraftComponentExp"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", CraftComponentExp));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "CraftComponentTime"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", CraftComponentTime));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "LootExp"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", LootExp));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "EconomicValue"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", EconomicValue));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "EconomicBundleSize"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", EconomicBundleSize));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "SellableToTrader"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", SellableToTrader));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "CraftingSkillExp"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", CraftingSkillExp));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "ActionSkillExp"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", ActionSkillExp));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "RepairExpMultiplier"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", RepairExpMultiplier));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "Insulation"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", Insulation));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "WaterProof"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", WaterProof));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "Encumbrance"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", Encumbrance));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "SoundUnholster"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", SoundUnholster));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "SoundHolster"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", SoundHolster));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "bShowCrosshairOnAiming"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", bShowCrosshairOnAiming));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "bCrosshairUpAfterShot"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", bCrosshairUpAfterShot));
//      //strb.Append(",");

//      //strb.Append(string.Format("\"{0}\"", "PickupJournalEntry"));
//      //strb.Append(":");
//      //strb.Append(string.Format("\"{0}\"", PickupJournalEntry));

//      strb.Append("}");
//      return strb.ToString();
//    }

//  }
//}
