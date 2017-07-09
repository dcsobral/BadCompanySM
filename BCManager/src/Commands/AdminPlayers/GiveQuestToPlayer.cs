namespace BCM.Commands
{
  public class GiveQuestToPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count != 2)
      {
        SendOutput("Invalid arguments");
        SendOutput(GetHelp());
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
            SendOutput("Multiple matches found: " + count);
          }
          else
          {
            SendOutput("Playername or entity ID not found.");
          }
        }
        else
        {
          if (_senderInfo.IsLocalGame)
          {
            SendOutput("Use the \"givequest\" command for the local player.");
          }
          else if (clientInfo.playerId != null)
          {
            clientInfo.SendPackage(new NetPackageConsoleCmdClient("givequest " + str, true));
          }
        }
      }
    }
  }
}
