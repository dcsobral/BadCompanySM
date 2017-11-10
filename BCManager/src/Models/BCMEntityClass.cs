using System;
using System.Collections.Generic;

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
      public const string CanSpawn = "canspawn";
      public const string Class = "class";
      public const string Model = "model";
      public const string Archetype = "archetype";
      public const string Experience = "experience";
      public const string MaxHealth = "maxhp";
      public const string DeadHP = "deadhp";
      public const string HandItem = "handitem";
      public const string LootListOnDeath = "lootdead";
      public const string LootListAlive = "lootalive";
      public const string IsEnemy = "isenemy";
      public const string WalkType = "walktype";
      public const string RotateToGround = "toground";
      public const string SpeedWander = "wander";
      public const string SpeedApproach = "approach";
      public const string SpeedWanderNight = "wandernight";
      public const string SpeedApproachNight = "approachnight";
      public const string SpeedPanic = "panic";
      public const string CanClimbVertical = "climbwall";
      public const string CanClimbLadders = "climbladder";
      public const string AttackTimeoutDay = "attackday";
      public const string AttackTimeoutNight = "attacknight";
      public const string AITasks = "tasks";
      public const string AITargetTasks = "targets";
      public const string ExplosionData = "explosion";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name },
      { 2,  StrFilters.CanSpawn },
      { 3,  StrFilters.Class },
      { 4,  StrFilters.Model },
      { 5,  StrFilters.Archetype },
      { 6,  StrFilters.Experience },
      { 7,  StrFilters.MaxHealth },
      { 8,  StrFilters.DeadHP },
      { 9,  StrFilters.HandItem },
      { 10,  StrFilters.LootListOnDeath },
      { 11,  StrFilters.LootListAlive },
      { 12,  StrFilters.IsEnemy },
      { 13,  StrFilters.WalkType },
      { 14,  StrFilters.RotateToGround },
      { 15,  StrFilters.SpeedWander },
      { 16,  StrFilters.SpeedApproach },
      { 17,  StrFilters.SpeedWanderNight },
      { 18,  StrFilters.SpeedApproachNight },
      { 19,  StrFilters.SpeedPanic },
      { 20,  StrFilters.CanClimbVertical },
      { 21,  StrFilters.CanClimbLadders },
      { 22,  StrFilters.AttackTimeoutDay },
      { 23,  StrFilters.AttackTimeoutNight },
      { 24,  StrFilters.AITasks },
      { 25,  StrFilters.AITargetTasks },
      { 26,  StrFilters.ExplosionData }

    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public int Id;
    public string Name;
    public bool CanSpawn;
    public string Class;
    public string Model;
    public string Archetype;
    public int Experience;
    public int MaxHealth;
    public int DeadHP;
    public string HandItem;
    public int LootListOnDeath;
    public int LootListAlive;
    public bool IsEnemy;
    public int WalkType;
    public bool RotateToGround;
    public double SpeedWander;
    public double SpeedApproach;
    public double SpeedWanderNight;
    public double SpeedApproachNight;
    public double SpeedPanic;
    public bool CanClimbVertical;
    public bool CanClimbLadders;
    public double AttackTimeoutDay;
    public double AttackTimeoutNight;

    public List<BCMDynamicProp> AITasks = new List<BCMDynamicProp>();
    public List<BCMDynamicProp> AITargetTasks = new List<BCMDynamicProp>();
    public BCMExplosionData Explosion;
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
            case StrFilters.CanSpawn:
              GetUserCanSpawn(ec);
              break;
            case StrFilters.Class:
              GetClass(ec);
              break;
            case StrFilters.Model:
              GetModel(ec);
              break;
            case StrFilters.Archetype:
              GetArchetype(ec);
              break;
            case StrFilters.Experience:
              GetExperience(ec);
              break;
            case StrFilters.MaxHealth:
              GetMaxHealth(ec);
              break;
            case StrFilters.DeadHP:
              GetDeadHP(ec);
              break;
            case StrFilters.HandItem:
              GetHandItem(ec);
              break;
            case StrFilters.LootListOnDeath:
              GetLootListOnDeath(ec);
              break;
            case StrFilters.LootListAlive:
              GetLootListAlive(ec);
              break;
            case StrFilters.IsEnemy:
              GetIsEnemy(ec);
              break;
            case StrFilters.WalkType:
              GetWalkType(ec);
              break;
            case StrFilters.RotateToGround:
              GetRotateToGround(ec);
              break;
            case StrFilters.SpeedWander:
              GetSpeedWander(ec);
              break;
            case StrFilters.SpeedApproach:
              GetSpeedApproach(ec);
              break;
            case StrFilters.SpeedWanderNight:
              GetSpeedWanderNight(ec);
              break;
            case StrFilters.SpeedApproachNight:
              GetSpeedApproachNight(ec);
              break;
            case StrFilters.SpeedPanic:
              GetSpeedPanic(ec);
              break;
            case StrFilters.CanClimbVertical:
              GetCanClimbVertical(ec);
              break;
            case StrFilters.CanClimbLadders:
              GetCanClimbLadders(ec);
              break;
            case StrFilters.AttackTimeoutDay:
              GetAttackTimeoutDay(ec);
              break;
            case StrFilters.AttackTimeoutNight:
              GetAttackTimeoutNight(ec);
              break;
            case StrFilters.AITasks:
              GetAITasks(ec);
              break;
            case StrFilters.AITargetTasks:
              GetAITargetTasks(ec);
              break;
            case StrFilters.ExplosionData:
              GetExplosionData(ec);
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
        GetUserCanSpawn(ec);
        GetClass(ec);
        GetModel(ec);
        GetArchetype(ec);
        GetExperience(ec);
        GetMaxHealth(ec);
        GetDeadHP(ec);
        GetHandItem(ec);
        GetLootListOnDeath(ec);
        GetLootListAlive(ec);
        GetIsEnemy(ec);
        GetWalkType(ec);
        GetRotateToGround(ec);
        GetSpeedWander(ec);
        GetSpeedApproach(ec);
        GetSpeedWanderNight(ec);
        GetSpeedApproachNight(ec);
        GetSpeedPanic(ec);
        GetCanClimbVertical(ec);
        GetCanClimbLadders(ec);
        GetAttackTimeoutDay(ec);
        GetAttackTimeoutNight(ec);
        GetAITasks(ec);
        GetAITargetTasks(ec);
        GetExplosionData(ec);

        if (!Options.ContainsKey("full") && !Options.ContainsKey("extra")) return;

        //IMPORTANT PROPS
        GetProperty(ec, EntityClass.PropEntityType);
        GetProperty(ec, EntityClass.PropMesh);
        GetProperty(ec, EntityClass.PropAvatarController);
        GetProperty(ec, EntityClass.PropMaterialSwap0);
        GetProperty(ec, EntityClass.PropMaterialSwap1);
        GetProperty(ec, EntityClass.PropMaterialSwap2);
        GetProperty(ec, EntityClass.PropMaterialSwap3);
        GetProperty(ec, EntityClass.PropMaterialSwap4);
        //GetProperty(ec, EntityClass.PropTintMaterial);
        GetProperty(ec, EntityClass.PropRightHandJointName);
        GetProperty(ec, EntityClass.PropMaxViewAngle);
        GetProperty(ec, EntityClass.PropTimeStayAfterDeath);
        GetProperty(ec, EntityClass.PropImmunity);
        GetProperty(ec, EntityClass.PropPhysicsBody);
        GetProperty(ec, EntityClass.PropCorpseBlock);
        GetProperty(ec, EntityClass.PropCorpseBlockChance);
        GetProperty(ec, EntityClass.PropParticleOnSpawn);
        GetProperty(ec, EntityClass.PropParticleOnDeath);
        GetProperty(ec, EntityClass.PropNPCID);
        GetProperty(ec, EntityClass.PropAIPackages);
        GetProperty(ec, EntityClass.PropBuffs);

        if (!Options.ContainsKey("full")) return;

        //TRIVIAL PROPS
        GetProperty(ec, EntityClass.PropPrefab);
        GetProperty(ec, EntityClass.PropParent);
        GetProperty(ec, EntityClass.PropWeight);
        GetProperty(ec, EntityClass.PropPushFactor);
        GetProperty(ec, EntityClass.PropMapIcon);
        GetProperty(ec, EntityClass.PropRootMotion);
        GetProperty(ec, EntityClass.PropHasDeathAnim);

        //SOUNDS
        GetProperty(ec, EntityClass.PropSoundRandomTime);
        GetProperty(ec, EntityClass.PropSoundAlertTime);
        GetProperty(ec, EntityClass.PropSoundRandom);
        GetProperty(ec, EntityClass.PropSoundHurt);
        GetProperty(ec, EntityClass.PropSoundJump);
        GetProperty(ec, EntityClass.PropSoundPlayerLandThump);
        GetProperty(ec, EntityClass.PropSoundLandSoft);
        GetProperty(ec, EntityClass.PropSoundLandHard);
        GetProperty(ec, EntityClass.PropSoundHurtSmall);
        GetProperty(ec, EntityClass.PropSoundSprint);
        GetProperty(ec, EntityClass.PropSoundDrownPain);
        GetProperty(ec, EntityClass.PropSoundDrownDeath);
        GetProperty(ec, EntityClass.PropSoundWaterSurface);
        GetProperty(ec, EntityClass.PropSoundDeath);
        GetProperty(ec, EntityClass.PropSoundAttack);
        GetProperty(ec, EntityClass.PropSoundAlert);
        GetProperty(ec, EntityClass.PropSoundSense);
        GetProperty(ec, EntityClass.PropSoundStamina);
        GetProperty(ec, EntityClass.PropSoundLiving);
        GetProperty(ec, EntityClass.PropSoundSpawn);
        GetProperty(ec, EntityClass.PropSoundLand);
        GetProperty(ec, EntityClass.PropSoundFootstepModifier);
        GetProperty(ec, EntityClass.PropSoundGiveUp);

        //HITZONES
        GetProperty(ec, EntityClass.PropLegCrippleThreshold);
        GetProperty(ec, EntityClass.PropLegCrawlerThreshold);
        GetProperty(ec, EntityClass.PropLowerLegDismemberThreshold);
        GetProperty(ec, EntityClass.PropLowerLegDismemberBonusChance);
        GetProperty(ec, EntityClass.PropLowerLegDismemberBaseChance);
        GetProperty(ec, EntityClass.PropUpperLegDismemberThreshold);
        GetProperty(ec, EntityClass.PropUpperLegDismemberBonusChance);
        GetProperty(ec, EntityClass.PropUpperLegDismemberBaseChance);
        GetProperty(ec, EntityClass.PropLowerArmDismemberThreshold);
        GetProperty(ec, EntityClass.PropLowerArmDismemberBonusChance);
        GetProperty(ec, EntityClass.PropLowerArmDismemberBaseChance);
        GetProperty(ec, EntityClass.PropUpperArmDismemberThreshold);
        GetProperty(ec, EntityClass.PropUpperArmDismemberBonusChance);
        GetProperty(ec, EntityClass.PropUpperArmDismemberBaseChance);
        GetProperty(ec, EntityClass.PropHeadDismemberThreshold);
        GetProperty(ec, EntityClass.PropHeadDismemberBaseChance);
        GetProperty(ec, EntityClass.PropHeadDismemberBonusChance);
        GetProperty(ec, EntityClass.PropKnockdownKneelDamageThreshold);
        GetProperty(ec, EntityClass.PropKnockdownKneelStunDuration);
        GetProperty(ec, EntityClass.PropKnockdownProneDamageThreshold);
        GetProperty(ec, EntityClass.PropKnockdownProneStunDuration);
        GetProperty(ec, EntityClass.PropKnockdownProneRefillRate);
        GetProperty(ec, EntityClass.PropKnockdownKneelRefillRate);
        GetProperty(ec, EntityClass.PropArmsExplosionDamageMultiplier);
        GetProperty(ec, EntityClass.PropLegsExplosionDamageMultiplier);
        GetProperty(ec, EntityClass.PropChestExplosionDamageMultiplier);
        GetProperty(ec, EntityClass.PropHeadExplosionDamageMultiplier);

        //SLEEPER/VISION
        GetProperty(ec, EntityClass.PropSightRange);
        GetProperty(ec, EntityClass.PropSleeperWakeupSightDetectionMin);
        GetProperty(ec, EntityClass.PropSleeperWakeupSightDetectionMax);
        GetProperty(ec, EntityClass.PropSleeperGroanSightDetectionMin);
        GetProperty(ec, EntityClass.PropSleeperGroanSightDetectionMax);
        GetProperty(ec, EntityClass.PropSoundSleeperGroan);
        GetProperty(ec, EntityClass.PropSoundSleeperSnore);
        GetProperty(ec, EntityClass.PropSightScale);
        GetProperty(ec, EntityClass.PropNoiseAlertThreshold);
        GetProperty(ec, EntityClass.PropSleeperNoiseWakeThreshold);
        GetProperty(ec, EntityClass.PropSleeperNoiseGroanThreshold);
        GetProperty(ec, EntityClass.PropSmellAlertThreshold);
        GetProperty(ec, EntityClass.PropSleeperSmellWakeThreshold);
        GetProperty(ec, EntityClass.PropSleeperSmellGroanThreshold);
        GetProperty(ec, EntityClass.PropSoundSleeperGroanChance);

        //GetProperty(ec, EntityClass.PropMaxWanderVelocity);
        //GetProperty(ec, EntityClass.PropMaxApproachVelocity);
        //GetProperty(ec, EntityClass.PropMaxNightWanderVelocity);
        //GetProperty(ec, EntityClass.PropMaxNightApproachVelocity);
        //GetProperty(ec, EntityClass.PropMaxPanicVelocity);
        //GetProperty(ec, EntityClass.PropDeathType);
        //GetProperty(ec, EntityClass.PropIsEnemyEntity);
        //GetProperty(ec, EntityClass.PropIsAnimalEntity);
        //GetProperty(ec, EntityClass.PropLootListOnDeath);
        //GetProperty(ec, EntityClass.PropLootListAlive);
        //GetProperty(ec, EntityClass.PropLootListAliveMP);
        //GetProperty(ec, EntityClass.PropCompassIcon);
        //GetProperty(ec, EntityClass.PropItemsOnEnterGame);
        //GetProperty(ec, EntityClass.PropDropInventoryBlock);
        //GetProperty(ec, EntityClass.PropModelType);
        //GetProperty(ec, EntityClass.PropRagdollOnDeathChance);
        //GetProperty(ec, EntityClass.PropHasRagdoll);
        //GetProperty(ec, EntityClass.PropColliders);
        //GetProperty(ec, EntityClass.PropCrouchYOffsetFP);
        //GetProperty(ec, EntityClass.PropExperienceGain);
        //GetProperty(ec, EntityClass.PropArchetype);
        //GetProperty(ec, EntityClass.PropSwimOffset);
        //GetProperty(ec, EntityClass.PropUMARace);
        //GetProperty(ec, EntityClass.PropUMAGeneratedModelName);
        //GetProperty(ec, EntityClass.PropClassMechanimPainStates);
        //GetProperty(ec, EntityClass.PropClassMechanimPainTriggers);
        //GetProperty(ec, EntityClass.PropClassMechanimAttacks);
        //GetProperty(ec, EntityClass.PropClassMechanimAttackTriggers);
        //GetProperty(ec, EntityClass.PropClassMechanimSpecialAttacks);
        //GetProperty(ec, EntityClass.PropClassMechanimSpecialAttackTriggers);
        //GetProperty(ec, EntityClass.PropClassMechanimDeathTriggers);
        //GetProperty(ec, EntityClass.PropClassMechanimDeathStates);
        //GetProperty(ec, EntityClass.PropModelTransformAdjust);
        //GetProperty(ec, EntityClass.PropStealthSoundDecayRate);
        //GetProperty(ec, EntityClass.PropMaxTurnSpeed);
        //GetProperty(ec, EntityClass.PropSearchArea);
        //GetProperty(ec, EntityClass.PropMeshFP);
        //GetProperty(ec, EntityClass.PropClass);
        //GetProperty(ec, EntityClass.PropLocalAvatarController);
        //GetProperty(ec, EntityClass.PropSkinTexture);
        //GetProperty(ec, EntityClass.PropMaxHealth);
        //GetProperty(ec, EntityClass.PropMaxStamina);
        //GetProperty(ec, EntityClass.PropSickness);
        //GetProperty(ec, EntityClass.PropGassiness);
        //GetProperty(ec, EntityClass.PropWellness);
        //GetProperty(ec, EntityClass.PropFood);
        //GetProperty(ec, EntityClass.PropWater);
        //GetProperty(ec, EntityClass.PropIsMale);
        //GetProperty(ec, EntityClass.PropIsChunkObserver);
        //GetProperty(ec, EntityClass.PropParticleOnDestroy);
        //GetProperty(ec, EntityClass.PropCorpseBlockDensity);
      }
    }

    private void GetProperty(EntityClass ec, string prop) => Bin.Add(prop, ec.Properties.Values.ContainsKey(prop) ? ec.Properties.Values[prop] : null);

    private void GetAITargetTasks(EntityClass ec)
    {
      var i = 1;
      while (ec.Properties.Values.ContainsKey(EntityClass.PropAITargetTask + i))
      {
        AITargetTasks.Add(new BCMDynamicProp(new[]
        {
          EntityClass.PropAITargetTask + i, ec.Properties.Values[EntityClass.PropAITargetTask + i],
          ec.Properties.Params1.ContainsKey(EntityClass.PropAITargetTask + i)
            ? ec.Properties.Params1[EntityClass.PropAITargetTask + i]
            : "",
          ec.Properties.Params2.ContainsKey(EntityClass.PropAITargetTask + i)
            ? ec.Properties.Params2[EntityClass.PropAITargetTask + i]
            : ""
        }));
        i++;
      }
      Bin.Add("AITargetTasks", AITargetTasks);
    }
    private void GetAITasks(EntityClass ec)
    {
      var i = 1;
      while (ec.Properties.Values.ContainsKey(EntityClass.PropAITask + i))
      {
        AITasks.Add(new BCMDynamicProp(new[]
        {
          EntityClass.PropAITask + i, ec.Properties.Values[EntityClass.PropAITask + i],
          ec.Properties.Params1.ContainsKey(EntityClass.PropAITask + i)
            ? ec.Properties.Params1[EntityClass.PropAITask + i]
            : "",
          ec.Properties.Params2.ContainsKey(EntityClass.PropAITask + i)
            ? ec.Properties.Params2[EntityClass.PropAITask + i]
            : ""
        }));
        i++;
      }
      Bin.Add("AITasks", AITasks);
    }
    private void GetExplosionData(EntityClass ec) => Bin.Add("Explosion",
      Explosion = !ec.Properties.Values.ContainsKey("Explosion.ParticleIndex") ?
      null : (int)Utils.ParseFloat(ec.Properties.Values["Explosion.ParticleIndex"]) <= 0 ? null : new BCMExplosionData(ec.Properties));
    private void GetWalkType(EntityClass ec) => Bin.Add("WalkType", WalkType = ec.Properties.Values.ContainsKey(EntityClass.PropWalkType) ? int.Parse(ec.Properties.Values[EntityClass.PropWalkType]) : 0);
    private void GetSpeedWander(EntityClass ec) => Bin.Add("SpeedWander", SpeedWander = ec.Properties.Values.ContainsKey(EntityClass.PropSpeedWander) ? double.Parse(ec.Properties.Values[EntityClass.PropSpeedWander]) : 0);
    private void GetSpeedApproach(EntityClass ec) => Bin.Add("SpeedApproach", SpeedApproach = ec.Properties.Values.ContainsKey(EntityClass.PropSpeedApproach) ? double.Parse(ec.Properties.Values[EntityClass.PropSpeedApproach]) : 0);
    private void GetSpeedWanderNight(EntityClass ec) => Bin.Add("SpeedWanderNight", SpeedWanderNight = ec.Properties.Values.ContainsKey(EntityClass.PropSpeedWanderNight) ? double.Parse(ec.Properties.Values[EntityClass.PropSpeedWanderNight]) : 0);
    private void GetSpeedApproachNight(EntityClass ec) => Bin.Add("SpeedApproachNight", SpeedApproachNight = ec.Properties.Values.ContainsKey(EntityClass.PropSpeedApproachNight) ? double.Parse(ec.Properties.Values[EntityClass.PropSpeedApproachNight]) : 0);
    private void GetSpeedPanic(EntityClass ec) => Bin.Add("SpeedPanic", SpeedPanic = ec.Properties.Values.ContainsKey(EntityClass.PropSpeedPanic) ? double.Parse(ec.Properties.Values[EntityClass.PropSpeedPanic]) : 0);
    private void GetAttackTimeoutDay(EntityClass ec) => Bin.Add("AttackTimeoutDay", AttackTimeoutDay = ec.Properties.Values.ContainsKey(EntityClass.PropAttackTimeoutDay) ? double.Parse(ec.Properties.Values[EntityClass.PropAttackTimeoutDay]) : 0);
    private void GetAttackTimeoutNight(EntityClass ec) => Bin.Add("AttackTimeoutNight", AttackTimeoutNight = ec.Properties.Values.ContainsKey(EntityClass.PropAttackTimeoutNight) ? double.Parse(ec.Properties.Values[EntityClass.PropAttackTimeoutNight]) : 0);
    private void GetRotateToGround(EntityClass ec) => Bin.Add("RotateToGround", RotateToGround = ec.Properties.Values.ContainsKey(EntityClass.PropRotateToGround) && bool.Parse(ec.Properties.Values[EntityClass.PropRotateToGround]));
    private void GetCanClimbVertical(EntityClass ec) => Bin.Add("CanClimbVertical", CanClimbVertical = ec.Properties.Values.ContainsKey(EntityClass.PropCanClimbVertical) && bool.Parse(ec.Properties.Values[EntityClass.PropCanClimbVertical]));
    private void GetCanClimbLadders(EntityClass ec) => Bin.Add("CanClimbLadders", CanClimbLadders = ec.Properties.Values.ContainsKey(EntityClass.PropCanClimbLadders) && bool.Parse(ec.Properties.Values[EntityClass.PropCanClimbLadders]));
    private void GetHandItem(EntityClass ec) => Bin.Add("HandItem", HandItem = ec.Properties.Values.ContainsKey(EntityClass.PropHandItem) ? ec.Properties.Values[EntityClass.PropHandItem] : null);
    private void GetIsEnemy(EntityClass ec) => Bin.Add("IsEnemy", IsEnemy = ec.bIsEnemyEntity);
    private void GetLootListAlive(EntityClass ec) => Bin.Add("LootListAlive", LootListAlive = ec.Properties.Values.ContainsKey(EntityClass.PropLootListAlive) ? int.Parse(ec.Properties.Values[EntityClass.PropLootListAlive]) : 0);
    private void GetLootListOnDeath(EntityClass ec) => Bin.Add("LootListOnDeath", LootListOnDeath = ec.Properties.Values.ContainsKey(EntityClass.PropLootListOnDeath) ? int.Parse(ec.Properties.Values[EntityClass.PropLootListOnDeath]) : 0);
    private void GetDeadHP(EntityClass ec) => Bin.Add("DeadHP", DeadHP = ec.DeadBodyHitPoints);
    private void GetMaxHealth(EntityClass ec) => Bin.Add("MaxHealth", MaxHealth = ec.Properties.Values.ContainsKey(EntityClass.PropMaxHealth) ? int.Parse(ec.Properties.Values[EntityClass.PropMaxHealth]) : 100);
    private void GetExperience(EntityClass ec) => Bin.Add("Experience", Experience = ec.ExperienceValue);
    private void GetArchetype(EntityClass ec) => Bin.Add("Archetype", Archetype = ec.ArchetypeName);
    private void GetModel(EntityClass ec) => Bin.Add("Model", Model = ec.modelType);
    private void GetClass(EntityClass ec) => Bin.Add("Class", Class = ec.classname?.ToString());
    private void GetUserCanSpawn(EntityClass ec) => Bin.Add("CanSpawn", CanSpawn = ec.bAllowUserInstantiate);
    private void GetName(EntityClass entityClass) => Bin.Add("Name", Name = entityClass.entityClassName);
    private void GetId(EntityClass entityClass) => Bin.Add("Id", Id = entityClass.entityClassName.GetHashCode());
  }
}
