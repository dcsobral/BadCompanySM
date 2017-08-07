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
        //todo: get by name
        ItemClass _item = null;
        int i = -1;
        if (int.TryParse(_params[0], out i))
        {
          _item = ItemClass.list[i];
        }
        if (_item == null)
        {
          SdtdConsole.Instance.Output("Itemclass not found.");

          return;
        }

        var item = new BCMItemClass(ItemClass.list[i], _options);
        if (_options.ContainsKey("nokeys"))
        {
          List<List<object>> keyless = new List<List<object>>();
          List<object> obj = new List<object>();
          foreach (var d in item.Data())
          {
            obj.Add(d.Value);
          }
          keyless.Add(obj);
          SendJson(keyless);
        }
        else
        {
          SendJson(item.Data());
        }
      }
      else
      {
        List<object> data = new List<object>();
        List<List<object>> keyless = new List<List<object>>();

        int start = 0;
        int end = ItemClass.list.Length;
        if (_options.ContainsKey("items"))
        {
          start = 4097;
        }
        else if (_options.ContainsKey("blocks"))
        {
          end = 4097;
        }

        //todo: Use /items or /blocks as filter, using the individual filters for the type.
        //      Use bc-ic /items /filters to get the specific key lists
        //      If not set the /filter uses the general key list, skipping keys that aren't available on that type

        for (var i = start; i <= end - 1; i++)
        {
          if (ItemClass.list[i] != null)
          {
            var item = new BCMItemClass(ItemClass.list[i], _options);
            if (_options.ContainsKey("nokeys"))
            {
              var obj = new List<object>();
              foreach (var d in item.Data())
              {
                obj.Add(d.Value);
              }
              keyless.Add(obj);
            }
            else
            {
              data.Add(item.Data());
            }
          }
        }

        if (_options.ContainsKey("nokeys"))
        {
          SendJson(keyless);
        }
        else
        {
          SendJson(data);
        }
      }
    }
  }
}
