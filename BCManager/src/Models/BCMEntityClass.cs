using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntityClass : BCMAbstract
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
    //todo: dynamic properties
    public int Id;
    public string Name;

    public string Class;
    public string Skin;
    public string Parent;
    public bool UserInst;
    public bool IsEnemy;
    public bool IsAnimal;
    public BCMExplosionData Explosion;
    public string Model;
    public string ModelP1;
    public string ModelP2;
    public double RagdollChance;
    public bool HasRagdoll;
    public string Colliders;
    public double CrouchY;
    public string CorpseId;
    public double CorpseChance;
    public int CorpseDen;
    public double MaxTurn;
    public bool RootMotion;
    public bool HasDeathAnim;
    public bool IsMale;
    public bool IsObserver;
    public int Experience;
    public string Physics;
    public int MaxHealth;
    public int DeadHP;
    public int LootListAlive;
    public int LootListOnDeath;
    public double SwimOffset;
    public double SightRange;
    public double SearchArea;
    public BCMVector2 GroanMin;
    public BCMVector2 GroanMax;
    public BCMVector2 WakeMin;
    public BCMVector2 WakeMax;
    public BCMVector2 SightScale;
    public BCMVector2 SmellAlert;
    public BCMVector2 NoiseAlert;
    public BCMVector2 SmellWake;
    public BCMVector2 SmellGroan;
    public BCMVector2 NoiseWake;
    public BCMVector2 NoiseGroan;
    public double GroanChance;
    public string UMARace;
    public string UMAGenModel;
    public double LegCrippleTh;
    public double LegCrawlerTh;
    public double LowerLegDisTh;
    public double LowerLegDisBonus;
    public double LowerLegDisBase;
    public double UpperLegDisTh;
    public double UpperLegDisBonus;
    public double UpperLegDisBase;
    public double LowerArmDisTh;
    public double LowerArmDisBonus;
    public double LowerArmDisBase;
    public double UpperArmDisTh;
    public double UpperArmDisBonus;
    public double UpperArmDisBase;
    public double HeadDisTh;
    public double HeadDisBase;
    public double HeadDisBonus;
    public double KDKneelDamTh;
    public BCMVector2 KDKneelStunDur;
    public BCMVector2 KDKneelRefillRate;
    public double KDProneDamTh;
    public BCMVector2 KDProneStunDur;
    public BCMVector2 KDProneRefillRate;
    public double LegsExpMult;
    public double ArmsExpMult;
    public double ChestExpMult;
    public double HeadExpMult;

    public List<string> MatSwap = new List<string>();
    public List<BCMVector3> Tints = new List<BCMVector3>();
    public EntityClass.ParticleData SpawnParticle;
    public BCMVector3 ModelTransformAdjust;
    public string Archetype;
    public bool UseAIPackages;
    public List<string> AIPackages = new List<string>();
    public List<string> PainStates = new List<string>();
    public List<string> PainTriggers = new List<string>();
    public List<string> Attacks = new List<string>();
    public List<string> AttackTriggers = new List<string>();
    public List<string> SpecialAttacks = new List<string>();
    public List<string> SpecialAttackTriggers = new List<string>();
    public List<string> DeathStates = new List<string>();
    public List<string> DeathStateTriggers = new List<string>();
    public List<string> Buffs = new List<string>();

    public class BCMExplosionData
    {
      public int Particle;
      public int BlockR;
      public int EntityR;
      public int BuffsR;
      public int BlastPower;
      public double EntityDam;
      public double BlockDam;
      //public Dictionary<string, double> damageMultiplier = new Dictionary<string, double>();
      public List<string> Buffs = new List<string>();

      public BCMExplosionData(ExplosionData expl)
      {
        Particle = expl.ParticleIndex;
        BlockR = expl.BlockRadius;
        EntityR = expl.EntityRadius;
        BuffsR = expl.BuffsRadius;
        BlastPower = expl.BlastPower;
        EntityDam = expl.EntityDamage;
        BlockDam = expl.BlockDamage;
        //todo:
        //damageMultiplier = expl.damageMultiplier;
        if (expl.BuffActions != null)
        {
          foreach (var buff in expl.BuffActions)
          {
            Buffs.Add(buff.Class?.Name);
          }
        }
      }
    }

    public class BCMVector2
    {
      public int x;
      public int y;
      public BCMVector2()
      {
        x = 0;
        y = 0;
      }
      public BCMVector2(int x, int y)
      {
        this.x = x;
        this.y = y;
      }
      public BCMVector2(Vector2 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
      }
      public BCMVector2(Vector2i v)
      {
        x = v.x;
        y = v.y;
      }
    }

    public class BCMVector3
    {
      public int x;
      public int y;
      public int z;
      public BCMVector3()
      {
        x = 0;
        y = 0;
        z = 0;
      }
      public BCMVector3(Vector3 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
        z = Mathf.RoundToInt(v.z);
      }
      public BCMVector3(Vector3i v)
      {
        x = v.x;
        y = v.y;
        z = v.z;
      }
    }
    #endregion;

    public BCMEntityClass(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (!(obj is EntityClass ec)) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(ec);
              break;
            case StrFilters.Name:
              GetName(ec);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(ec);
        GetName(ec);

        Bin.Add("Class", Class = ec.classname?.ToString());
        Bin.Add("Skin", Skin = ec.skinTexture);
        Bin.Add("Parent", Parent = ec.parentGameObjectName);
        Bin.Add("UserInst", UserInst = ec.bAllowUserInstantiate);
        Bin.Add("IsEnemy", IsEnemy = ec.bIsEnemyEntity);
        Bin.Add("IsAnimal", IsAnimal = ec.bIsAnimalEntity);
        Bin.Add("Explosion", Explosion = new BCMExplosionData(ec.explosionData));
        Bin.Add("Model", Model = ec.modelType);
        Bin.Add("ModelP1", ModelP1 = ec.modelTypeParams1);
        Bin.Add("ModelP2", ModelP2 = ec.modelTypeParams2);
        Bin.Add("RagdollChance", RagdollChance = Math.Round(ec.RagdollOnDeathChance, 6));
        Bin.Add("HasRagdoll", HasRagdoll = ec.HasRagdoll);
        Bin.Add("Colliders", Colliders = ec.CollidersRagdollAsset);
        Bin.Add("CrouchY", CrouchY = Math.Round(ec.crouchYOffsetFP, 6));
        Bin.Add("CorpseId", CorpseId = ec.CorpseBlockId);
        Bin.Add("CorpseChance", CorpseChance = Math.Round(ec.CorpseBlockChance, 6));
        Bin.Add("CorpseDen", CorpseDen = ec.CorpseBlockDensity);
        Bin.Add("MaxTurn", MaxTurn = Math.Round(ec.MaxTurnSpeed, 6));
        Bin.Add("RootMotion", RootMotion = ec.RootMotion);
        Bin.Add("HasDeathAnim", HasDeathAnim = ec.HasDeathAnim);
        Bin.Add("IsMale", IsMale = ec.bIsMale);
        Bin.Add("IsObserver", IsObserver = ec.bIsChunkObserver);
        Bin.Add("Experience", Experience = ec.ExperienceValue);
        Bin.Add("Physics", Physics = ec.PhysicsBody?.Name);
        Bin.Add("MaxHealth", MaxHealth = ec.Properties.Values.ContainsKey(EntityClass.PropMaxHealth) ? int.Parse(ec.Properties.Values[EntityClass.PropMaxHealth]) : 100);
        Bin.Add("LootListOnDeath", LootListOnDeath = ec.Properties.Values.ContainsKey(EntityClass.PropLootListOnDeath) ? int.Parse(ec.Properties.Values[EntityClass.PropLootListOnDeath]) : 0);
        Bin.Add("LootListAlive", LootListAlive = ec.Properties.Values.ContainsKey(EntityClass.PropLootListAlive) ? int.Parse(ec.Properties.Values[EntityClass.PropLootListAlive]) : 0);
        Bin.Add("DeadHP", DeadHP = ec.DeadBodyHitPoints);
        Bin.Add("LegCrippleTh", LegCrippleTh = Math.Round(ec.LegCrippleThreshold, 6));
        Bin.Add("LegCrawlerTh", LegCrawlerTh = Math.Round(ec.LegCrawlerThreshold, 6));
        Bin.Add("LowerLegDisTh", LowerLegDisTh = Math.Round(ec.LowerLegDismemberThreshold, 6));
        Bin.Add("LowerLegDisBonus", LowerLegDisBonus = Math.Round(ec.LowerLegDismemberBonusChance, 6));
        Bin.Add("LowerLegDisBase", LowerLegDisBase = Math.Round(ec.LowerLegDismemberBaseChance, 6));
        Bin.Add("UpperLegDisTh", UpperLegDisTh = Math.Round(ec.UpperLegDismemberThreshold, 6));
        Bin.Add("UpperLegDisBonus", UpperLegDisBonus = Math.Round(ec.UpperLegDismemberBonusChance, 6));
        Bin.Add("UpperLegDisBase", UpperLegDisBase = Math.Round(ec.UpperLegDismemberBaseChance, 6));
        Bin.Add("LowerArmDisTh", LowerArmDisTh = Math.Round(ec.LowerArmDismemberThreshold, 6));
        Bin.Add("LowerArmDisBonus", LowerArmDisBonus = Math.Round(ec.LowerArmDismemberBonusChance, 6));
        Bin.Add("LowerArmDisBase", LowerArmDisBase = Math.Round(ec.LowerArmDismemberBaseChance, 6));
        Bin.Add("UpperArmDisTh", UpperArmDisTh = Math.Round(ec.UpperArmDismemberThreshold, 6));
        Bin.Add("UpperArmDisBonus", UpperArmDisBonus = Math.Round(ec.UpperArmDismemberBonusChance, 6));
        Bin.Add("UpperArmDisBase", UpperArmDisBase = Math.Round(ec.UpperArmDismemberBaseChance, 6));
        Bin.Add("HeadDisTh", HeadDisTh = Math.Round(ec.HeadDismemberThreshold, 6));
        Bin.Add("HeadDisBase", HeadDisBase = Math.Round(ec.HeadDismemberBaseChance, 6));
        Bin.Add("HeadDisBonus", HeadDisBonus = Math.Round(ec.HeadDismemberBonusChance, 6));
        Bin.Add("KDKneelDamTh", KDKneelDamTh = Math.Round(ec.KnockdownKneelDamageThreshold, 6));
        Bin.Add("LegsExpMult", LegsExpMult = Math.Round(ec.LegsExplosionDamageMultiplier, 6));
        Bin.Add("ArmsExpMult", ArmsExpMult = Math.Round(ec.ArmsExplosionDamageMultiplier, 6));
        Bin.Add("ChestExpMult", ChestExpMult = Math.Round(ec.ChestExplosionDamageMultiplier, 6));
        Bin.Add("HeadExpMult", HeadExpMult = Math.Round(ec.HeadExplosionDamageMultiplier, 6));
        Bin.Add("SearchArea", SearchArea = Math.Round(ec.SearchArea, 6));
        Bin.Add("SwimOffset", SwimOffset = Math.Round(ec.SwimOffset, 6));
        Bin.Add("SightRange", SightRange = Math.Round(ec.SightRange, 6));
        Bin.Add("GroanMin", GroanMin = new BCMVector2(ec.GroanMin));
        Bin.Add("GroanMax", GroanMax = new BCMVector2(ec.GroanMax));
        Bin.Add("WakeMin", WakeMin = new BCMVector2(ec.WakeMin));
        Bin.Add("WakeMax", WakeMax = new BCMVector2(ec.WakeMax));
        Bin.Add("SightScale", SightScale = new BCMVector2(ec.SightScale));
        Bin.Add("SmellAlert", SmellAlert = new BCMVector2(ec.SmellAlert));
        Bin.Add("NoiseAlert", NoiseAlert = new BCMVector2(ec.NoiseAlert));
        Bin.Add("SmellWake", SmellWake = new BCMVector2(ec.SmellWake));
        Bin.Add("SmellGroan", SmellGroan = new BCMVector2(ec.SmellGroan));
        Bin.Add("NoiseWake", NoiseWake = new BCMVector2(ec.NoiseWake));
        Bin.Add("NoiseGroan", NoiseGroan = new BCMVector2(ec.NoiseGroan));
        Bin.Add("GroanChance", GroanChance = Math.Round(ec.GroanChance, 6));
        Bin.Add("UMARace", UMARace = ec.UMARace);
        Bin.Add("UMAGenModel", UMAGenModel = ec.UMAGeneratedModelName);
        Bin.Add("MatSwap", MatSwap = ec.MaterialSwap?.ToList());
        if (ec.tintColors != null)
          foreach (var tint in ec.tintColors.Values)
          {
            Tints.Add(new BCMVector3(tint));
          }
        Bin.Add("Tints", Tints);
        Bin.Add("SpawnParticle", SpawnParticle = ec.particleOnSpawn);
        Bin.Add("KDKneelStunDur", KDKneelStunDur = new BCMVector2(ec.KnockdownProneStunDuration));
        Bin.Add("KDProneDamTh", KDProneDamTh = Math.Round(ec.KnockdownProneDamageThreshold, 6));
        Bin.Add("KDProneStunDur", KDProneStunDur = new BCMVector2(ec.KnockdownProneStunDuration));
        Bin.Add("KDProneRefillRate", KDProneRefillRate = new BCMVector2(ec.KnockdownProneRefillRate));
        Bin.Add("KDKneelRefillRate", KDKneelRefillRate = new BCMVector2(ec.KnockdownKneelRefillRate));
        Bin.Add("ModelTransformAdjust", ModelTransformAdjust = new BCMVector3(ec.ModelTransformAdjust));
        Bin.Add("Archetype", Archetype = ec.ArchetypeName);
        Bin.Add("AIPackages", AIPackages = ec.AIPackages?.ToList());
        Bin.Add("UseAIPackages", UseAIPackages = ec.UseAIPackages);
        Bin.Add("PainStates", PainStates = ec.MechanimPainStates);
        Bin.Add("PainTriggers", PainTriggers = ec.MechanimPainTriggers);
        Bin.Add("Attacks", Attacks = ec.MechanimAttacks);
        Bin.Add("AttackTriggers", AttackTriggers = ec.MechanimAttackTriggers);
        Bin.Add("SpecialAttacks", SpecialAttacks = ec.MechanimSpecialAttacks);
        Bin.Add("SpecialAttackTriggers", SpecialAttackTriggers = ec.MechanimSpecialAttackTriggers);
        Bin.Add("DeathStates", DeathStates = ec.MechanimDeathStates);
        Bin.Add("DeathStateTriggers", DeathStateTriggers = ec.MechanimDeathStateTriggers);
        Bin.Add("Buffs", Buffs = ec.Buffs);
      }
    }

    private void GetName(EntityClass entityClass) => Bin.Add("Name", Name = entityClass.entityClassName);

    private void GetId(EntityClass entityClass) => Bin.Add("Id", Id = entityClass.GetHashCode());
  }
}
