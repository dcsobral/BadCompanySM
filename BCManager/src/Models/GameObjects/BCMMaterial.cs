using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  [Serializable]
  public class BCMMaterial : BCMAbstract
  {
    #region Filters
    private static class StrFilters
    {
      public const string Id = "id";
      public const string Experience = "exp";
      public const string MaxDamage = "maxdam";
      public const string MovementFactor = "movement";
      public const string FertileLevel = "fert";
      public const string Hardness = "hardness";
      public const string LightOpacity = "light";
      public const string Mass = "mass";
      public const string StabilityGlue = "stabglue";
      public const string StabilitySupport = "stabsupport";
      public const string DamageCategory = "damcat";
      public const string ForgeCategory = "forgecat";
      public const string ParticleCategory = "particle";
      public const string DestroyCategory = "destroycat";
      public const string SurfaceCategory = "surfacecat";
      public const string StepSound = "sound";
      public const string Resistance = "resistance";
      public const string IsCollidable = "collide";
      public const string IsGroundCover = "cover";
      public const string IsLiquid = "liquid";
      public const string IsPlant = "plant";
      //public const string Friction = "friction";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0, StrFilters.Id },
      { 1, StrFilters.Experience },
      { 2, StrFilters.MaxDamage },
      { 3, StrFilters.MovementFactor },
      { 4, StrFilters.FertileLevel },
      { 5, StrFilters.Hardness },
      { 6, StrFilters.LightOpacity },
      { 7, StrFilters.Mass },
      { 8, StrFilters.StabilityGlue },
      { 9, StrFilters.StabilitySupport },
      { 10, StrFilters.DamageCategory },
      { 11, StrFilters.ForgeCategory },
      { 12, StrFilters.ParticleCategory },
      { 13, StrFilters.DestroyCategory },
      { 14, StrFilters.SurfaceCategory },
      { 15, StrFilters.StepSound },
      { 16, StrFilters.Resistance },
      { 17, StrFilters.IsCollidable },
      { 18, StrFilters.IsGroundCover },
      { 19, StrFilters.IsLiquid },
      { 20, StrFilters.IsPlant }
      //{ 21, StrFilters.Friction }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public double Experience;
    [UsedImplicitly] public int MaxDamage;
    [UsedImplicitly] public double MovementFactor;
    [UsedImplicitly] public int FertileLevel;
    [UsedImplicitly] public double Hardness;
    [UsedImplicitly] public int LightOpacity;
    [UsedImplicitly] public int Mass;
    [UsedImplicitly] public int StabilityGlue;
    [UsedImplicitly] public bool StabilitySupport;
    [UsedImplicitly] public string DamageCategory;
    [UsedImplicitly] public string ForgeCategory;
    [UsedImplicitly] public string ParticleCategory;
    [UsedImplicitly] public string DestroyCategory;
    [UsedImplicitly] public string SurfaceCategory;
    [UsedImplicitly] public string StepSound;
    [UsedImplicitly] public double Resistance;
    [UsedImplicitly] public bool IsCollidable;
    [UsedImplicitly] public bool IsGroundCover;
    [UsedImplicitly] public bool IsLiquid;
    [UsedImplicitly] public bool IsPlant;
    #endregion;

    public BCMMaterial(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is MaterialBlock material)) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(material);
              break;

            case StrFilters.Experience:
              GetExperience(material);
              break;

            case StrFilters.MaxDamage:
              GetMaxDamage(material);
              break;

            case StrFilters.MovementFactor:
              GetMovementFactor(material);
              break;

            case StrFilters.FertileLevel:
              GetFertileLevel(material);
              break;

            case StrFilters.Hardness:
              GetHardness(material);
              break;

            case StrFilters.LightOpacity:
              GetLightOpacity(material);
              break;

            case StrFilters.Mass:
              GetMass(material);
              break;

            case StrFilters.StabilityGlue:
              GetStabilityGlue(material);
              break;

            case StrFilters.StabilitySupport:
              GetStabilitySupport(material);
              break;

            case StrFilters.DamageCategory:
              GetDamageCatagory(material);
              break;

            case StrFilters.ForgeCategory:
              GetForgeCategory(material);
              break;

            case StrFilters.ParticleCategory:
              GetParticleCategory(material);
              break;

            case StrFilters.DestroyCategory:
              GetDestroyCategory(material);
              break;

            case StrFilters.SurfaceCategory:
              GetSurfaceCategory(material);
              break;

            case StrFilters.StepSound:
              GetStepSound(material);
              break;

            case StrFilters.Resistance:
              GetResistance(material);
              break;

            case StrFilters.IsCollidable:
              GetIsCollidable(material);
              break;

            case StrFilters.IsGroundCover:
              GetIsGroundCover(material);
              break;

            case StrFilters.IsLiquid:
              GetIsLiquid(material);
              break;

            case StrFilters.IsPlant:
              GetIsPlant(material);
              break;

            //case StrFilters.Friction:
            //  GetFriction(material);
            //  break;

            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(material);
        GetExperience(material);
        GetMaxDamage(material);
        GetMovementFactor(material);
        GetFertileLevel(material);
        GetHardness(material);
        GetLightOpacity(material);
        GetMass(material);
        GetStabilityGlue(material);
        GetStabilitySupport(material);
        GetDamageCatagory(material);
        GetForgeCategory(material);
        GetParticleCategory(material);
        GetDestroyCategory(material);
        GetSurfaceCategory(material);
        GetStepSound(material);
        GetResistance(material);
        GetIsCollidable(material);
        GetIsGroundCover(material);
        GetIsLiquid(material);
        GetIsPlant(material);
        //GetFriction(material);
      }
    }

    //private void GetFriction(MaterialBlock material) => Bin.Add("Friction", Friction = Math.Round(material.Friction, 2));

    private void GetIsPlant(MaterialBlock material) => Bin.Add("IsPlant", IsPlant = material.IsPlant);

    private void GetIsLiquid(MaterialBlock material) => Bin.Add("IsLiquid", IsLiquid = material.IsLiquid);

    private void GetIsGroundCover(MaterialBlock material) => Bin.Add("IsGroundCover", IsGroundCover = material.IsGroundCover);

    private void GetIsCollidable(MaterialBlock material) => Bin.Add("IsCollidable", IsCollidable = material.IsCollidable);

    private void GetResistance(MaterialBlock material) => Bin.Add("Resistance", Resistance = Math.Round(material.Resistance, 3));

    private void GetStepSound(MaterialBlock material) => Bin.Add("StepSound", StepSound = material.stepSound?.name);

    private void GetSurfaceCategory(MaterialBlock material) => Bin.Add("SurfaceCategory", SurfaceCategory = material.SurfaceCategory);

    private void GetDestroyCategory(MaterialBlock material) => Bin.Add("DestroyCategory", DestroyCategory = material.ParticleDestroyCategory);

    private void GetParticleCategory(MaterialBlock material) => Bin.Add("ParticleCategory", ParticleCategory = material.ParticleCategory);

    private void GetForgeCategory(MaterialBlock material) => Bin.Add("ForgeCategory", ForgeCategory = material.ForgeCategory);

    private void GetDamageCatagory(MaterialBlock material) => Bin.Add("DamageCategory", DamageCategory = material.DamageCategory);

    private void GetStabilitySupport(MaterialBlock material) => Bin.Add("StabilitySupport", StabilitySupport = material.StabilitySupport);

    private void GetStabilityGlue(MaterialBlock material) => Bin.Add("StabilityGlue", StabilityGlue = material.StabilityGlue);

    private void GetMass(MaterialBlock material) => Bin.Add("Mass", Mass = material.Mass.Value);

    private void GetLightOpacity(MaterialBlock material) => Bin.Add("LightOpacity", LightOpacity = material.LightOpacity);

    private void GetHardness(MaterialBlock material) => Bin.Add("Hardness", Hardness = material.Hardness.Value);

    private void GetFertileLevel(MaterialBlock material) => Bin.Add("FertileLevel", FertileLevel = material.FertileLevel);

    private void GetMovementFactor(MaterialBlock material) => Bin.Add("MovementFactor", MovementFactor = Math.Round(material.MovementFactor, 2));

    private void GetMaxDamage(MaterialBlock material) => Bin.Add("MaxDamage", MaxDamage = material.MaxDamage);

    private void GetExperience(MaterialBlock material) => Bin.Add("Experience", Experience = material.Experience);

    private void GetId(MaterialBlock material) => Bin.Add("Id", Id = material.id);
  }
}
