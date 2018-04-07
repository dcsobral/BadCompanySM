using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCHelp : BCCommandAbstract
  {
    protected override void Process()
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

      var output = new List<string> { "***Bad Company Commands***" };
      output.AddRange(
        Config.CommandDictionary.Where(kvp => kvp.Value.Name != "BCCommandAbstract")
          .Select(kvp => $"{string.Join(",", kvp.Value.Commands)} => {Config.GetDescription(kvp.Key)}")
      );

      output.Add("***Options***");
      output.Add("/log => Send the command output to the log file");
      output.Add("/chat => Send the command output to chat");
      output.Add("/console => Override command default settings for /log or /chat");
      output.Add("/color=FFFFFF => Specify a color for text sent to chat");
      output.Add("/details => For commands that support it, will give more details on items returned");
      output.Add("/nodetails => Override command default settings for /details");
      output.Add("/online => For ListPlayers commands it will display only online players (default shows all players)");
      output.Add("/offline => For ListPlayers commands it will display only offline players");
      output.Add("/all => Override command default settings for /online or /offline");
      output.Add("***Output Format Options***");
      output.Add("/1l => Returns json output on a single line (for server managers)");
      output.Add("/pp => Returns json output with print pretty enabled (default: on)");
      output.Add("/vectors => Returns all BCM Vectors as x y z objects rather than single string");
      output.Add("/csvpos =>  Converts all Vector3 co-ords to csv seperated (default is space seperated)");
      output.Add("/worldpos => Converts all Vector3 co-ords to Map Co-ords");
      output.Add("/strpos => Override command default settings for /csvpos or /worldpos");

      SendOutput(string.Join("\n", output.ToArray()));
    }
  }
}
