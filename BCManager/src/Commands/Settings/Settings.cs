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
        List<string> settings = _settings.GetAll();
        output += "Settings(" + settings.Count + ")" + _sep;


        if (settings.Count > 0)
        {
          foreach (string n in settings)
          {
            Dictionary<string, string> s = _settings.Get(n);
            output += n + "(" + s.Count + ")" + _sep;
            if (s.Count > 0)
            {
              foreach (string k in s.Keys)
              {
                string v = _settings.GetValue(n, k);
                output += "  " + k + ": " + v + _sep;
              }
            }
          }
        }
      }

      if (_params.Count == 1)
      {
        //todo: report details on setting
      }

      if (_params.Count == 2)
      {
        string jsonvalues = _settings.GetValue(_params[0], _params[1]);
        if (jsonvalues != null)
        {
          output += "Command/Key:" + _params[0] + "/" + _params[1] + _sep;
          output += "Previous Value:" + jsonvalues + _sep;
        }
        Dictionary<string, string> values = JsonUtility.FromJson<Dictionary<string, string>>(jsonvalues);

        foreach (string option in _options.Keys)
        {
          if (_options[option] != null)
          {
            if (values.ContainsKey(option))
            {
              values[option] = _options[option];
            }
            else
            {
              values.Add(option, _options[option]);
            }
          }
        }
        string encodedvalues = JsonUtility.ToJson(values);
        _settings.SetValue(_params[0], _params[1], encodedvalues);
        PersistentContainer.Instance.Save();
      }

      SendOutput(output);
    }
  }
}
