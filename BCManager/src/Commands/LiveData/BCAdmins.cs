using System;
using System.Collections.Generic;
using System.Linq;
using BCM.Models;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCAdmins : BCCommandAbstract
  {
    protected override void Process()
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
        SendJson(GenerateData(new BCMAdmins()));
      }
    }

    private static object GenerateData(BCMAdmins adminData)
    {
      var data = new Dictionary<string, object>();
      if (Options.ContainsKey("admins") || !(Options.ContainsKey("bans") || Options.ContainsKey("whitelist") || Options.ContainsKey("permissions")))
      {
        data.Add("Admins", adminData.Admins);
      }
      if (Options.ContainsKey("bans") || !(Options.ContainsKey("admins") || Options.ContainsKey("whitelist") || Options.ContainsKey("permissions")))
      {
        data.Add("Bans", adminData.Bans);
      }
      if (Options.ContainsKey("whitelist") || !(Options.ContainsKey("admins") || Options.ContainsKey("bans") || Options.ContainsKey("permissions")))
      {
        data.Add("Whitelist", adminData.Whitelist);
      }
      if (Options.ContainsKey("permissions") || !(Options.ContainsKey("admins") || Options.ContainsKey("bans") || Options.ContainsKey("whitelist")))
      {
        data.Add("Permissions", adminData.Permissions);
      }

      return data;
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

      sortedList.Add("ServerHostIP", BCUtils.GetIPAddress());

      return sortedList;
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
        if (int.TryParse($"{stat}", out var stati))
        {
          sortedList.Add(enumGameStats.ToString(), stati);
        }
        else if (bool.TryParse($"{stat}", out var statb))
        {
          sortedList.Add(enumGameStats.ToString(), statb);
        }
        else if (double.TryParse($"{stat}", out var statd))
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

    private readonly string[] _filterStats =
    {
      "last"
    };

    private bool IsViewablePref(EnumGamePrefs pref) => _filterPrefs.All(value => !pref.ToString().ToLower().Contains(value));

    private bool IsViewableStat(EnumGameStats stat) => _filterStats.All(value => !stat.ToString().ToLower().Contains(value));
  }
}