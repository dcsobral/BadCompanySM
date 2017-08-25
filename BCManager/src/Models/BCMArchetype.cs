using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMArchetype : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Name = "name";
      public const string IsMale = "ismale";
      public const string HairColor = "hair";
      public const string EyeColor = "eye";
      public const string SkinColor = "skin";
      public const string Type = "type";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1,  StrFilters.IsMale },
      { 2,  StrFilters.HairColor },
      { 3,  StrFilters.EyeColor },
      { 4,  StrFilters.SkinColor },
      { 5,  StrFilters.Type }
    };
    public static Dictionary<int, string> FilterMap
    {
      get => _filterMap;
      set => _filterMap = value;
    }
    #endregion

    #region Properties
    public string Name;
    public bool? IsMale;
    public string HairColor;
    public string EyeColor;
    public string SkinColor;
    public string Type;
    #endregion;

    public BCMArchetype(object obj, string typeStr, Dictionary<string, string> options, List<string> filter) : base(obj, typeStr, options, filter)
    {
    }

    public override void GetData(object obj)
    {
      var archetype = obj as Archetype;
      if (archetype == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              ArchetypeName(archetype);
              break;
            case StrFilters.Type:
              ArchetypeType(archetype);
              break;
            case StrFilters.IsMale:
              ArchetypeGender(archetype);
              break;
            case StrFilters.EyeColor:
              ArchetypeEyes(archetype);
              break;
            case StrFilters.HairColor:
              ArchetypeHair(archetype);
              break;
            case StrFilters.SkinColor:
              ArchetypeSkin(archetype);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        ArchetypeName(archetype);
        ArchetypeType(archetype);
        ArchetypeGender(archetype);
        ArchetypeEyes(archetype);
        ArchetypeHair(archetype);
        ArchetypeSkin(archetype);
      }

      //todo:
      //a.BaseSlots;
      //a.PreviewSlots;
      //a.Dna;
      //a.ExpressionData;
      //a.VoiceSet;
    }

    private void ArchetypeSkin(Archetype archetype) => Bin.Add("SkinColor", SkinColor = archetype.SkinColor.ToStringRgbHex());

    private void ArchetypeHair(Archetype archetype) => Bin.Add("HairColor", HairColor = archetype.HairColor.ToStringRgbHex());

    private void ArchetypeEyes(Archetype archetype) => Bin.Add("EyeColor", EyeColor = archetype.EyeColor.ToStringRgbHex());

    private void ArchetypeGender(Archetype archetype) => Bin.Add("IsMale", IsMale = archetype.IsMale);

    private void ArchetypeType(Archetype archetype) => Bin.Add("Type", Type = archetype.Type.ToString());

    private void ArchetypeName(Archetype archetype) => Bin.Add("Name", Name = archetype.Name);
  }
}
