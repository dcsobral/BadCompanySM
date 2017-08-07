using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BCM
{
  public static class BCUtils
  {
    public static string ColorToHex(UnityEngine.Color _color)
    {
      return string.Format("{0:X02}{1:X02}{2:X02}", (int)(_color.r * 255), (int)(_color.g * 255), (int)(_color.b * 255));
    }

    public static Dictionary<int, Entity> filterEntities(Dictionary<int, Entity> _entities, Dictionary<string, string> _options)
    {
      var entities = new Dictionary<int, Entity>();
      foreach (var _e in _entities)
      {
        if (_options.ContainsKey("all"))
        {
          entities.Add(_e.Key, _e.Value);
        }
        else if (_options.ContainsKey("type"))
        {
          if (_e.Value != null)
          {
            if (_e.Value.GetType().ToString() == _options["type"])
            {
              entities.Add(_e.Key, _e.Value);
            }
          }
        }
        else if (_options.ContainsKey("istype"))
        {
          if (_e.Value != null)
          {
            Type type = Type.GetType(_e.Value.GetType().AssemblyQualifiedName.Replace(_e.Value.GetType().ToString(), _options["istype"]));

            if (type != null)
            {
              if (type.IsInstanceOfType(_e.Value))
              {
                entities.Add(_e.Key, _e.Value);
              }
            }
          }
        }
        else
        {
          if (_e.Value is EntityEnemy || _e.Value is EntityAnimal)
          {
            entities.Add(_e.Key, _e.Value);
          }
        }
      }

      return entities;
    }

    //todo: remove
    public static string toJson(Dictionary<string, Dictionary<string, string>> data)
    {
      StringBuilder strb = new StringBuilder();
      strb.Append("{");
      bool first = true;
      foreach (string key in data.Keys)
      {
        if (!first) { strb.Append(","); } else { first = false; }
        strb.Append(string.Format('"' + "{0}" + '"', key));
        //strb.Append(string.Format("\"{0}\"", key));
        strb.Append(":");
        strb.Append(string.Format("{0}", toJson(data[key])));
      }
      strb.Append("}");
      return strb.ToString();
    }

    public static string toJson(Dictionary<string, string> data)
    {
      StringBuilder strb = new StringBuilder();
      bool first = true;
      if (data.Keys == null && data.Keys.Count == 0)
      {
        strb.Append("null");
      }
      else
      {
        strb.Append("{");
        foreach (string key in data.Keys)
        {
          if (!first) { strb.Append(","); } else { first = false; }
          strb.Append(string.Format('"' + "{0}" + '"', key));
          //strb.Append(string.Format("\"{0}\"", key));
          strb.Append(":");
          if (data[key].Length > 0 && (data[key].Substring(0, 1) == "{" || data[key].Substring(0, 1) == "["))
          {
            strb.Append(string.Format("{0}", data[key]));
          }
          else
          {
            strb.Append(string.Format('"' + "{0}" + '"', data[key]));
            //strb.Append(string.Format("\"{0}\"", data[key]));
          }
        }
        strb.Append("}");
      }
      return strb.ToString();
    }
    public static string toJson(List<string> data)
    {
      StringBuilder strb = new StringBuilder();
      bool first = true;

      strb.Append("[");
      foreach (string value in data)
      {
        if (!first) { strb.Append(","); } else { first = false; }
        strb.Append(string.Format("{0}", value));
      }
      strb.Append("]");

      return strb.ToString();
    }
  }
}
