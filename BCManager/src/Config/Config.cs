using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using BCM.Models;
using static System.String;

namespace BCM
{
  public static class Config
  {
    public static readonly Dictionary<string, BCMCommand> CommandDictionary = new Dictionary<string, BCMCommand>();
    public const string ModPrefix = "(BCM)";
    private static readonly string ModDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
    private const string CommandsFile = "Commands.xml";
    private const string SystemFile = "System.xml";
    private static readonly string DefaultConfigPath = ModDir + "DefaultConfig" + Path.DirectorySeparatorChar;
    public static readonly string ConfigPath = ModDir + "Config" + Path.DirectorySeparatorChar;
    public static readonly string DefaultEventsPath = DefaultConfigPath + "Events" + Path.DirectorySeparatorChar;
    public static readonly string EventsPath = ConfigPath + "Events" + Path.DirectorySeparatorChar;
    public static string DefaultLocale = "en";

    //todo: implement filesystemwatcher so changes to the config are dynamically updated
    private static FileSystemWatcher _fsw;
    private static void ProcessChangedConfig(object item, FileSystemEventArgs args)
    {
      Log.Out($"{ModPrefix} System.xml Changed");
    }

    private static void AddFsWatch()
    {
      _fsw = new FileSystemWatcher(ConfigPath, SystemFile);
      _fsw.Changed += ProcessChangedConfig;
      _fsw.Created += ProcessChangedConfig;
      _fsw.Deleted += ProcessChangedConfig;
      _fsw.EnableRaisingEvents = true;
    }

    public static void Init()
    {
      LoadCommands();
      ConfigureSystem();
      AddFsWatch();
    }

