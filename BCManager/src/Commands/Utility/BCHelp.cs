using System.Collections.Generic;
using System.Linq;

namespace BCM.Commands
{
  public class BCHelp : BCCommandAbstract
  {
    public override void Process()
    {
      if (Options.ContainsKey("sm"))
      {
        if (Params.Count > 0)
        {
          var command = SdtdConsole.Instance.GetCommand(Params[0]);
          if (command == null)
          {
            SendJsonError($"Command not found: {Params[0]}");

            return;
          }

          var help = command.GetHelp();
          var helpLines = help.Split('\n');
          var i = 0;
          foreach (var helpLine in helpLines)
          {
            helpLines[i] = helpLine.TrimEnd('\r');
            i++;
          }
          SendJson(helpLines);

          return;
        }

        var commands = new Dictionary<string, object>();
        foreach (var command in SdtdConsole.Instance.GetCommands())
        {
          var key = command.GetType().ToString();
          var comString = string.Join(" ", command.GetCommands());
          var i = 0;
          if (commands.ContainsKey(key))
          {
            while (commands.ContainsKey($"{key}_{i}"))
            {
              i++;
            }
            key = $"{key}_{i}";
          }
          commands.Add(key, new { Commands = comString, Description = command.GetDescription(), Permission = command.DefaultPermissionLevel });
        }
        SendJson(commands);

        return;
      }

      //todo: pull options text from a config file
      //todo: add permission checking to display only commands sender has permission to execute
      //AdminTools.CommandAllowedFor(string[] _cmdNames, string _playerId)
      var output = Config.CommandDictionary.Where(kvp => kvp.Value.Name != "BCCommandAbstract").Aggregate(
        "***Bad Company Commands***",
        (current, kvp) => current + string.Join(", ", kvp.Value.Commands) + " => " + Config.GetDescription(kvp.Key));

      output += "***Options***";
      output += "/log => Send the command output to the log file";
      output += "/chat => Send the command output to chat";
      output += "/console => Override command default settings for /log or /chat";
      output += "/color=FFFFFF => Specify a color for text sent to chat";
      output += "/details => For commands that support it, will give more details on items returned";
      output += "/nodetails => Override command default settings for /details";
      output += "/online => For ListPlayers commands it will display only online players (default shows all players)";
      output += "/offline => For ListPlayers commands it will display only offline players";
      output += "/all => Override command default settings for /online or /offline";
      output += "***Output Format Options***";
      output += "/1l => Returns json output on a single line (for server managers)";
      output += "/pp => Returns json output with print pretty enabled (default: on)";
      output += "/vectors => Returns all BCM Vectors as x y z objects rather than single string";
      output += "/csvpos =>  Converts all Vector3 co-ords to csv seperated (default is space seperated)";
      output += "/worldpos => Converts all Vector3 co-ords to Map Co-ords";
      output += "/strpos => Override command default settings for /csvpos or /worldpos";

      SendOutput(output);
    }
  }
}
