using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMItem
  {
    public int iv_type;
    public bool iv_HasQuality;
    public int iv_quality;
    public int count;
    public int iv_maxUseTimes;
    public int iv_useTimes;
    public bool iv_activated;
    public byte iv_ammo;
    [OptionalField]
    public string json_attachments;
    [OptionalField]
    public string json_parts;
    [NonSerialized]
    public ItemValue itemValue;
    [NonSerialized]
    public ItemValue[] attachments;
    [NonSerialized]
    public ItemValue[] parts;

    public BCMItem(ItemStack i)
    {
      if (i.itemValue != null && i.itemValue.type != 0)
      {
        Parse(i);
      }
    }
    public BCMItem(ItemValue i)
    {
      if (i != null && i.type != 0)
      {
        Parse(i);

      }
    }

    public void Parse(ItemStack item)
    {
      count = item.count;
      Parse(item.itemValue);
    }

    public void Parse(ItemValue item)
    {
      //todo: magazine contents for guns

      iv_type = item.type;
      iv_HasQuality = item.HasQuality;
      if (iv_HasQuality)
      {
        iv_quality = item.Quality;
      }
      if (count == 0)
      {
        count = 1;
      }
      iv_maxUseTimes = item.MaxUseTimes;
      iv_useTimes = item.UseTimes;
      iv_activated = item.Activated;
      iv_ammo = item.SelectedAmmoTypeIndex;

      if (item.Attachments != null)
      {
        if (item.Attachments.Length > 0)
        {
          string json_att = null;
          bool first = true;
          foreach (ItemValue attachment in item.Attachments)
          {
            if (attachment.type != 0)
            {
              string json = "";
              BCMItem at = new BCMItem(attachment);
              if (!first) { json += ","; } else { json += "["; first = false; }
              json += at.GetJson();
              json_att += json;
            }
          }
          if (json_att != null && json_att.Length > 0)
          {
            json_att += "]";
            json_attachments = json_att;
          }
        }
      }

      if (item.Parts != null)
      {
        if (item.Parts.Length > 0)
        {
          string json_pts = null;
          bool first = true;
          foreach (ItemValue part in item.Parts)
          {
            if (part != null && part.type != 0)
            {
              string json = "";
              BCMItem at = new BCMItem(part);
              if (!first) { json += ","; } else { json += "["; first = false; }
              json += at.GetJson();
              json_pts += json;
            }
          }
          if (json_pts != null && json_pts.Length > 0)
          {
            json_pts += "]";
            json_parts = json_pts;
          }
        }
      }
    }

    public string GetJson()
    {
      StringBuilder strb = new StringBuilder();
      strb.Append("{");

      strb.Append(string.Format("\"{0}\"", "iv_type"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_type));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "iv_HasQuality"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_HasQuality));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "iv_quality"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_quality));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "count"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", count));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "iv_maxUseTimes"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_maxUseTimes));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "iv_useTimes"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_useTimes));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "iv_activated"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_activated));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "iv_ammo"));
      strb.Append(":");
      strb.Append(string.Format("\"{0}\"", iv_ammo));
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "json_attachments"));
      strb.Append(":");
      if (json_attachments != null && json_attachments.Length > 0)
      {
        strb.Append(string.Format("{0}", json_attachments));
      }
      else
      {
        strb.Append(string.Format("{0}", "null"));
      }
      strb.Append(",");

      strb.Append(string.Format("\"{0}\"", "json_parts"));
      strb.Append(":");
      if (json_parts != null && json_parts.Length > 0)
      {
        strb.Append(string.Format("{0}", json_parts));
      }
      else
      {
        strb.Append(string.Format("{0}", "null"));
      }

      strb.Append("}");
      return strb.ToString();



      //return JsonUtility.ToJson(this);
    }

  }
}
