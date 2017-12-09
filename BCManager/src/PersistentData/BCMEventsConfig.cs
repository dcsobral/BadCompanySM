using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Steamworks;

namespace BCM.PersistentData
{
  [Serializable]
  public class BCMEventsConfig
  {
    private readonly Dictionary<string, NeuronConfig> _eventsConfig = new Dictionary<string, NeuronConfig>();

    public int Count => _eventsConfig.Count;

    [CanBeNull]
    public NeuronConfig this[string name, bool create]
    {
      get
      {
        if (string.IsNullOrEmpty(name))
        {
          return null;
        }

        if (_eventsConfig.ContainsKey(name))
        {
          return _eventsConfig[name];
        }

        if (!create) return null;

        var c = new NeuronConfig(name);
        _eventsConfig.Add(name, c);
        return c;
      }
    }
  }

  [Serializable]
  public class NeuronConfig
  {
    public string Name;
    public Dictionary<string, object> Settings = new Dictionary<string, object>();

    public NeuronConfig(string name)
    {
      Name = name;
    }

    public NeuronConfig SetItem(string key, object value)
    {
      if (Settings.ContainsKey(key))
      {
        Settings[key] = value;
      }
      else
      {
        Settings.Add(key, value);
      }
      return this;
    }
  }
}