    private static void ConfigureSystem()
    {
      var xmlDoc = new XmlDocument();
      try
      {
        //todo: Load defaults, then merge in settings from Config/system.xml and persistent data
        xmlDoc.Load(File.Exists($"{ConfigPath}{SystemFile}")
          ? $"{ConfigPath}{SystemFile}"
          : $"{DefaultConfigPath}{SystemFile}");

        //Heartbeat.IsAlive
        var isAlive = xmlDoc.SelectNodes("/System/Heartbeat/@isalive");
        if (isAlive == null || isAlive.Count == 0)
        {
          Log.Out($"{ModPrefix} Unable to load Heartbeat IsAlive, setting not found in {SystemFile}");
        }
        else
        {
          if (!bool.TryParse(isAlive.Item(0)?.Value, out Heartbeat.IsAlive))
          {
            Log.Out($"{ModPrefix} Unable to load Heartbeat, isalive is not a valid boolean in {SystemFile}");
          }
        }

        //Heartbeat.BPM
        var bpm = xmlDoc.SelectNodes("/System/Heartbeat/@bpmrate");
        if (bpm == null || bpm.Count <= 0)
        {
          Log.Out($"{ModPrefix} Unable to load Heartbeat BPM rate, setting not found in {SystemFile}");
        }
        else
        {
          if (!int.TryParse(bpm.Item(0)?.Value, out Heartbeat.Bpm))
          {
            Log.Out($"{ModPrefix} Unable to load BPM, \'rate\' is not a valid int in {SystemFile}");
          }
        }

        //Synapses
        var synapseNodes = xmlDoc.SelectNodes("/System/Synapse");
        if (synapseNodes == null || synapseNodes.Count == 0)
        {
          Log.Out($"{ModPrefix} Unable to load Synapses, settings not found in {SystemFile}");
        }
        else
        {
          var count = 0;
          foreach (XmlElement node in synapseNodes)
          {
            count++;
            var synapse = new Synapse();

            if (!node.HasAttribute("name"))
            {
              Log.Out($"{ModPrefix} Skipping Synapse element #{count}, missing \'name\' attribute in {SystemFile}");
              continue;
            }
            synapse.Name = node.GetAttribute("name");

            if (!node.HasAttribute("enabled"))
            {
              Log.Out($"{ModPrefix} Skipping Synapse element #{count}, missing \'enabled\' attribute in {SystemFile}");
              continue;
            }
            if (!bool.TryParse(node.GetAttribute("enabled"), out synapse.IsEnabled))
            {
              Log.Out($"{ModPrefix} Unable to load Synapse \'enabled\' in element #{count}, value is not a valid boolean in {SystemFile}");
            }

            if (node.HasAttribute("beats") && !int.TryParse(node.GetAttribute("beats"), out synapse.Beats))
            {
              Log.Out($"{ModPrefix} Unable to load Synapse \'beats\' in element #{count}, value not a valid int in {SystemFile}");
              continue;
            }

            if (node.HasAttribute("options"))
            {
              synapse.Options = node.GetAttribute("options");
            }

            if (node.HasAttribute("config"))
            {
              synapse.Cfg = node.GetAttribute("config");
            }

            synapse.WireNeurons();
            Brain.BondSynapse(synapse);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error($"{ModPrefix} Error configuring tasks\n{e}");
      }
    }

    private static void LoadCommands()
    {
      var xmlDoc = new XmlDocument();
      try
      {
        if (File.Exists(ConfigPath + CommandsFile))
        {
          xmlDoc.Load(ConfigPath + CommandsFile);
        }
        else
        {
          xmlDoc.Load(DefaultConfigPath + CommandsFile);
        }

        var locale = xmlDoc.SelectNodes("/Commands/DefaultLocale");
        if (locale != null && locale.Count > 0)
        {
          DefaultLocale = ((XmlElement)locale.Item(0))?.GetAttribute("value");
        }
        else
        {
          Log.Out($"{ModPrefix} Using default locale \'{DefaultLocale}\', setting not found in {CommandsFile}");
        }

        var commands = xmlDoc.SelectNodes("/Commands/Command");

        if (commands != null && commands.Count > 0)
        {

          var count = 0;
          foreach (XmlElement element in commands)
          {
            var command = new BCMCommand();
            var cmdLocale = $"{ModDir}/Config/Commands/{DefaultLocale}";
            count++;

            // NAME
            if (!element.HasAttribute("name"))
            {
              Log.Out($"{ModPrefix} Skipping Command element #{count}, missing \'name\' attribute in {CommandsFile}");

              continue;
            }
            command.Name = element.GetAttribute("name");

            // COMMANDS
            if (!element.HasAttribute("commands"))
            {
              Log.Out($"{ModPrefix} Skipping Command element #{count}, missing \'commands\' attribute {CommandsFile}");

              continue;
            }
            command.Commands = element.GetAttribute("commands").Split(',');

            // DEFAULTPERMISSIONLEVEL [optional]
            if (element.HasAttribute("defaultpermissionlevel"))
            {
              int.TryParse(element.GetAttribute("defaultpermissionlevel"), out command.DefaultPermission);
            }

            // DEFAULTOPTIONS [optional]
            if (element.HasAttribute("defaultoptions"))
            {
              command.DefaultOptions = element.GetAttribute("defaultoptions");
            }

            // HELP [optional]
            var helpfile = $"{cmdLocale}/Help/{command.Name}.txt";
            if (File.Exists(helpfile))
            {
              var fs = File.OpenText(helpfile);
              command.Help = fs.ReadToEnd();
              fs.Close();
            }

            // DESCRIPTION [optional]
            var descfile = $"{cmdLocale}/Description/{command.Name}.txt";
            if (File.Exists(descfile))
            {
              var fs = File.OpenText(descfile);
              command.Description = fs.ReadToEnd();
              fs.Close();
            }

            // +COMMAND
            CommandDictionary.Add(command.Name, command);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error($"{ModPrefix} Error loading config files\n{e}");
      }
    }

    public static string GetDescription(string command)
    {
      if (command == "BCCommandAbstract" || !CommandDictionary.Keys.Contains(command)) return Empty;

      return IsNullOrEmpty(CommandDictionary[command].Description)
        ? $"{ModPrefix} {command}"
        : $"{ModPrefix} {CommandDictionary[command].Description}";
    }

    public static string GetHelp(string command)
    {
      if (command == "BCCommandAbstract" || !CommandDictionary.Keys.Contains(command)) return Empty;

      if (IsNullOrEmpty(CommandDictionary[command].Help))
      {
        return $"{ModPrefix} {command}\nNo Help Available.\n";
      }

      return ModPrefix + " " + CommandDictionary[command]
        .Help
        .Replace("{description}", CommandDictionary[command].Description)
        .Replace("{commands}", Join(", ", CommandDictionary[command].Commands))
        .Replace("{command}", CommandDictionary[command].Commands[0])
        .Replace("{defaultoptions}",
          CommandDictionary[command]
            .DefaultOptions.Split(',')
            .Aggregate("", (current, split) => $"{current}/{split} ")
        );
    }

    public static string[] GetCommands(string command)
    {
      return command == "BCCommandAbstract" || !CommandDictionary.Keys.Contains(command) ?
        new[] { Empty } : CommandDictionary[command].Commands;
    }

    public static int GetDefaultPermissionLevel(string command)
    {
      return command == "BCCommandAbstract" || !CommandDictionary.Keys.Contains(command) ?
        0 : CommandDictionary[command].DefaultPermission;
    }

    public static string GetDefaultOptions(string command)
    {
      return command == "BCCommandAbstract" || !CommandDictionary.Keys.Contains(command) ?
        Empty : CommandDictionary[command].DefaultOptions;
    }
  }
}
