using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace BCM.PersistentData
{
  [Serializable]
  public class BCMSettings
  {
    [OptionalField]
    private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>> _settings;

    public List<string> GetCollectionNames()
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
      }
      List<string> _s = new List<string>();
      foreach (string s in _settings.Keys)
      {
        _s.Add(s);
      }
      return _s;
    }

    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetCollection(string collectionName)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
      }
      if (_settings.ContainsKey(collectionName))
      {
        return _settings[collectionName];
      }
      return null;
    }

    //public bool Set(string setting, Dictionary<string, string> value)
    //{
    //  if (_settings == null)
    //  {
    //    _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
    //  }
    //  if (_settings.ContainsKey(setting))
    //  {
    //    foreach (string key in value.Keys)
    //    {
    //      _settings[setting].Add(key, value[key]);
    //      return true;
    //    }
    //  }
    //  return false;
    //}
    
    public Dictionary<string, string> GetFunction(string collection, string collectionId, string function)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
      }
      if (_settings.ContainsKey(collection))
      {
        if (_settings[collection].ContainsKey(collectionId))
        {
          if (_settings[collection][collectionId].ContainsKey(function))
          {
            return _settings[collection][collectionId][function];
          }
        }
      }
      return null;
    }

    public string GetValue(string collection, string collectionId, string function, string key)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
      }
      if (_settings.ContainsKey(collection))
      {
        if (_settings[collection].ContainsKey(collectionId))
        {
          if (_settings[collection][collectionId].ContainsKey(function))
          {
            if (_settings[collection][collectionId][function].ContainsKey(key))
            {
              return _settings[collection][collectionId][function][key];
            }
          }
        }
      }
      return null;
    }
    public void SetValue(string collection, string collectionId, string function, string key, string value)
    {
      if (_settings == null)
      {
        _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
      }

      if (_settings.ContainsKey(collection))
      {
        if (_settings[collection].ContainsKey(collectionId))
        {
          if (_settings[collection][collectionId].ContainsKey(function))
          {
            if (_settings[collection][collectionId][function].ContainsKey(key))
            {
              _settings[collection][collectionId][function][key] = value;
            }
            else
            {
              _settings[collection][collectionId][function].Add(key, value);
            }
          }
          else
          {
            var kvp = new Dictionary<string, string>();
            kvp.Add(key, value);
            _settings[collection][collectionId].Add(function, kvp);
          }
        }
        else
        {
          var kvp2 = new Dictionary<string, Dictionary<string, string>>();
          var kvp = new Dictionary<string, string>();
          kvp.Add(key, value);
          kvp2.Add(function, kvp);
          _settings[collection].Add(collectionId, kvp2);
        }
      }
      else
      {
        var kvp3 = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        var kvp2 = new Dictionary<string, Dictionary<string, string>>();
        var kvp = new Dictionary<string, string>();
        kvp.Add(key, value);
        kvp2.Add(function, kvp);
        kvp3.Add(collectionId, kvp2);
        _settings.Add(collection, kvp3);
      }
    }





    //public bool Clear(string setting)
    //{
    //  if (_settings == null)
    //  {
    //    _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
    //  }
    //  if (_settings.ContainsKey(setting))
    //  {
    //    _settings[setting].Clear();
    //    return true;
    //  }
    //  return false;
    //}
    //public bool ClearValue(string setting, string key, string value)
    //{
    //  if (_settings == null)
    //  {
    //    _settings = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
    //  }
    //  if (_settings.ContainsKey(setting))
    //  {
    //    if (_settings[setting][key] != null)
    //    {
    //      _settings[setting][key] = null;
    //      return true;
    //    }
    //  }
    //  return false;
    //}

  }
}
