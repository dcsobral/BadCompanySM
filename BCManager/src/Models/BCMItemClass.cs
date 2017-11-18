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
      public const string Local = "local";
      public const string IsBlock = "block";
      public const string Material = "material";
      public const string StackNumber = "stack";
      public const string Icon = "icon";
      public const string IconTint = "icontint";
      public const string CanSpawn = "spawn";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name },
      { 2,  StrFilters.Local },
      { 3,  StrFilters.IsBlock },
      { 4,  StrFilters.Material },
      { 5,  StrFilters.StackNumber },
      { 6,  StrFilters.Icon },
      { 7,  StrFilters.IconTint },
      { 8, StrFilters.CanSpawn }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public int Id;
    public string Name;
    public string Local;
    public bool IsBlock;
    public string Material;
    public int StackNumber;
    public string Icon;
    public string IconTint;
    public bool CanSpawn;
    #endregion;

    public BCMItemClass(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (!(obj is ItemClass item)) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(item);
              break;
            case StrFilters.Name:
              GetName(item);
              break;
            case StrFilters.Local:
              GetLocal(item);
              break;
            case StrFilters.IsBlock:
              GetIsBlock(item);
              break;
            case StrFilters.Material:
              GetMaterial(item);
              break;
            case StrFilters.StackNumber:
              GetStackNumber(item);
              break;
            case StrFilters.Icon:
              GetIcon(item);
              break;
            case StrFilters.IconTint:
              GetIconTint(item);
              break;
            case StrFilters.CanSpawn:
              GetCanSpawn(item);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(item);
        GetName(item);
        GetLocal(item);
        GetIsBlock(item);
        GetMaterial(item);
        GetStackNumber(item);
        GetIcon(item);
        GetIconTint(item);
        GetCanSpawn(item);

        if (Options.ContainsKey("properties"))
        {
          Bin.Add("Properties.Values", item.Properties.Values);
          Bin.Add("Properties.Params1", item.Properties.Params1);
          Bin.Add("Properties.Params2", item.Properties.Params2);
          Bin.Add("Properties.Classes", item.Properties.Classes);
        }
      }
    }

    private void GetCanSpawn(ItemClass item) => Bin.Add("CanSpawn", CanSpawn = item.CreativeMode != "None" );

    private void GetIconTint(ItemClass item) => Bin.Add("IconTint", IconTint = item.GetIconTint().ToStringRgbHex(hash:false));

    private void GetIcon(ItemClass item) => Bin.Add("Icon", Icon = item.GetIconName());

    private void GetStackNumber(ItemClass item) => Bin.Add("StackNumber", StackNumber = item.Stacknumber.Value);

    private void GetMaterial(ItemClass item) => Bin.Add("Material", Material = item.MadeOfMaterial?.id);

    private void GetIsBlock(ItemClass item) => Bin.Add("IsBlock", IsBlock = item.IsBlock());

    private void GetLocal(ItemClass item) => Bin.Add("Local", Local = item.GetLocalizedItemName() ?? item.GetItemName());

    private void GetName(ItemClass itemClass) => Bin.Add("Name", Name = itemClass.Name);

    private void GetId(ItemClass itemClass) => Bin.Add("Id", Id = itemClass.Id);
  }
}
