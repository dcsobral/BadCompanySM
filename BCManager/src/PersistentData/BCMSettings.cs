using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace BCM.PersistentData
{
  [Serializable]
  public class BCMSettings
  {
    [OptionalField]
    private Dictionary<string, Dictionary<string, string>> _settings;

    public List<string> GetAll()
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      List<string> _s = new List<string>();
      foreach (string s in _settings.Keys)
      {
        _s.Add(s);
      }
      return _s;
    }
    public Dictionary<string, string> Get(string setting)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      if (_settings.ContainsKey(setting))
      {
        return _settings[setting];
      }
      return null;
    }
    public bool Set(string setting, Dictionary<string, string> value)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      if (_settings.ContainsKey(setting))
      {
        foreach (string key in value.Keys)
        {
          _settings[setting].Add(key, value[key]);
          return true;
        }
      }
      return false;
    }

    public string GetValue(string setting, string key)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      if (_settings.ContainsKey(setting))
      {
        if (_settings[setting].ContainsKey(key))
        {
          return _settings[setting][key];
        }
      }
      return null;
    }
    public void SetValue(string setting, string key, string value)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      if (!_settings.ContainsKey(setting))
      {
        Dictionary<string, string> neurondict = new Dictionary<string, string>();
        neurondict.Add(key, value);
        _settings.Add(setting, neurondict);
      }
      else
      {
        if (!_settings[setting].ContainsKey(key))
        {
          _settings[setting].Add(key, value);
        }
        else
        {
          _settings[setting][key] = value;
        }
      }
    }
    public bool Clear(string setting)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      if (_settings.ContainsKey(setting))
      {
        _settings[setting].Clear();
        return true;
      }
      return false;
    }
    public bool ClearValue(string setting, string key, string value)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, string>>();
      }
      if (_settings.ContainsKey(setting))
      {
        if (_settings[setting][key] != null)
        {
          _settings[setting][key] = null;
          return true;
        }
      }
      return false;
    }

  }
}
