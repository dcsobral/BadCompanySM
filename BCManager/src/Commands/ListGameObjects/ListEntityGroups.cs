using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListEntityGroups : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      foreach (string entitygroupname in EntityGroups.list.Keys)
      {
        List<SEntityClassAndProb> EG = EntityGroups.list[entitygroupname];
        output += entitygroupname + ":" + EG.Count + "" + _sep;
      }
      SendOutput(output);
    }
  }
}
