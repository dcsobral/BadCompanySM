using UnityEngine;

namespace BCM.Commands
{
  public class ListItems : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      for (var i = 0; i <= ItemClass.list.Length - 1; i++)
      {
        if (ItemClass.list[i] != null)
          if (ItemClass.list[i].IsBlock() == false)
          {
            output += ItemClass.list[i].Name;
            if (_options.ContainsKey("itemids"))
            {
              output += "(" + (i - 4096) + ")";
            }
            output += _sep;
          }
      }
      SendOutput(output);
    }
  }
}
