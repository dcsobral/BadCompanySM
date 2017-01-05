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
            output += ItemClass.list[i].Name + "(" + (i - 4096) + ")" + _sep;
          }
      }
      SendOutput(output);
    }
  }
}
