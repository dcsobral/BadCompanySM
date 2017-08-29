using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  [Serializable]
  public class BCMArchetype : BCMAbstract
  {
    #region Filters
    public static class StrFilters
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
    public string Name;
    public bool? IsMale;
    public string HairColor;
    public string EyeColor;
    public string SkinColor;
    public string Type;
    public class BCMSlot
    {
      public string Name;
      //public string UISlot;
      public string EqSlot;
      public string EqLayer;
      public bool AltHair;
      public List<string> Textures;
      public List<string> Colors;
      public Dictionary<string, string> Masks;

      public BCMSlot(UMASlot slot)
      {
        Name = slot.Name;
        //UISlot = slot.UISlot.ToString();
        EqSlot = slot.EquipmentSlot.ToString();
        EqLayer = slot.EquipmentLayer.ToString();
        AltHair = slot.ShowAltHair;
        Textures = slot.Textures;
        Colors = slot.Colors;
        Masks = slot.Masks.ToDictionary(s => s.EquipmentSlot.ToString(), s => s.EquipmentLayer.ToString());
      }
    }
    private List<BCMSlot> _baseSlots;
    public List<BCMSlot> BaseSlots => _baseSlots ?? (_baseSlots = new List<BCMSlot>());
    private List<BCMSlot> _previewSlots;
    public List<BCMSlot> PreviewSlots => _previewSlots ?? (_previewSlots = new List<BCMSlot>());

    private Dictionary<string, double> _dna;
    public Dictionary<string, double> Dna => _dna ?? (_dna = new Dictionary<string, double>());

    public class BCMExpressionData
    {
      public bool Blink;
      public bool Saccades;
      public double BlinkDur;
      public int BlinkMin;
      public int BlinkMax;
      public Dictionary<string, double> Values;
      public BCMExpressionData(UMAExpressionData expression)
      {
        Blink = expression.BlinkingEnabled;
        Saccades = expression.SaccadesEnabled;
        BlinkDur = Math.Round(expression.BlinkDuration, 3);
        BlinkMin = expression.BlinkMinDelay;
        BlinkMax = expression.BlinkMaxDelay;
        Values = expression.ExpressionValues.ToDictionary(v => v.Key, v => Math.Round(v.Value, 3));
      }
    }
    public BCMExpressionData Expressions;
    public string VoiceSet;
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
