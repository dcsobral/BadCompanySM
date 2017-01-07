using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListMaterials : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      Dictionary<string, MaterialBlock> materials = MaterialBlock.materials;
      foreach (string id in materials.Keys)
      {
        output += id;
        if (_options.ContainsKey("details"))
        {
          output += "(hp=" + materials[id].MaxDamage + ",hardness=" + materials[id].Hardness + ",mass=" + materials[id].Mass + ",glue=" + materials[id].StabilityGlue + ",support=" + materials[id].StabilitySupport + ")";
        }
        output += _sep;
      }
      SendOutput(output);
    }
  }
}
