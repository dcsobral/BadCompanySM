namespace BCM.Commands
{
  public class SetSkillOnPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count < 2)
      {
        SendOutput("Incorrect command format");
        SendOutput(GetHelp());
        return;
      }

      ClientInfo clientInfo;
      int offset = 0;
      clientInfo = GetClientInfo(ref offset);

      string skillName = GetSkillName(offset);

      if (clientInfo.entityId > 0 && skillName != "")
      {
        //todo: modify player data file instead if they are offline
        EntityPlayer entity = GameManager.Instance.World.Entities.dict[clientInfo.entityId] as EntityPlayer;

        if (entity == null)
        {
          SendOutput("Unable to find player entity");
        }
        else
        {
          if (!SetSkillLevel(clientInfo, skillName, entity)) { return; }
        }
      }
    }

    private ClientInfo GetClientInfo(ref int offset)
    {
      ClientInfo clientInfo;
      if (_options.ContainsKey("self") && _senderInfo.RemoteClientInfo != null && _senderInfo.RemoteClientInfo.entityId > 0)
      {
        clientInfo = _senderInfo.RemoteClientInfo;
      }
      else
      {
        offset = 1;

        string steamId = "";
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
      }

      return clientInfo;
    }

    private string GetSkillName(int offset = 0)
    {
      string skillName = _params[offset];
      for (int i = offset + 1; i <= _params.Count - 2; i++)
      {
        skillName = skillName + " " + _params[i];
      }
      return skillName;
    }

    private bool SetSkillLevel(ClientInfo clientInfo, string skillName, EntityPlayer entity)
    {
      Skill skillByName = entity.Skills.GetSkillByName(skillName);
      if (skillByName == null)
      {
        SendOutput("skill " + skillName + " not found");
        return false;
      }
      int level;
      if (!int.TryParse(_params[_params.Count - 1], out level))
      {
        SendOutput("Level must be a number");
        return false;
      }
      else
      {
        if (level <= 0)
        {
          SendOutput("Level must be 0+");
          return false;
        }
        //todo: check constraints
        SendOutput("Setting player '" + clientInfo.playerName + "' skill '" + skillName + "' to level: " + level);
        clientInfo.SendPackage(new NetPackageEntitySetSkillLevelClient(clientInfo.entityId, skillByName.Id, level));


        //GameManager.ShowTooltipWithAlert(this as EntityPlayerLocal, string.Format(Localization.Get("ttLevelUp", string.Empty), level2.ToString(), this.SkillPoints), "levelupplayer");

      }
      return true;
    }

  }
}
