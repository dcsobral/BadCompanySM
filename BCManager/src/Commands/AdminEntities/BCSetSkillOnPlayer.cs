using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCSetSkillOnPlayer : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      if (Params.Count < 2)
      {
        SendOutput("Incorrect command format");
        SendOutput(GetHelp());

        return;
      }

      var offset = 0;
      var clientInfo = GetClientInfo(ref offset, out var steamId);

      var skillName = GetSkillName(offset);
      if (string.IsNullOrEmpty(skillName))
      {
        SendOutput("Invalid skill name");

        return;
      }

      if (clientInfo == null)
      {
        if (steamId == null)
        {
          SendOutput("Unable to find player");
        }
        else
        {
          SetSkillLevelOffline(steamId, skillName);
        }

        return;
      }

      var entity = world.Entities.dict[clientInfo.entityId] as EntityPlayer;
      if (entity == null)
      {
        SendOutput("Unable to find player entity");

        return;
      }

      SetSkillLevel(clientInfo, skillName, entity);
    }

    private static ClientInfo GetClientInfo(ref int offset, out string steamId)
    {
      steamId = null;
      ClientInfo clientInfo;
      if (Options.ContainsKey("self") && SenderInfo.RemoteClientInfo != null && SenderInfo.RemoteClientInfo.entityId > 0)
      {
        clientInfo = SenderInfo.RemoteClientInfo;
      }
      else
      {
        offset = 1;
        var count = ConsoleHelper.ParseParamPartialNameOrId(Params[0], out steamId, out clientInfo);

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

      if (!int.TryParse(Params[Params.Count - 1], out var level))
      {
        SendOutput("Level must be a number");

        return;
      }

      if (!Skills.AllSkills.ContainsKey(skillByName.Id)) return;

      var skill = Skills.AllSkills[skillByName.Id];
      if (level < skill.Level)
      {
        SendOutput("Level is below the mininmum level of " + skill.Level + " for " + skillName);

        return;
      }

      if (level > skill.MaxLevel)
      {
        SendOutput("Level is higher than the maximum level of " + skill.MaxLevel + " for " + skillName);

        return;
      }

      //todo: check constraints
      clientInfo.SendPackage(new NetPackageEntitySetSkillLevelClient(clientInfo.entityId, skillByName.Id, level));
      SendOutput($@"Setting player '{clientInfo.playerName}' skill '{skillName}' to level: {level}");
    }

    private static void SetSkillLevelOffline(string steamId, string skillName)
    {
      if (!int.TryParse(Params[Params.Count - 1], out var level))
      {
        SendOutput("Level must be a number");

        return;
      }

      var pdf = new PlayerDataFile();
      pdf.Load(GameUtils.GetPlayerDataDir(), steamId);
      //todo: make backup

      var fieldInfo = typeof(PlayerDataFile).GetField("HH", BindingFlags.NonPublic | BindingFlags.Instance);
      if (fieldInfo == null) return;

      if (!(fieldInfo.GetValue(pdf) is MemoryStream curSkillStream)) return;

      var skills = new Skills();
      if (curSkillStream.Length > 0L)
      {
        skills.Read(new BinaryReader(curSkillStream), 36u);
      }

      var skillOnPlayer = skills.GetSkillByName(skillName); 
      if (skillOnPlayer == null)
      {
        SendOutput($"Skill {skillName} not found");

        return;
      }

      if (!Skills.AllSkills.ContainsKey(skillOnPlayer.Id)) return;

      var skillFromXml = Skills.AllSkills[skillOnPlayer.Id];
      if (level < skillFromXml.Level)
      {
        SendOutput("Level is below the mininmum level of " + skillFromXml.Level + " for " + skillName);

        return;
      }

      if (level > skillFromXml.MaxLevel)
      {
        SendOutput("Level is higher than the maximum level of " + skillFromXml.MaxLevel + " for " + skillName);

        return;
      }

      //todo: check requirements
      if (skillOnPlayer.IsLocked)
      {
        skillOnPlayer.IsLocked = false;
      }

      if (skillFromXml.IsPerk)
      {
        var fieldInfo2 = typeof(Skill).GetField("expToNextLevel", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo2 != null)
        {
          fieldInfo2.SetValue(skillOnPlayer, 0);
        }
        else
        {
          SendOutput("Unable to set skill 'expToNextLevel' for perk");
        }
      }
      skillOnPlayer.Level = level;
      var newSkillStream = new MemoryStream();
      skills.Write(new BinaryWriter(newSkillStream));
      fieldInfo.SetValue(pdf, newSkillStream);
      pdf.Save(GameUtils.GetPlayerDataDir(), steamId);
      SendOutput($"Setting player '{steamId}' skill '{skillName}' to level: {level}");
    }
  }
}
