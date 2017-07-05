using System;
using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class BCCommandAbstract : ConsoleCmdAbstract
  {
    public CommandSenderInfo _senderInfo;
    public List<string> _params = new List<string>();
    public Dictionary<string,string> _options = new Dictionary<string, string>();
    public string _sep = "";

    public override string GetDescription()
    {
      return Config.GetDescription(GetType().Name);
    }

    public override string GetHelp()
    {
      return Config.GetHelp(GetType().Name);
    }

    public override string[] GetCommands()
    {
      return Config.GetCommands(GetType().Name);
    }

    public override int DefaultPermissionLevel
    {
      get
      {
        return Config.GetDefaultPermissionLevel(GetType().Name);
      }
    }

    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      this._senderInfo = _senderInfo;
      this._params = new List<string>();
      _options = new Dictionary<string, string>();
      ParseParams(_params);
      try
      {
        Process();
      }
      catch (Exception e)
      {
        SdtdConsole.Instance.Output("Error while executing command.");
        Log.Out(Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    public virtual void Process()
    {
      // function to override in extention commands instead of Execute
      // this allows param parsing and exception handling to be done in this class
    }

    //public enum CommandParams
    //{
    //  Player,
    //  Entity,
    //  x,
    //  y,
    //  z,
    //  Text
    //}

    public void ParseParams(List<string> _p)
    {
      int pindex = 0;
      foreach (string _param in _p)
      {
        if (_param.IndexOf('/', 0) == 0)
        {
          if (_param.IndexOf('=', 1) != -1)
          {
            string[] p1 = _param.Substring(1).Split('=');
            _options.Add(p1[0], p1[1]);
          }
          else
          {
            _options.Add(_param.Substring(1), null);
          }
        }
        else
        {
          _params.Add(_param);
          pindex++;
        }
      }

      string defaultoptions = Config.GetDefaultOptions(GetType().Name);
      string[] addDefaults = defaultoptions.Split(',');
      foreach (string add in addDefaults)
      {
        if (
          (add == "online" && (_options.ContainsKey("offline") || _options.ContainsKey("all")))
          ||
          (add == "offline" && (_options.ContainsKey("online") || _options.ContainsKey("all")))
          ||
          (add == "nl" && _options.ContainsKey("nonl"))
          ||
          (add == "csv" && _options.ContainsKey("nocsv"))
          ||
          (add == "details" && _options.ContainsKey("nodetails"))
          ||
          (add == "csvpos" && _options.ContainsKey("spacepos"))
          ||
          (add == "worldpos" && _options.ContainsKey("spacepos"))
          ||
          (_options.ContainsKey(add))
          )
        {
          continue;
        }
        else 
        {
          _options.Add(add, null);
        }
      }

      _sep = "";
      if (_options.ContainsKey("csv"))
      {
        _sep += ",";
      }
      if (_options.ContainsKey("nl"))
      {
        _sep += "\n";
      }
      if (_sep == "")
      {
        _sep = " ";
      }

    }
    public void SendOutput(string output)
    {
      if (_options.ContainsKey("log"))
      {
        Log.Out(output);
      }
      else if (_options.ContainsKey("chat"))
      {
        if (_options.ContainsKey("color"))
        {
          output = "[" + _options["color"] + "]" + output + "[-]";
        }
        string[] split = output.Split('\n');
        foreach (string text in split)
        {
          GameManager.Instance.GameMessageServer(null, EnumGameMessages.Chat, text, "Server", false, string.Empty, false);
        }
      }
      else
      {
        string[] split = output.Split('\n');
        foreach (string text in split)
        {
          SdtdConsole.Instance.Output(text);
        }
      }
    }

  }
}
