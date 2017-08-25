using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BCM.PersistentData
{
  [Serializable]
  public class BCMSettings
  {
    [OptionalField]
    private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>> _settings;

    private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>> Settings
    {
      get
      {
        _settings = _settings ?? new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
        return _settings;
      }
    }

    public object Data()
    {
      return Settings;
    }

    public List<string> GetCollectionNames()
    {
      return Settings.Keys.ToList();
    }

    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetCollection(string collectionName)
    {
      return Settings.ContainsKey(collectionName) ? Settings[collectionName] : null;
    }

    public Dictionary<string, string> GetFunction(string collection, string collectionId, string function)
    {
      return Settings.ContainsKey(collection)
        ? (Settings[collection].ContainsKey(collectionId)
          ? (Settings[collection][collectionId].ContainsKey(function)
            ? Settings[collection][collectionId][function]
            : null)
          : null)
        : null;
    }

    public string GetValue(string collection, string collectionId, string function, string key)
    {
      return Settings.ContainsKey(collection)
        ? (Settings[collection].ContainsKey(collectionId)
          ? (Settings[collection][collectionId].ContainsKey(function)
            ? (Settings[collection][collectionId][function].ContainsKey(key)
              ? Settings[collection][collectionId][function][key]
              : null)
            : null)
          : null)
        : null;
    }
    public void SetValue(string collection, string collectionId, string function, string key, string value)
    {
      if (Settings.ContainsKey(collection))
      {
        if (Settings[collection].ContainsKey(collectionId))
        {
          if (Settings[collection][collectionId].ContainsKey(function))
          {
            if (Settings[collection][collectionId][function].ContainsKey(key))
            {
              Settings[collection][collectionId][function][key] = value;
            }
            else
            {
              Settings[collection][collectionId][function].Add(key, value);
            }
          }
          else
          {
            Settings[collection][collectionId].Add(function, new Dictionary<string, string> { { key, value } });
          }
        }
        else
        {
          Settings[collection].Add(collectionId, new Dictionary<string, Dictionary<string, string>>
          {
            {function, new Dictionary<string, string> {{ key, value}}}
          });
        }
      }
      else
      {
        Settings.Add(collection, new Dictionary<string, Dictionary<string, Dictionary<string, string>>>
        {
          {
            collectionId, new Dictionary<string, Dictionary<string, string>>
            {
              {function, new Dictionary<string, string> {{ key, value}}}
            }
          }
        });
      }
    }
  }
}
