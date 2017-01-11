using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class Test : BCCommandAbstract
  {
    public override void Process()
    {
      string steamid = "76561198106697782";
      PlayerDataReader pdr = new PlayerDataReader();
      pdr.GetData(steamid);

      foreach (Skill s in pdr.skills)
      {
        SdtdConsole.Instance.Output(s.Name + "\n");
      }

    }
  }


}
