using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BCM
{
  public class LogEntry
  {
    public string Date;
    public string Time;
    public string Uptime;
    public string Message;
    public string Trace;
    public LogType Type;
  }

  public class LogCache : NeuronAbstract
  {
    private static readonly Regex LogRegex = new Regex(@"^([0-9]{4}-[0-9]{2}-[0-9]{2})T([0-9]{2}:[0-9]{2}:[0-9]{2}) ([0-9]+[,.][0-9]+) [A-Z]+ (.*)$");
    public List<LogEntry> LogEntries = new List<LogEntry>();

    public LogCache(Synapse s) : base(s) => Logger.Main.LogCallbacks += LogCallback;

    public override void Awake()
    {
      if (synapse.IsEnabled)
      {
        Log.Out($"{Config.ModPrefix} LogCache Initialised");
      }
    }

    private void LogCallback(string msg, string trace, LogType type)
    {
      var entry = new LogEntry();

      var match = LogRegex.Match(msg);
      if (match.Success)
      {
        entry.Date = match.Groups[1].Value;
        entry.Time = match.Groups[2].Value;
        entry.Uptime = match.Groups[3].Value;
        entry.Message = match.Groups[4].Value;
      }
      else
      {
        var dt = DateTime.UtcNow;
        entry.Date = $"{dt.Year:0000}-{dt.Month:00}-{dt.Day:00}";
        entry.Time = $"{dt.Hour:00}:{dt.Minute:00}:{dt.Second:00}";
        entry.Uptime = "";
        entry.Message = msg;
      }

      entry.Trace = trace;
      entry.Type = type;

      lock (LogEntries) LogEntries.Add(entry);
    }

    public LogEntry this[int index]
    {
      get
      {
        lock (LogEntries)
          if (index >= 0 && index < LogEntries.Count)
            return LogEntries[index];
        return null;
      }
    }

    public List<LogEntry> GetAllEntries()
    {
      lock (LogEntries)
        return LogEntries;
    }

    public List<LogEntry> GetErrorEntries()
    {
      lock (LogEntries)
        return LogEntries.Where(e=> e.Type == LogType.Error).ToList();
    }

    public List<LogEntry> GetGmsgEntries()
    {
      lock (LogEntries)
        return LogEntries.Where(e => e.Type == LogType.Log && e.Message.StartsWith("GMSG:")).ToList();
    }

    public List<LogEntry> GetBcmEntries()
    {
      lock (LogEntries)
        return LogEntries.Where(e => e.Type == LogType.Log && e.Message.StartsWith(Config.ModPrefix)).ToList();
    }

    public List<LogEntry> GetChatEntries()
    {
      lock (LogEntries)
        return LogEntries.Where(e => e.Type == LogType.Log && e.Message.StartsWith("Chat:")).ToList();
    }

    public override void Fire(int b) { }
  }
}
