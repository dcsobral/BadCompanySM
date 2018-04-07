using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMSpawnPoint
  {
    [UsedImplicitly] public BCMVector3 Pos;
    [UsedImplicitly] public BCMVector3 Look;
    [UsedImplicitly] public double Rot;
    [UsedImplicitly] public int Pose;
    [UsedImplicitly] public int BlockId;

    public BCMSpawnPoint(SleeperVolume.SpawnPoint spawn)
    {
      Pos = new BCMVector3(spawn.pos);
      Look = new BCMVector3(spawn.look);
      Rot = spawn.rot;
      Pose = spawn.pose;
      BlockId = spawn.blockID;
    }
  }
}