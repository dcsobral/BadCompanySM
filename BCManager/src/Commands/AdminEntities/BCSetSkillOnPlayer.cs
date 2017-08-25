namespace BCM.Commands
{
  public class BCSetSkillOnPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (Params.Count < 2)
      {
        SendOutput("Incorrect command format");
        SendOutput(GetHelp());
        return;
      }

      var offset = 0;
      var clientInfo = GetClientInfo(ref offset);

      var skillName = GetSkillName(offset);

      if (clientInfo.entityId <= 0 || skillName == "") return;

      //todo: modify player data file instead if they are offline
      var entity = GameManager.Instance.World.Entities.dict[clientInfo.entityId] as EntityPlayer;

      if (entity == null)
      {
        SendOutput("Unable to find player entity");

        return;
      }

      SetSkillLevel(clientInfo, skillName, entity);
    }

    private static ClientInfo GetClientInfo(ref int offset)
    {
      ClientInfo clientInfo;
      if (Options.ContainsKey("self") && SenderInfo.RemoteClientInfo != null && SenderInfo.RemoteClientInfo.entityId > 0)
      {
        clientInfo = SenderInfo.RemoteClientInfo;
      }
      else
      {
        offset = 1;
        var count = ConsoleHelper.ParseParamPartialNameOrId(Params[0], out string _, out clientInfo);

        if (count > 1)
        {
          SendOutput("Multiple matches found: " + count);
        }

        if (count == 0)
        {
          SendOutput("Playername or entity ID not found.");
        }
      }

      return clientInfo;
    }

    private static string GetSkillName(int offset = 0)
    {
      var skillName = Params[offset];
      for (var i = offset + 1; i <= Params.Count - 2; i++)
      {
        skillName = skillName + " " + Params[i];
      }
      return skillName;
    }

    private static void SetSkillLevel(ClientInfo clientInfo, string skillName, EntityPlayer entity)
    {
      var skillByName = entity.Skills.GetSkillByName(skillName);
      if (skillByName == null)
      {
        SendOutput($"Skill {skillName} not found");

        return;
      }

      if (!int.TryParse(Params[Params.Count - 1], out int level))
      {
        SendOutput("Level must be a number");

        return;
      }

      if (level <= 0)
      {
        SendOutput("Level must be 0+");

        return;
      }

      //todo: check constraints
      clientInfo.SendPackage(new NetPackageEntitySetSkillLevelClient(clientInfo.entityId, skillByName.Id, level));
      SendOutput($@"Setting player '{clientInfo.playerName}' skill '{skillName}' to level: {level}");
      //GameManager.ShowTooltipWithAlert(this as EntityPlayerLocal, string.Format(Localization.Get("ttLevelUp", string.Empty), level2.ToString(), this.SkillPoints), "levelupplayer");
    }
  }
}
