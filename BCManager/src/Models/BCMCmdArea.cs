using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMCmdArea : BCCmd
  {
    public string Filter;
    public ushort Radius;

    public bool HasPos;
    public BCMVector3 Position;

    public bool HasSize;
    public BCMVector3 Size;

    public bool HasChunkPos;
    public BCMVector4 ChunkBounds;
    public ItemStack ItemStack;

    public Dictionary<string, string> Opts;
    public List<string> Pars;
    public string CmdType;

    public BCMCmdArea(List<string> pars, Dictionary<string, string> options, string cmdType)
    {
      Opts = options;
      Pars = pars;
      CmdType = cmdType;
    }

    public bool IsWithinBounds(Vector3i pos)
    {
      if (!HasPos) return false;

      var size = HasSize ? Size : new BCMVector3(0, 0, 0);
      return (Position.x <= pos.x && pos.x <= Position.x + size.x) &&
             (Position.y <= pos.y && pos.y <= Position.y + size.y) &&
             (Position.z <= pos.z && pos.z <= Position.z + size.z);
    }
  }
}
