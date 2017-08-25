using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCHelp : BCCommandAbstract
  {
    public override void Process()
    {
      //todo: change to a string builder instead of output each line, then apss to SendOutput
      //todo: pull options text from a config file
      //todo: add permission checking to display only commands sender has permission to execute
      //      AdminTools.CommandAllowedFor(string[] _cmdNames, string _playerId)
      SendOutput("***Bad Company Commands***");
      foreach(KeyValuePair<string, BCMCommand> kvp in Config.CommandDictionary)
      {
        if (kvp.Value.Name != "BCCommandAbstract")
        {
          SendOutput(string.Join(", ", kvp.Value.Commands) + " => " + Config.GetDescription(kvp.Key));
        }
      }
      SendOutput("***Options***");
      SendOutput("/log => Send the command output to the log file");
      SendOutput("/chat => Send the command output to chat");
      SendOutput("/console => Override command default settings for /log or /chat");
      SendOutput("/color=FFFFFF => Specify a color for text sent to chat");
      SendOutput("/details => For commands that support it, will give more details on items returned");
      SendOutput("/nodetails => Override command default settings for /details");
      SendOutput("/online => For ListPlayers commands it will display only online players (default shows all players)");
      SendOutput("/offline => For ListPlayers commands it will display only offline players");
      SendOutput("/all => Override command default settings for /online or /offline");
      //SendOutput("/json => for commands that support it, will return the data in json encoded format");
      SendOutput("***Output Format Options***");
      SendOutput("/1l => Returns json output on a single line (for server managers)");
      SendOutput("/pp => Returns json output with print pretty enabled (default: on)");
      //SendOutput("/nl => Uses a newline to seperate items (can be combined with /csv)");
      //SendOutput("/csv => Uses a comma to seperate items (can be combined with /nl)");
      //SendOutput("/nocsv or /nonl => Override command default settins for /csv and /nl");
      SendOutput("/vectors => Returns all BCM Vectors as x y z objects rather than single string");
      SendOutput("/csvpos =>  Converts all Vector3 co-ords to csv seperated (default is space seperated)");
      SendOutput("/worldpos => Converts all Vector3 co-ords to Map Co-ords");
      SendOutput("/strpos => Override command default settings for /csvpos or /worldpos");
    }
  }
}
