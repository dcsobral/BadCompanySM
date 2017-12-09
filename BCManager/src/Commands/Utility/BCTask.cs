using System;
using System.Collections.Generic;
using System.Threading;
using BCM.Models;

namespace BCM.Commands
{
  public class BCTask : BCCommandAbstract
  {
    public enum Status
    {
      InProgress,
      Complete
    }

    public class BCMTask
    {
      public int Hash;
      public string Type;
      public DateTime Timestamp;
      public DateTime Completion;
      public TimeSpan Duration;
      public object Output;
      public BCCmd Command;
      public Status Status = Status.InProgress;
    }

    private static readonly Dictionary<string, BCMTask> Tasks = new Dictionary<string, BCMTask>();

    public override void Process()
    {
      if (Tasks.Count == 0)
      {
        SendOutput("No tracked tasks currently running");

        return;
      }

      foreach (var task in Tasks.Values)
      {
        SendOutput(string.Join(" - ", new[]
        {
          $"{task.Type}_{task.Hash}",
          task.Timestamp.ToUtcStr(),
          task.Completion.ToUtcStr(),
          $"{task.Duration.TotalSeconds}s",
          task.Status.ToString(),
          task.Command == null ? "" : task.Command.Command
        }));
        SendJson(new { task.Output });
      }
    }

    public static void AddTask(string taskType, int hash, BCCmd command)
    {
      if (Tasks.ContainsKey($"{taskType}_{hash}"))
      {
        Log.Out($"{Config.ModPrefix} Unable to add tracked task to list, already in tracked list");

        return;
      }

      Tasks.Add($"{taskType}_{hash}", new BCMTask
      {
        Type = taskType,
        Hash = hash,
        Output = null,
        Timestamp = DateTime.UtcNow,
        Command = command
      });
    }

    public static void DelTask(string taskType, int hash, int delay = 60)
    {
      ThreadManager.AddSingleTask(info =>
        {
          if (!Tasks.ContainsKey($"{taskType}_{hash}")) return;

          var bcmTask = Tasks[$"{taskType}_{hash}"];

          bcmTask.Status = Status.Complete;
          bcmTask.Completion = DateTime.UtcNow;
          bcmTask.Duration = bcmTask.Completion - bcmTask.Timestamp;

          Thread.Sleep(delay * 1000);
          if (!Tasks.ContainsKey($"{taskType}_{hash}")) return;

          Tasks.Remove($"{taskType}_{hash}");
          SendOutput($"Task complete {hash}");
        }
      );
    }

    public static BCMTask GetTask(string taskType, int? hash)
    {
      if (hash == null || !Tasks.ContainsKey($"{taskType}_{hash}")) return null;

      return Tasks[$"{taskType}_{hash}"];
    }
  }
}
