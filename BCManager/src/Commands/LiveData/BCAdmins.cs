using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Commands
{
  public class BCAdmins : BCCommandAbstract
  {
    public class BCMAdmin
    {
      public string SteamId;
      public int PermissionLevel;
      public string PlayerName;

      public BCMAdmin(AdminToolsClientInfo atci, ClientInfo ci)
      {
        SteamId = atci.SteamID;
        PermissionLevel = atci.PermissionLevel;
        PlayerName = ci != null ? ci.playerName : "";
      }
    }

    public class BCMBan
    {
      public string SteamId;
      public string BannedUntil;
      public string BanReason;

      public BCMBan(AdminToolsClientInfo atci)
      {
        SteamId = atci.SteamID;
        BannedUntil = atci.BannedUntil.ToCultureInvariantString();
        BanReason = atci.BanReason;
      }
    }

    public class BCMWhitelist
    {
      public string SteamId;
      public string PlayerName;

      public BCMWhitelist(AdminToolsClientInfo atci, ClientInfo ci)
      {
        SteamId = atci.SteamID;
        PlayerName = ci != null ? ci.playerName : "";
      }
    }

    public class BCMPermission
    {
      public string Command;
      public int PermissionLevel;

      public BCMPermission(AdminToolsCommandPermissions atcp)
      {
        Command = atcp.Command;
        PermissionLevel = atcp.PermissionLevel;
      }
    }

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

      public object Data()
      {
        var data = new Dictionary<string, object>();
        if (Options.ContainsKey("admins") || !(Options.ContainsKey("bans") || Options.ContainsKey("whitelist") || Options.ContainsKey("permissions")))
        {
          data.Add("Admins", Admins);
        }
        if (Options.ContainsKey("bans") || !(Options.ContainsKey("admins") || Options.ContainsKey("whitelist") || Options.ContainsKey("permissions")))
        {
          data.Add("Bans", Bans);
        }
        if (Options.ContainsKey("whitelist") || !(Options.ContainsKey("admins") || Options.ContainsKey("bans") || Options.ContainsKey("permissions")))
        {
          data.Add("Whitelist", Whitelist);
        }
        if (Options.ContainsKey("permissions") || !(Options.ContainsKey("admins") || Options.ContainsKey("bans") || Options.ContainsKey("whitelist")))
        {
          data.Add("Permissions", Permissions);
        }
        
        return data;
      }
    }

    public override void Process()
    {
      if (Options.ContainsKey("gg"))
      {
        SendJson(GetGamePrefs());
      }
      else if (Options.ContainsKey("ggs"))
      {
        SendJson(GetGameStats());
      }
      else
      {
        SendJson(new BCMAdmins().Data());
      }

    }

    private SortedList<string, object> GetGamePrefs()
    {
      var sortedList = new SortedList<string, object>();

      var enumerator = Enum.GetValues(typeof(EnumGamePrefs)).GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current == null) continue;

        var enumGamePrefs = (EnumGamePrefs)(int)enumerator.Current;
        if (!IsViewablePref(enumGamePrefs)) continue;

        try
        {
          sortedList.Add(enumGamePrefs.ToString(), GamePrefs.GetObject(enumGamePrefs));
        }
        catch (Exception)
        {
          //Log.Out("Exception getting object for " + enumGamePrefs);
        }
      }
      (enumerator as IDisposable)?.Dispose();

      return sortedList;
    }

    private readonly string[] _filterPrefs =
    {
      //"telnet",
      "adminfilename",
      "controlpanel",
      "password",
      "savegamefolder",
      "options",
      "last"
    };

    private bool IsViewablePref(EnumGamePrefs pref)
    {
      return _filterPrefs.All(value => !pref.ToString().ToLower().Contains(value));
    }

    private SortedList<string, object> GetGameStats()
    {
      var sortedList = new SortedList<string, object>();

      var enumerator = Enum.GetValues(typeof(EnumGameStats)).GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current == null) continue;

        var enumGameStats = (EnumGameStats)(int)enumerator.Current;

        if (!IsViewableStat(enumGameStats)) continue;

        var stat = GameStats.GetObject(enumGameStats);
        if (int.TryParse($"{stat}", out int stati))
        {
          sortedList.Add(enumGameStats.ToString(), stati);
        }
        else if (bool.TryParse($"{stat}", out bool statb))
        {
          sortedList.Add(enumGameStats.ToString(), statb);
        }
        else if (double.TryParse($"{stat}", out double statd))
        {
          sortedList.Add(enumGameStats.ToString(), statd);
        }
        else
        {
          sortedList.Add(enumGameStats.ToString(), string.IsNullOrEmpty($"{stat}") ? null : $"{stat}");
        }
      }

      return sortedList;
    }

    private readonly string[] _filterStats =
    {
      "last"
    };

    private bool IsViewableStat(EnumGameStats stat)
    {
      return _filterStats.All(value => !stat.ToString().ToLower().Contains(value));
    }
  }
}