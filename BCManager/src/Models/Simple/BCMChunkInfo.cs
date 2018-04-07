using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMChunkInfo
  {
    [UsedImplicitly] public int X;
    [UsedImplicitly] public int Z;
    [UsedImplicitly] public bool IsDisplayed;
    [UsedImplicitly] public int Mem;

    public BCMChunkInfo([NotNull] Chunk chunk)
    {
      X = chunk.X;
      Z = chunk.Z;
      IsDisplayed = chunk.IsDisplayed;
      Mem = chunk.GetUsedMem();
    }
  }
}