using BCM.ConfigModels;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCHelp : BCCommandAbstract
  {
    public override void Process()
    {
      // todo: add permission checking to display only commands sender has permission to execute
      // AdminTools.CommandAllowedFor(string[] _cmdNames, string _playerId)
      SdtdConsole.Instance.Output("***Bad Company Commands***");
      foreach(KeyValuePair<string, Command> kvp in Config.commandDictionary)
      {
        if (kvp.Value.description != string.Empty)
        {
          SdtdConsole.Instance.Output(string.Join(", ", kvp.Value.commands) + " => " + Config.GetDescription(kvp.Key));
        }
      }
      SdtdConsole.Instance.Output("***Options***");
      SdtdConsole.Instance.Output("/log => Send the command output to the log file");
      SdtdConsole.Instance.Output("/chat => Send the command output to chat");
      SdtdConsole.Instance.Output("/console => Override command default settings for /log or /chat");
      SdtdConsole.Instance.Output("/color=FFFFFF => Specify a color for text sent to chat");
      SdtdConsole.Instance.Output("/details => For commands that support it, will give more details on items returned");
      SdtdConsole.Instance.Output("/nodetails => Override command default settings for /details");
      SdtdConsole.Instance.Output("/online => For ListPlayers commands it will display only online players (default shows all players)");
      SdtdConsole.Instance.Output("/offline => For ListPlayers commands it will display only offline players");
      SdtdConsole.Instance.Output("/all => Override command default settings for /online or /offline");
      //      SdtdConsole.Instance.Output("/json => for commands that support it, will return the data in json encoded format");
      SdtdConsole.Instance.Output("***Output Format Options***");
      SdtdConsole.Instance.Output("/nl => Uses a newline to seperate items (can be combined with /csv)");
      SdtdConsole.Instance.Output("/csv => Uses a comma to seperate items (can be combined with /nl)");
      SdtdConsole.Instance.Output("/nocsv or /nonl => Override command default settins for /csv and /nl");
      SdtdConsole.Instance.Output("/csvpos =>  Converts all Vector3 co-ords to csv seperated (default is space seperated)");
      SdtdConsole.Instance.Output("/worldpos => Converts all Vector3 co-ords to Map Co-ords");
      SdtdConsole.Instance.Output("/spacepos => Override command default settins for /csvpos or /worldpos");
    }
  }
}
