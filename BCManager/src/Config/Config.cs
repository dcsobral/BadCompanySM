using BCM.ConfigModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace BCM
{
  public static class Config
  {
    public static string ModPrefix = "(BCM)";
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
    private static string commandsFile = ModDir + "Config/Commands.xml";
    public static string DefaultLocale = "en";
    private static Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>();

    public static bool Load()
    {
      XmlDocument _xd = new XmlDocument();
      try
      {
        _xd.Load(commandsFile);
        //XmlNode _de = _xd.DocumentElement;
        XmlNodeList _locale = _xd.SelectNodes("/Commands/DefaultLocale");
        if (_locale.Count > 0)
        {
          DefaultLocale = ((XmlElement)_locale.Item(0)).GetAttribute("value");
        }
        else
        {
          Log.Out("" + ModPrefix + " Using DefaultLocale '" + DefaultLocale + "', setting not found in config");
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
              Log.Out("" + ModPrefix + " Skipping Command element #" + count + ", missing 'name' attribute");
              continue;
            }
            else
            {
              _command.name = _el.GetAttribute("name");
            }

            // command commands
            if (!_el.HasAttribute("commands"))
            {
              Log.Out("" + ModPrefix + " Skipping Command element #" + count + ", missing 'commands' attribute");
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
          return "" + ModPrefix + " " + commandDictionary[command].help;
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
  }
}
