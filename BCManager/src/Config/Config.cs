using BCM.ConfigModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace BCM
{
  public static class Config
  {
    public const string ModPrefix = "(BCM)";
    private static string DllPath = Utils.GetDirectoryFromPath(System.Reflection.Assembly.GetExecutingAssembly().Location);
    public static string ModDir
    {
      get
      {
        string _filepath = string.Empty;
        int num = DllPath.LastIndexOf('/');
        if (num == -1)
        {
          num = DllPath.LastIndexOf('\\');
        }
        if (num > 0 && num + 1 < DllPath.Length)
        {
          _filepath = DllPath.Substring(0, num + 1);
        }
        return _filepath;
      }
    }
    private static string commandsFile = "Commands.xml";
    private static string systemFile = "System.xml";
    private static string ConfigPath = ModDir + "Config/";
    public static string DefaultLocale = "en";
    public static bool logCache = false;
    public static Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>();

    public static bool Init()
    {
      LoadCommands();
      ConfigureSystem();
      // todo: load hubcelldata for all cells to populate world data properly and create a cache for list commands?
      // todo: generate json data files for list game object commands and store in cache
      return true;
    }

    public static bool ConfigureSystem()
    {
      XmlDocument _xd = new XmlDocument();
      try
      {
        _xd.Load(ConfigPath + systemFile);

        //LogCache.enabled
        XmlNodeList _lc = _xd.SelectNodes("/System/LogCache/@enabled");
        if (_lc.Count > 0)
        {
          if (!bool.TryParse(_lc.Item(0).Value, out logCache))
          {
            Log.Out("" + ModPrefix + " Unable to load LogCache, enabled is not a valid boolean in " + systemFile);
          }
        }
        else
        {
          Log.Out("" + ModPrefix + " Unable to load LogCache enabled, setting not found in " + systemFile);
        }

        //Heartbeat.IsAlive
        XmlNodeList _hb = _xd.SelectNodes("/System/Heartbeat/@isalive");
        if (_hb.Count > 0)
        {
          if (!bool.TryParse(_hb.Item(0).Value, out Heartbeat.IsAlive))
          {
            Log.Out("" + ModPrefix + " Unable to load Heartbeat, isalive is not a valid boolean in " + systemFile);
          }
        }
        else
        {
          Log.Out("" + ModPrefix + " Unable to load Heartbeat IsAlive, setting not found in " + systemFile);
        }

        //Heartbeat.BPM
        XmlNodeList _bpm = _xd.SelectNodes("/System/BPM/@rate");
        if (_bpm.Count > 0)
        {
          if (!int.TryParse(_bpm.Item(0).Value, out Heartbeat.BPM))
          {
            Log.Out("" + ModPrefix + " Unable to load BPM, 'rate' is not a valid int in " + systemFile);
          }
        }
        else
        {
          Log.Out("" + ModPrefix + " Unable to load BPM, setting not found in " + systemFile);
        }

        //Synapses
        XmlNodeList _synapses = _xd.SelectNodes("/System/Synapse");
        if (_synapses.Count > 0)
        {
          int count = 0;
          foreach (XmlElement _s in _synapses)
          {
            count++;
            Synapse _synapse = new Synapse();

            if (!_s.HasAttribute("name"))
            {
              Log.Out("" + ModPrefix + " Skipping Synapse element #" + count + ", missing 'name' attribute in " + systemFile);
              continue;
            }
            else
            {
              _synapse.name = _s.GetAttribute("name");
            }

            if (!_s.HasAttribute("enabled"))
            {
              Log.Out("" + ModPrefix + " Skipping Synapse element #" + count + ", missing 'enabled' attribute in " + systemFile);
              continue;
            }
            else
            {
              if (!bool.TryParse(_s.GetAttribute("enabled"), out _synapse.IsEnabled))
              {
                Log.Out("" + ModPrefix + " Unable to load Synapse 'enabled' in element #" + count + ", value is not a valid boolean in " + systemFile);
              }
            }

            if (!_s.HasAttribute("beats"))
            {
              Log.Out("" + ModPrefix + " Skipping Synapse element #" + count + ", missing 'beats' attribute in " + systemFile);
              continue;
            }
            else
            {
              if (!int.TryParse(_s.GetAttribute("beats"), out _synapse.beats))
              {
                Log.Out("" + ModPrefix + " Unable to load Synapse 'beats' in element #" + count + ", value not a valid int in " + systemFile);
              }
            }

            if (_s.HasAttribute("options"))
            {
              _synapse.options = _s.GetAttribute("options");
            }

            _synapse.WireNeurons();
            Brain.BondSynapse(_synapse);
          }
        }
        else
        {
          Log.Out("" + ModPrefix + " Unable to load Synapses, settings not found in " + systemFile);
        }
      }
      catch (Exception e)
      {
        Log.Error("" + ModPrefix + " Error configuring tasks\n" + e);
        return false;
      }
      return true;
    }

    public static bool LoadCommands()
    {
      XmlDocument _xd = new XmlDocument();
      try
      {
        _xd.Load(ConfigPath + commandsFile);
        XmlNodeList _locale = _xd.SelectNodes("/Commands/DefaultLocale");
        if (_locale.Count > 0)
        {
          DefaultLocale = ((XmlElement)_locale.Item(0)).GetAttribute("value");
        }
        else
        {
          Log.Out("" + ModPrefix + " Using DefaultLocale '" + DefaultLocale + "', setting not found in " + commandsFile);
        }

        XmlNodeList _commands = _xd.SelectNodes("/Commands/Command");

        if (_commands.Count > 0)
        {

          int count = 0;
          foreach (XmlElement _el in _commands)
          {
            Command _command = new Command();
            count++;
            // command name
            if (!_el.HasAttribute("name"))
            {
              Log.Out("" + ModPrefix + " Skipping Command element #" + count + ", missing 'name' attribute in " + commandsFile);
              continue;
            }
            else
            {
              _command.name = _el.GetAttribute("name");
            }

            // command commands
            if (!_el.HasAttribute("commands"))
            {
              Log.Out("" + ModPrefix + " Skipping Command element #" + count + ", missing 'commands' attribute " + commandsFile);
              continue;
            }
            else
            {
              _command.commands = _el.GetAttribute("commands").Split(',');
            }

            // command defaultpermissionlevel
            if (_el.HasAttribute("defaultpermissionlevel"))
            {
              int.TryParse(_el.GetAttribute("defaultpermissionlevel"), out _command.dpl);
            }

            // command defaultoptions
            if (_el.HasAttribute("defaultoptions"))
            {
              _command.defaultoptions = _el.GetAttribute("defaultoptions");
            }

            // load Description and Help text
            string cmdLocale = ModDir + "/Config/Commands/" + DefaultLocale;
            string helpfile = cmdLocale + "/Help/" + _command.name + ".txt";
            string descfile = cmdLocale + "/Description/" + _command.name + ".txt";
            if (File.Exists(helpfile))
            {
              StreamReader fs = File.OpenText(helpfile);
              _command.help = fs.ReadToEnd();
              fs.Close();
            }
            if (File.Exists(descfile))
            {
              StreamReader fs = File.OpenText(descfile);
              _command.description = fs.ReadToEnd();
              fs.Close();
            }

            // add command to dictionary
            commandDictionary.Add(_command.name, _command);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error("" + ModPrefix + " Error loading config files\n" + e);
        return false;
      }
      return true;
    }
    public static string GetDescription(string command)
    {
      if (command == "BCCommandAbstract")
      {
        return string.Empty;
      }
      if (commandDictionary.Keys.Contains(command))
      {
        if (commandDictionary[command].description != "" && commandDictionary[command].description != null)
        {
          return "" + ModPrefix + " " + commandDictionary[command].description;
        }
        else
        {
          return "" + ModPrefix + " " + command;
        }
      }
      return string.Empty;
    }
    public static string GetHelp(string command)
    {
      if (command == "BCCommandAbstract")
      {
        return string.Empty;
      }
      if (commandDictionary.Keys.Contains(command))
      {
        if (commandDictionary[command].help != "" && commandDictionary[command].help != null)
        {
          string help = commandDictionary[command].help;
          help = help.Replace("{description}", commandDictionary[command].description);
          help = help.Replace("{commands}", string.Join(", ", commandDictionary[command].commands));
          help = help.Replace("{command}", commandDictionary[command].commands[0]);
          string[] splitDefaults = commandDictionary[command].defaultoptions.Split(',');
          string slashdefaultoptions = "";
          foreach (string split in splitDefaults)
          {
            slashdefaultoptions += "/" + split + " ";
          }
          help = help.Replace("{defaultoptions}", slashdefaultoptions);
          return "" + ModPrefix + " " + help;
        }
        else
        {
          return "" + ModPrefix + " " + command + "\nNo Help Available.\n";
        }
      }
      return string.Empty;
    }
    public static string[] GetCommands(string command)
    {
      if (command == "BCCommandAbstract")
      {
        return new string[] { string.Empty };
      }
      if (commandDictionary.Keys.Contains(command))
      {
        return commandDictionary[command].commands;
      }
      else
      {
        return new string[] { string.Empty };
      }
    }
    public static int GetDefaultPermissionLevel(string command)
    {
      if (command == "BCCommandAbstract")
      {
        return 0;
      }
      if (commandDictionary.Keys.Contains(command))
      {
        return commandDictionary[command].dpl;
      }
      else
      {
        return 0;
      }
    }
    public static string GetDefaultOptions(string command)
    {
      if (command == "BCCommandAbstract")
      {
        return "";
      }
      if (commandDictionary.Keys.Contains(command))
      {
        return commandDictionary[command].defaultoptions;
      }
      else
      {
        return "";
      }
    }
  }
}
