namespace BCM
{
  public class BCMPrefabCache
  {
    public readonly string Filename;
    public Vector3i Pos;

    public BCMPrefabCache(string filename, Vector3i pos)
    {
      Filename = filename;
      Pos = pos;
    }
  }
}
