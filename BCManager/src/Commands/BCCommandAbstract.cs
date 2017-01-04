using System;
using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class BCCommandAbstract : ConsoleCmdAbstract
  {
    public CommandSenderInfo _senderInfo;
    public List<string> _params = null;
    public Dictionary<string,string> _options = null;

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

    public override void Execute(List<string> _p, CommandSenderInfo _s)
    {
      _senderInfo = _s;
      _params = new List<string>();
      _options = new Dictionary<string, string>();
      ParseParams(_p);
      try
      {
        Process();
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
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
        GameManager.Instance.GameMessageServer(null, EnumGameMessages.Chat, output, "Server", false, string.Empty, false);
      }
      else
      {
        SdtdConsole.Instance.Output(output);
      }
    }

  }
}
