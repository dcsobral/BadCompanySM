namespace BCM.Commands
{
  public class BCRemoveQuestFromPlayer : BCCommandAbstract
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
        if (clientInfo == null) return;

        clientInfo.SendPackage(new NetPackageConsoleCmdClient($"removequest {Params[1]}", true));
      }
      else if (count > 1)
      {
        SendOutput($"{count} matches found, please refine your search text");
      }
      else
      {
        SendOutput("Playername or entity ID not found.");
      }
    }
  }
}
