using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCRemoveBuffFromPlayer : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld()) return;

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

        if (MultiBuffClass.s_classes.ContainsKey(Params[1]))
        {
          clientInfo.SendPackage(new NetPackageConsoleCmdClient("debuff " + Params[1], true));
          SendOutput($"Buff {Params[1]} removed from player {clientInfo.playerName}");
        }
        else
        {
          SendOutput($"Unable to find buff {Params[1]}");
        }
      }
      else if (count > 1)
      {
        SendOutput($"{count} matches found, please refine your search text.");
      }
      else
      {
        SendOutput("Playername or entity ID not found.");
      }
    }
  }
}
