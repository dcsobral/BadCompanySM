namespace BCM.Commands
{
  public class BCGiveBuffToPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (Params.Count != 2)
      {
        SendOutput("Invalid arguments");
        SendOutput(GetHelp());

        return;
      }

      var count = ConsoleHelper.ParseParamPartialNameOrId(Params[0], out string _, out var clientInfo);
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
