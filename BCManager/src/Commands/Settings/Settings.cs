using BCM.PersistentData;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class Settings : BCCommandAbstract
  {
    public override void Process()
    {
      BCMSettings _settings = PersistentContainer.Instance.Settings;
      string output = "";

      if (_params.Count == 0)
      {
        //todo: a method to clear values

        List<string> collectionNames = _settings.GetCollectionNames();
        output += "[";

        if (collectionNames.Count > 0)
        {
          foreach (string _name in collectionNames)
          {
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> _collection = _settings.GetCollection(_name);

            if (_collection != null && _collection.Count > 0)
            {
              output += "{\"name\":\"" + _name + "\", \"count\":\"" + _collection.Count + "\",\"items\":[";
              foreach (string _id in _collection.Keys)
              {
                string jsonItem = BCUtils.toJson(_collection[_id]);
                output += "{\"id\":\"" + _id + "\",\"functions\":" + jsonItem + "},";
              }
              output = output.Substring(0, output.Length - 1);
              output += "]},";
            }
          }
          output = output.Substring(0, output.Length - 1);
        }
        output += "]";
      }

      if (_params.Count == 1)//with no =
      {
        //todo: report details on setting

      //same output as 0 params but with deeper root node. root=param1 
      }

      if (_params.Count == 2)//or count=1 and contains valid setting kvp (kvp:a.b.c.d=v vs 2par:a.b.c.d v)
      {
        var _path = _params[0].Split('.');

        if (_path.Length != 4)
        {
          SendOutput("Incorrect format for the key string (four strings seperated by a dot.)");

          //todo: allow 3 parts + an array of kvp from options. i.e. Player.8200889977663366.SpawnManager /opt1=val1 /opt2=val2
          return;
        }


        string value = _settings.GetValue(_path[0], _path[1], _path[2], _path[3]);
        if (value != null)
        {
          output += "New Value:" + _params[0] + "=" + _params[1] + _sep;
          output += "Prev Value:" + value + _sep;
        }
        _settings.SetValue(_path[0], _path[1], _path[2], _path[3], _params[1]);
        PersistentContainer.Instance.Save();
      }

      SendOutput(output);
    }
  }
}
