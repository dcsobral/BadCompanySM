using UnityEngine;

namespace BCM.Commands
{
  public class ListBlocks : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      for (var i = 0; i <= ItemClass.list.Length - 1; i++)
      {
        if (ItemClass.list[i] != null)
          if (ItemClass.list[i].IsBlock() == true)
          {
            output += ItemClass.list[i].Name;
            if (_options.ContainsKey("itemids"))
            {
              output += "(" + i + ")";
            }
            output += _sep;
          }
      }
      SendOutput(output);
    }
  }
}
