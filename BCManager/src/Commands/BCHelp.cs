using BCM.ConfigModels;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCHelp : BCCommandAbstract
  {
    public override void Process()
    {
      // todo: add permission checking to display only commands sender has permission to execute
      SdtdConsole.Instance.Output("***Bad Company Commands***");
      foreach(KeyValuePair<string, Command> kvp in Config.commandDictionary)
      {
        if (kvp.Value.description != string.Empty)
        {
          SdtdConsole.Instance.Output(string.Join(", ", kvp.Value.commands) + " => " + Config.GetDescription(kvp.Key));
        }
      }
      SdtdConsole.Instance.Output("***Options***");
      SdtdConsole.Instance.Output("/log => send the command output to the log file");
      SdtdConsole.Instance.Output("/chat => send the command output to chat");
      SdtdConsole.Instance.Output("/color=FFFFFF => specify a color for text sent to chat");
      SdtdConsole.Instance.Output("/detail => for commands that support it, will give more details on items returned");
      SdtdConsole.Instance.Output("/json => for commands that support it, will return the data in json encoded format");
    }
  }
}
