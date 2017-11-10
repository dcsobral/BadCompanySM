using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMDynamicProp
  {
    public string Name;
    public string Value;
    public string Param1;
    public string Param2;

    public BCMDynamicProp(IList<string> prop)
    {
      Name = prop[0];
      Value = prop[1];
      Param1 = prop[2];
      Param2 = prop[3];
    }
  }
}
