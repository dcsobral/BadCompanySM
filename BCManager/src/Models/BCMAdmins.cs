using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMAdmins
  {
    public List<BCMAdmin> Admins = new List<BCMAdmin>();
    public List<BCMBan> Bans = new List<BCMBan>();
    public List<BCMWhitelist> Whitelist = new List<BCMWhitelist>();
    public List<BCMPermission> Permissions = new List<BCMPermission>();

    public BCMAdmins()
    {
      //ADMIN
      for (var i = 0; i < GameManager.Instance.adminTools.GetAdmins().Count; i++)
      {
        var atci = GameManager.Instance.adminTools.GetAdmins()[i];
        var ci = SingletonMonoBehaviour<ConnectionManager>.Instance.GetClientInfoForPlayerId(atci.SteamID);
        Admins.Add(new BCMAdmin(atci, ci));
      }

      //BANS
      for (var i = 0; i < GameManager.Instance.adminTools.GetBanned().Count; i++)
      {
        var atci = GameManager.Instance.adminTools.GetBanned()[i];
        Bans.Add(new BCMBan(atci));
      }

      //WHITELIST
      for (var i = 0; i < GameManager.Instance.adminTools.GetWhitelisted().Count; i++)
      {
        var atci = GameManager.Instance.adminTools.GetWhitelisted()[i];
        var ci = ConnectionManager.Instance.GetClientInfoForPlayerId(atci.SteamID);
        Whitelist.Add(new BCMWhitelist(atci, ci));
      }

      //COMMAND PERMISSIONS
      for (var i = 0; i < GameManager.Instance.adminTools.GetCommands().Count; i++)
      {
        var atcp = GameManager.Instance.adminTools.GetCommands()[i];
        Permissions.Add(new BCMPermission(atcp));
      }
    }
  }
}