using RWG2.Rules;
using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMPrefabInfo
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public double Prob;
    [UsedImplicitly] public bool List;
    [UsedImplicitly] public int Min;
    [UsedImplicitly] public int Max;

    public BCMPrefabInfo([NotNull] PrefabInfo info)
    {
      Name = info.Name;
      Prob = Math.Round(info.Prob, 3);
      List = info.FilteredList;
      Min = info.MinCount;
      Max = info.MaxCount;
    }
  }
}
