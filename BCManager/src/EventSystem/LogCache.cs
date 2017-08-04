using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BCM
{
  public class LogCache
  {
    //private const int MAX_ENTRIES = 3000;
    private static LogCache instance;

    public static LogCache Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new LogCache();
        }
        return instance;
      }
    }

    private static Regex logMessageMatcher = new Regex(@"^([0-9]{4}-[0-9]{2}-[0-9]{2})T([0-9]{2}:[0-9]{2}:[0-9]{2}) ([0-9]+[,.][0-9]+) [A-Z]+ (.*)$");
    private List<LogEntry> logEntries = new List<LogEntry>();
    private int listOffset = 0;

    public int OldestLine
    {
      get
      {
        lock (logEntries)
        {
          return listOffset;
        }
      }
    }

    public int LatestLine
    {
      get
      {
        lock (logEntries)
        {
          return listOffset + logEntries.Count - 1;
        }
      }
    }

    public int StoredLines
    {
      get
      {
        lock (logEntries)
        {
          return logEntries.Count;
        }
      }
    }

    public LogEntry this[int index]
    {
      get
      {
        lock (logEntries)
        {
          if (index >= listOffset && index < listOffset + logEntries.Count)
          {
            return logEntries[index];
          }
        }
        return null;
      }
    }

    private LogCache()
    {
      Logger.Main.LogCallbacks += LogCallback;
    }

    private void LogCallback(string _msg, string _trace, LogType _type)
    {
      LogEntry le = new LogEntry();

      Match match = logMessageMatcher.Match(_msg);
      if (match.Success)
      {
        le.date = match.Groups[1].Value;
        le.time = match.Groups[2].Value;
        le.uptime = match.Groups[3].Value;
        le.message = match.Groups[4].Value;
      }
      else
      {
        DateTime dt = DateTime.Now;
        le.date = string.Format("{0:0000}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day);
        le.time = string.Format("{0:00}:{1:00}:{2:00}", dt.Hour, dt.Minute, dt.Second);
        le.uptime = "";
        le.message = _msg;
      }

      le.trace = _trace;
      le.type = _type;

      lock (logEntries)
      {
        logEntries.Add(le);
        //if (logEntries.Count > MAX_ENTRIES)
        //{
        //  listOffset += logEntries.Count - MAX_ENTRIES;
        //  logEntries.RemoveRange(0, logEntries.Count - MAX_ENTRIES);
        //}
      }
    }
    public List<LogEntry> GetRange()
    {
      lock(logEntries){
        //confirm: needs lock?
        return logEntries;
      }
    }

    public List<LogEntry> GetRange(ref int _start, int _count, out int _end)
    {
      lock (logEntries)
      {
        if (_count < 1)
        {
          _end = _start;
          return new List<LogEntry>();
        }

        if (_start < listOffset)
        {
          _start = listOffset;
        }

        if (_start >= listOffset + logEntries.Count)
        {
          _end = _start;
          return new List<LogEntry>();
        }

        int index = _start - listOffset;

        if (index + _count > logEntries.Count)
        {
          _count = logEntries.Count - index;
        }

        _end = _start + _count;

        return logEntries.GetRange(index, _count);
      }
    }


    public class LogEntry
    {
      public string date;
      public string time;
      public string uptime;
      public string message;
      public string trace;
      public LogType type;
    }
  }
}

