namespace BCM.Commands
{
  public class BCGiveBuffToPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (SenderInfo.IsLocalGame)
      {
        SendOutput(@"Use the ""buff"" command for the local player.");

        return;
      }

      if (Params.Count != 2)
      {
        SendOutput("Invalid arguments");
        SendOutput(GetHelp());

        return;
      }

      var count = ConsoleHelper.ParseParamPartialNameOrId(Params[0], out string _, out ClientInfo clientInfo);
      if (count == 1)
      {
        if (clientInfo == null)
        {
          SendOutput("Unable to locate player.");

          return;
        }
        //todo: check buff is valid
        clientInfo.SendPackage(new NetPackageConsoleCmdClient("buff " + Params[1], true));
      }
      else if (count > 1)
      {
        SendOutput("Multiple matches found: " + count);
      }
      else
      {
        SendOutput("Playername or entity ID not found.");
      }
    }
  }
}
