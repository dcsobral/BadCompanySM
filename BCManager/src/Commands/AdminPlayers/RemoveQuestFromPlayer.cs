namespace BCM.Commands
{
  public class RemoveQuestFromPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count != 2)
      {
        SdtdConsole.Instance.Output("Invalid arguments");
        SdtdConsole.Instance.Output(GetHelp());
      }
      else
      {
        string str = _params[1];
        string steamId = "";
        ClientInfo clientInfo = new ClientInfo();
        int count = ConsoleHelper.ParseParamPartialNameOrId(_params[0], out steamId, out clientInfo);
        if (count != 1)
        {
          if (count > 1)
          {
            SdtdConsole.Instance.Output("Multiple matches found: " + count);
          }
          else
          {
            SdtdConsole.Instance.Output("Playername or entity ID not found.");
          }
        }
        else
        {
          if (_senderInfo.IsLocalGame)
          {
            SdtdConsole.Instance.Output("Use the \"removequest\" command for the local player.");
          }
          else if (clientInfo.playerId != null)
          {
            clientInfo.SendPackage(new NetPackageConsoleCmdClient("removequest " + str, true));
          }
        }
      }
    }
  }
}
