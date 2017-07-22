//using BCM.Models;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEngine;

//namespace BCM.Commands
//{
//  public class ListItems : BCCommandAbstract
//  {
//    public virtual Dictionary<string, string> jsonObject()
//    {
//      Dictionary<string, string> data = new Dictionary<string, string>();

//      for (var i = 0; i <= ItemClass.list.Length - 1; i++)
//      {
//        if (ItemClass.list[i] != null)
//        {
//          if (ItemClass.list[i].IsBlock() == false)
//          {
//            BCMItemClass bi = new BCMItemClass(ItemClass.list[i]);
//            data.Add(ItemClass.list[i].Id.ToString(), bi.GetJson());
//          }
//        }
//      }

//      return data;
//    }

//    public override void Process()
//    {
//      string output = "";
//      if (_options.ContainsKey("json"))
//      {
//        if (_options.ContainsKey("tag"))
//        {
//          if (_options["tag"] == null)
//          {
//            _options["tag"] = "bc-items";
//          }

//          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
//        }
//        else
//        {
//          SendOutput(BCUtils.toJson(jsonObject()));
//        }
//      }
//      else
//      {
//        for (var i = 0; i <= ItemClass.list.Length - 1; i++)
//        {
//          if (ItemClass.list[i] != null)
//            if (ItemClass.list[i].IsBlock() == false)
//            {
//              output += ItemClass.list[i].Name;
//              if (_options.ContainsKey("itemids"))
//              {
//                output += "(" + (i - 4096) + ")";
//              }
//              output += _sep;
//            }
//        }
//        SendOutput(output);
//      }
//    }
//  }
//}
