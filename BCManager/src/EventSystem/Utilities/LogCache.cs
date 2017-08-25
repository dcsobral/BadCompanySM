using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BCM
{
  public class LogCache
  {
    //private const int MAX_ENTRIES = 3000;
    private static LogCache _instance;
    public static LogCache Instance => _instance ?? (_instance = new LogCache());

    private static readonly Regex LogRegex = new Regex(@"^([0-9]{4}-[0-9]{2}-[0-9]{2})T([0-9]{2}:[0-9]{2}:[0-9]{2}) ([0-9]+[,.][0-9]+) [A-Z]+ (.*)$");
    public List<LogEntry> LogEntries = new List<LogEntry>();
    private int _listOffset = 0;

    public int OldestLine
    {
      get
      {
        lock (LogEntries)
        {
          return _listOffset;
        }
      }
    }

    public int LatestLine
    {
      get
      {
        lock (LogEntries)
        {
          return _listOffset + LogEntries.Count - 1;
        }
      }
    }

    public int StoredLines
    {
      get
      {
        lock (LogEntries)
        {
          return LogEntries.Count;
        }
      }
    }

    public LogEntry this[int index]
    {
      get
      {
        lock (LogEntries)
        {
          if (index >= _listOffset && index < _listOffset + LogEntries.Count)
          {
            return LogEntries[index];
          }
        }
        return null;
      }
    }

    private LogCache()
    {
      Logger.Main.LogCallbacks += LogCallback;
    }

    private void LogCallback(string msg, string trace, LogType type)
    {
      LogEntry le = new LogEntry();

      Match match = LogRegex.Match(msg);
      if (match.Success)
      {
        le.Date = match.Groups[1].Value;
        le.Time = match.Groups[2].Value;
        le.Uptime = match.Groups[3].Value;
        le.Message = match.Groups[4].Value;
      }
      else
      {
        DateTime dt = DateTime.UtcNow;
        le.Date = string.Format("{0:0000}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day);
        le.Time = string.Format("{0:00}:{1:00}:{2:00}", dt.Hour, dt.Minute, dt.Second);
        le.Uptime = "";
        le.Message = msg;
      }

      le.Trace = trace;
      le.Type = type;

      lock (LogEntries)
      {
        LogEntries.Add(le);
        //if (logEntries.Count > MAX_ENTRIES)
        //{
        //  listOffset += logEntries.Count - MAX_ENTRIES;
        //  logEntries.RemoveRange(0, logEntries.Count - MAX_ENTRIES);
        //}
      }
    }
    public List<LogEntry> GetRange()
    {
      lock(LogEntries){
        return LogEntries;
      }
    }

    public List<LogEntry> GetRange(ref int start, int count, out int end)
    {
      lock (LogEntries)
      {
        if (count < 1)
        {
          end = start;
          return new List<LogEntry>();
        }

        if (start < _listOffset)
        {
          start = _listOffset;
        }

        if (start >= _listOffset + LogEntries.Count)
        {
          end = start;
          return new List<LogEntry>();
        }

        int index = start - _listOffset;

        if (index + count > LogEntries.Count)
        {
          count = LogEntries.Count - index;
        }

        end = start + count;

        return LogEntries.GetRange(index, count);
      }
    }


    public class LogEntry
    {
      public string Date;
      public string Time;
      public string Uptime;
      public string Message;
      public string Trace;
      public LogType Type;
    }
  }
}

