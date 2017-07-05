using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListArchetypes : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();
      string[] names = Archetypes.Instance.GetArchetypeNames();

      for (var i = 0; i <= names.Length - 1; i++)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        Archetype a = Archetypes.Instance.GetArchetype(names[i]);

        details.Add("Name", (a.Name != null ? a.Name : ""));
        details.Add("IsMale", a.IsMale.ToString());
        details.Add("HairColor", (a.HairColor != null ? a.HairColor.ToString() : ""));
        details.Add("EyeColor", (a.EyeColor != null ? a.EyeColor.ToString() : ""));
        details.Add("SkinColor", (a.SkinColor != null ? a.SkinColor.ToString() : ""));
        details.Add("Type", a.Type.ToString());

        //todo:
        //a.BaseSlots;
        //a.Dna;
        //a.ExpressionData;
        //a.PreviewSlots;
        //a.VoiceSet;

        var jsonDetails = BCUtils.toJson(details);
        data.Add(i.ToString(), jsonDetails);
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        if (_options.ContainsKey("tag"))
        {
          if (_options["tag"] == null)
          {
            _options["tag"] = "bc-archetypes";
          }

          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
        }
        else
        {
          SendOutput(BCUtils.toJson(jsonObject()));
        }
      }
      else
      {
        foreach (string name in Archetypes.Instance.GetArchetypeNames())
        {
          output += name + _sep;
        }
        SendOutput(output);
      }
    }
  }
}
