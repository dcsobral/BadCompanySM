//using System;
//using UnityEngine;
//using XMLData.Item;

//public class EntityZombieBC : EntityEnemy
//{
//  public ulong timeToDie;

//  public float eyeHeightHackMod = 1f;

//  public bool IsRunning
//  {
//    get
//    {
//      int @int = GamePrefs.GetInt(EnumGamePrefs.ZombiesRun);
//      if (@int == 0)
//      {
//        int arg_67_0;
//        if (!this.isFeral)
//        {
//          if (!this.world.IsDark())
//          {
//            arg_67_0 = 0;
//            return arg_67_0 != 0;
//          }
//        }
//        arg_67_0 = ((base.Stats.SpeedModifier.Value > 0.7f) ? 1 : 0);
//        return arg_67_0 != 0;
//      }
//      if (@int == 1)
//      {
//        return false;
//      }
//      return true;
//    }
//  }

//  protected override void Awake()
//  {
//    base.Awake();
//    this.MaxLedgeHeight = 20;
//  }

//  public override void PostInit()
//  {
//    if (this.emodel != null)
//    {
//      this.emodel.PostInit();
//    }
//    if (this.Health <= 0 && this.isEntityRemote)
//    {
//      base.ClientKill(DamageResponse.New(true));
//    }
//  }

//  public void ResetDismemberment(bool _restoreState)
//  {
//    base.ExecuteDismember(_restoreState);
//  }

//  protected override void updateStepSound(float _distX, float _distZ)
//  {
//  }

//  public override void Init(int _entityClass)
//  {
//    base.Init(_entityClass);
//    this.timeToDie = this.world.worldTime + 1800uL + (ulong)(22000f * UnityEngine.Random.value);
//  }

//  public override void InitFromPrefab(int _entityClass)
//  {
//    base.InitFromPrefab(_entityClass);
//    this.timeToDie = this.world.worldTime + 1800uL + (ulong)(22000f * UnityEngine.Random.value);
//  }

//  public override void OnUpdateLive()
//  {
//    base.OnUpdateLive();
//    if (this.world.worldTime >= this.timeToDie)
//    {
//      if (!this.isEntityRemote)
//      {
//        this.Kill(DamageResponse.New(true));
//      }
//    }
//  }

//  public override Ray GetLookRay()
//  {
//    Ray result = new Ray(this.position + new Vector3(0f, this.GetEyeHeight() * this.eyeHeightHackMod, 0f), this.GetLookVector());
//    return result;
//  }

//  public override Vector3 GetLookVector()
//  {
//    if (this.lookAtPosition.Equals(Vector3.zero))
//    {
//      return base.GetLookVector();
//    }
//    return this.lookAtPosition - this.getHeadPosition();
//  }

//  public override float GetApproachSpeed()
//  {
//    int @int = GamePrefs.GetInt(EnumGamePrefs.ZombiesRun);
//    if (@int == 0)
//    {
//      return base.GetApproachSpeed();
//    }
//    if (@int == 1)
//    {
//      return this.speedApproach * base.Stats.SpeedModifier.Value;
//    }
//    return this.speedApproachNight * base.Stats.SpeedModifier.Value;
//  }

//  protected override float getNextStepSoundDistance()
//  {
//    float arg_27_0;
//    if (this.IsRunning)
//    {
//      arg_27_0 = 1.5f;
//    }
//    else
//    {
//      arg_27_0 = 0.5f;
//    }
//    return arg_27_0;
//  }

//  protected override bool CheckDamageMask(EnumEquipmentSlot slot)
//  {
//    if (this.IsDead())
//    {
//      return base.CheckDamageMask(slot);
//    }
//    return slot == EnumEquipmentSlot.Head || slot == EnumEquipmentSlot.Chest;
//  }

//  public override void OnEntityDeath()
//  {
//    base.OnEntityDeath();
//  }

//  protected override Vector3i dropCorpseBlock()
//  {
//    if (this.lootContainer != null)
//    {
//      if (this.lootContainer.IsUserAccessing())
//      {
//        return Vector3i.zero;
//      }
//    }
//    Vector3i vector3i = base.dropCorpseBlock();
//    if (vector3i == Vector3i.zero)
//    {
//      return Vector3i.zero;
//    }
//    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(new NetPackageTileEntityCreate(TileEntityType.Loot, vector3i), new IPackageDestinationFilter[0]);
//    Chunk chunk = (Chunk)this.world.GetChunkFromWorldPos(vector3i);
//    TileEntityLootContainer tileEntityLootContainer = (TileEntityLootContainer)TileEntity.Instantiate(TileEntityType.Loot, chunk);
//    if (this.lootContainer != null)
//    {
//      tileEntityLootContainer.CopyLootContainerDataFromOther(this.lootContainer);
//    }
//    else
//    {
//      tileEntityLootContainer.lootListIndex = this.lootListOnDeath;
//      tileEntityLootContainer.SetContainerSize(LootContainer.lootList[this.lootListOnDeath].size, true);
//    }
//    tileEntityLootContainer.localChunkPos = World.toBlock(vector3i);
//    chunk.AddTileEntity(tileEntityLootContainer);
//    tileEntityLootContainer.SetModified();
//    return vector3i;
//  }
//}
