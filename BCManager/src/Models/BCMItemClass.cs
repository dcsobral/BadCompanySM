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
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name },
      { 2,  StrFilters.Local },
      { 3,  StrFilters.IsBlock },
      { 4,  StrFilters.Material },
      { 5,  StrFilters.StackNumber }
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
    #endregion;

    public BCMItemClass(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var item = obj as ItemClass;
      if (item == null) return;

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
      }
    }

    private void GetStackNumber(ItemClass item) => Bin.Add("StackNumber", StackNumber = item.Stacknumber.Value);

    private void GetMaterial(ItemClass item) => Bin.Add("Material", Material = item.MadeOfMaterial?.id);

    private void GetIsBlock(ItemClass item) => Bin.Add("IsBlock", IsBlock = item.IsBlock());

    private void GetLocal(ItemClass item) => Bin.Add("Local", Local = item.GetLocalizedItemName() ?? item.GetItemName());

    private void GetName(ItemClass itemClass) => Bin.Add("Name", Name = itemClass.Name);

    private void GetId(ItemClass itemClass) => Bin.Add("Id", Id = itemClass.Id);
  }
}
