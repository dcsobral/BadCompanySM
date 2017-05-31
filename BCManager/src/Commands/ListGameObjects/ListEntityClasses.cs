using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListEntityClasses : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();
      DictionarySave<int, EntityClass> classes = EntityClass.list;

      foreach (int key in classes.Keys)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        details.Add("Id", key.ToString());
        details.Add("entityClassName", (classes[key].entityClassName != null ? classes[key].entityClassName : ""));
        details.Add("bAllowUserInstantiate", classes[key].bAllowUserInstantiate.ToString());

        //DYNAMIC PROPERTIES
        Dictionary<string, string> properties = new Dictionary<string, string>();
        foreach (string current in classes[key].Properties.Values.Keys)
        {
          if (classes[key].Properties.Values.ContainsKey(current))
          {
            properties.Add(current, classes[key].Properties.Values[current].ToString());
          }
        }
        string jsonProperties = BCUtils.toJson(properties);
        details.Add("Properties", jsonProperties);

        var jsonDetails = BCUtils.toJson(details);
        data.Add(key.ToString(), jsonDetails);
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
        foreach (EntityClass ec in EntityClass.list.Values)
        {
          output += ec.entityClassName + ":" + ec.classname + "" + _sep;
        }
        SendOutput(output);
      }
    }
  }
}
