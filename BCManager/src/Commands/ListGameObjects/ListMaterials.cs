using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListMaterials : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      Dictionary<string, MaterialBlock> materials = MaterialBlock.materials;

      var i = 0;
      foreach (MaterialBlock material in materials.Values)
      {

        Dictionary<string, string> details = new Dictionary<string, string>();

        details.Add("Id", (material.id != null ? material.id : ""));
        details.Add("DamageCategory", (material.DamageCategory != null ? material.DamageCategory : ""));
        details.Add("FertileLevel", material.FertileLevel.ToString());
        details.Add("ForgeCategory", (material.ForgeCategory != null ? material.ForgeCategory : "" ));
        details.Add("Friction", material.Friction.ToString());
        details.Add("Hardness", (material.Hardness != null ? material.Hardness.ToString()  : ""));
        details.Add("IsCollidable", material.IsCollidable.ToString());
        details.Add("IsGroundCover", material.IsGroundCover.ToString());
        details.Add("IsLiquid", material.IsLiquid.ToString());
        details.Add("IsPlant", material.IsPlant.ToString());
        details.Add("LightOpacity", material.LightOpacity.ToString());
        details.Add("Mass", (material.Mass != null ? material.Mass.ToString() : ""));
        details.Add("MaxDamage", material.MaxDamage.ToString());
        details.Add("MovementFactor", material.MovementFactor.ToString());
        details.Add("ParticleCategory", (material.ParticleCategory != null ? material.ParticleCategory : ""));
        details.Add("ParticleDestroyCategory", (material.ParticleDestroyCategory != null ? material.ParticleDestroyCategory : ""));
        details.Add("Resistance", material.Resistance.ToString());
        details.Add("StabilityGlue", material.StabilityGlue.ToString());
        details.Add("StabilitySupport", material.StabilitySupport.ToString());
        details.Add("stepSound", (material.stepSound != null ? material.stepSound.name : ""));
        details.Add("SurfaceCategory", (material.SurfaceCategory != null ? material.SurfaceCategory : ""));

        string jsonDetails = BCUtils.toJson(details);
        data.Add(i.ToString(), jsonDetails);
        i++;
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());
        SendOutput(output);
      }
      else
      {
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
}
