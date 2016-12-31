using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCCommandAbstract : ConsoleCmdAbstract
  {
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

    }
  }
}
