namespace BCM.Commands
{
  public class BCRemoveQuestFromPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (Params.Count != 2)
      {
        SendOutput(GetHelp());

        return;
      }

      var count = ConsoleHelper.ParseParamPartialNameOrId(Params[0], out string _, out var clientInfo);
      if (count == 1)
      {
        if (clientInfo == null) return;

        if (QuestClass.s_Quests.ContainsKey(Params[1]))
        {
          clientInfo.SendPackage(new NetPackageConsoleCmdClient($"removequest {Params[1]}", true));
          SendOutput($"Quest {Params[1]} removed from player {clientInfo.playerName}");
        }
        else
        {
          SendOutput($"Unable to find quest {Params[1]}");
        }
      }
      else if (count > 1)
      {
        SendOutput($"{count} matches found, please refine your search text.");
      }
      else
      {
        SendOutput("Unable to find player.");
      }
    }
  }
}
