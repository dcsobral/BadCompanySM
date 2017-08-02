using BCM.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListItemClass : BCCommandAbstract
  {
    private void Filters()
    {
      var data = new Dictionary<string, string>();
      var obj = typeof(BCMItemClass.StrFilters);
      foreach (var field in obj.GetFields())
      {
        data.Add(field.Name, field.GetValue(obj).ToString());
      }

      SendJson(data);
    }

    private void Index()
    {
      var data = new Dictionary<string, int>();
      foreach (var value in Enum.GetValues(typeof(BCMItemClass.Filters)))
      {
        data.Add(value.ToString(), (int)value);
      }

      SendJson(data);
    }

    public override void Process()
    {
      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      if (_options.ContainsKey("filters"))
      {
        Filters();
        return;
      }
      if (_options.ContainsKey("index"))
      {
        Index();
        return;
      }

      if (_params.Count > 1)
      {
        SendOutput("Wrong number of arguments");
        SendOutput(Config.GetHelp(GetType().Name));

        return;
      }

      if (_params.Count == 1)
      {
        int i = 0;
        if (int.TryParse(_params[0], out i))
        {
          if (ItemClass.list[i] != null)
          {
            var item = new BCMItemClass(ItemClass.list[i], _options);
            SendJson(item.Data());
          }
        }
      }
      else
      {
        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();

        int start = 0;
        int end = ItemClass.list.Length;
        if (_options.ContainsKey("type"))
        {
          if (_options["type"] == "items")
          {
            start = 4097;
          }
          else if (_options["type"] == "blocks")
          {
            end = 4097;
          }
        }
        for (var i = start; i <= end - 1; i++)
        {
          if (ItemClass.list[i] != null)
          {
            var item = new BCMItemClass(ItemClass.list[i], _options);
            data.Add(i.ToString(), item.Data());
          }
        }
        SendJson(data);
      }
    }
  }
}
