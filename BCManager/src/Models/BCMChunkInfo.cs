namespace BCM.Models
{
  public class BCMChunkInfo
  {
    public int X;
    public int Z;
    public bool IsDisplayed;
    public int Mem;

    public BCMChunkInfo(Chunk chunk)
    {
      X = chunk.X;
      Z = chunk.Z;
      IsDisplayed = chunk.IsDisplayed;
      Mem = chunk.GetUsedMem();
    }
  }
}