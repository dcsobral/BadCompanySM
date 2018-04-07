using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  [Serializable]
  public class BCMArchetype : BCMAbstract
  {
    #region Filters
    private static class StrFilters
    {
      public const string Name = "name";
      public const string Type = "type";
      public const string IsMale = "ismale";
      public const string HairColor = "hair";
      public const string EyeColor = "eye";
      public const string SkinColor = "skin";
      public const string VoiceSets = "voice";
      public const string BaseSlots = "base";
      public const string PreviewSlots = "preview";
      public const string Dna = "dna";
      public const string Expressions = "expression";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1,  StrFilters.Type },
      { 2,  StrFilters.IsMale },
      { 3,  StrFilters.HairColor },
      { 4,  StrFilters.EyeColor },
      { 5,  StrFilters.SkinColor },
      { 6,  StrFilters.VoiceSets },
      { 7,  StrFilters.BaseSlots },
      { 8,  StrFilters.PreviewSlots },
      { 9,  StrFilters.Dna },
      { 10,  StrFilters.Expressions }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public bool? IsMale;
    [UsedImplicitly] public string HairColor;
    [UsedImplicitly] public string EyeColor;
    [UsedImplicitly] public string SkinColor;
    [UsedImplicitly] public string Type;
    [NotNull] [UsedImplicitly] public List<BCMSlot> BaseSlots = new List<BCMSlot>();
    [NotNull] [UsedImplicitly] public List<BCMSlot> PreviewSlots = new List<BCMSlot>();
    [NotNull] [UsedImplicitly] public Dictionary<string, double> Dna = new Dictionary<string, double>();
    [UsedImplicitly] public BCMExpressionData Expressions;
    [UsedImplicitly] public string VoiceSet;
    #endregion;

    public BCMArchetype(object obj, string typeStr, Dictionary<string, string> options, List<string> filter) : base(obj, typeStr, options, filter)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is Archetype archetype)) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              GetName(archetype);
              break;
            case StrFilters.Type:
              GetType(archetype);
              break;
            case StrFilters.IsMale:
              GetGender(archetype);
              break;
            case StrFilters.EyeColor:
              GetEyes(archetype);
              break;
            case StrFilters.HairColor:
              GetHair(archetype);
              break;
            case StrFilters.SkinColor:
              GetSkin(archetype);
              break;
            case StrFilters.VoiceSets:
              GetVoiceSet(archetype);
              break;
            case StrFilters.BaseSlots:
              GetBaseSlots(archetype);
              break;
            case StrFilters.PreviewSlots:
              GetPreviewSlots(archetype);
              break;
            case StrFilters.Dna:
              GetDna(archetype);
              break;
            case StrFilters.Expressions:
              GetExpressions(archetype);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetName(archetype);
        GetType(archetype);
        GetGender(archetype);

        if (!IsOption("full")) return;

        GetEyes(archetype);
        GetHair(archetype);
        GetSkin(archetype);
        GetVoiceSet(archetype);
        GetBaseSlots(archetype);
        GetPreviewSlots(archetype);
        GetDna(archetype);
        GetExpressions(archetype);
      }
    }

    private void GetExpressions(Archetype archetype) => Bin.Add("Expressions", Expressions = new BCMExpressionData(archetype.ExpressionData));

    private void GetDna(Archetype archetype)
    {
      foreach (var dna in archetype.Dna.Names)
      {
        Dna.Add(dna, Math.Round(archetype.Dna.GetValue(dna), 3));
      }
      Bin.Add("Dna", Dna);
    }

    private void GetPreviewSlots(Archetype archetype)
    {
      foreach (var slot in archetype.PreviewSlots)
      {
        PreviewSlots.Add(new BCMSlot(slot));
      }
      Bin.Add("PreviewSlots", PreviewSlots);
    }

    private void GetBaseSlots(Archetype archetype)
    {
      foreach (var slot in archetype.BaseSlots)
      {
        BaseSlots.Add(new BCMSlot(slot));
      }
      Bin.Add("BaseSlots", BaseSlots);
    }

    private void GetVoiceSet(Archetype archetype) => Bin.Add("VoiceSets", VoiceSet = archetype.VoiceSet);

    private void GetSkin(Archetype archetype) => Bin.Add("SkinColor", SkinColor = archetype.SkinColor.ToStringRgbHex());

    private void GetHair(Archetype archetype) => Bin.Add("HairColor", HairColor = archetype.HairColor.ToStringRgbHex());

    private void GetEyes(Archetype archetype) => Bin.Add("EyeColor", EyeColor = archetype.EyeColor.ToStringRgbHex());

    private void GetGender(Archetype archetype) => Bin.Add("IsMale", IsMale = archetype.IsMale);

    private void GetType(Archetype archetype) => Bin.Add("Type", Type = archetype.Type.ToString());

    private void GetName(Archetype archetype) => Bin.Add("Name", Name = archetype.Name);
  }
}
