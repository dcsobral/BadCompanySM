using System;
using System.Collections.Generic;

namespace BCM
{
  public static class BCUtils
  {
    public static string UIntToHex(uint c)
    {
      return ColorToHex(UIntToColor(c));
    }

    public static UnityEngine.Color UIntToColor(uint c)
    {
      var a = (byte)(c >> 24);
      var r = (byte)(c >> 16);
      var g = (byte)(c >> 8);
      var b = (byte)c;
      return new UnityEngine.Color32(r, g, b, 255);
    }

    public static string ColorToHex(UnityEngine.Color color)
    {
      return $"{(int) (color.r * 255):X02}{(int) (color.g * 255):X02}{(int) (color.b * 255):X02}";
    }

    public static Dictionary<int, Entity> FilterEntities(Dictionary<int, Entity> entities, Dictionary<string, string> options)
    {
      var filteredEntities = new Dictionary<int, Entity>();

      foreach (var e in entities)
      {
        if (options.ContainsKey("all"))
        {
          filteredEntities.Add(e.Key, e.Value);
        }
        else if (options.ContainsKey("type"))
        {
          if (e.Value == null) continue;

          if (e.Value.GetType().ToString() != options["type"]) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
        else if (options.ContainsKey("istype"))
        {
          if (e.Value == null) continue;

          var name = e.Value.GetType().AssemblyQualifiedName;
          if (name == null) continue;

          var type = Type.GetType(name.Replace(e.Value.GetType().ToString(), options["istype"]));
          if (type == null) continue;

          if (!type.IsInstanceOfType(e.Value)) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
        else
        {
          if (!(e.Value is EntityEnemy) && !(e.Value is EntityAnimal)) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
      }

      return filteredEntities;
    }
  }
}
