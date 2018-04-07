using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMDynamicProp
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Value;
    [UsedImplicitly] public string Param1;
    [UsedImplicitly] public string Param2;

    public BCMDynamicProp([NotNull] IList<string> prop)
    {
      if (prop.Count < 4) return;

      Name = prop[0];
      Value = prop[1];
      Param1 = prop[2];
      Param2 = prop[3];
    }
  }
}
