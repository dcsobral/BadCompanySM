using System;

namespace BCM.Models
{
  public class BCMTask
  {
    public int Hash;
    public string Type;
    public DateTime Timestamp;
    public DateTime Completion;
    public TimeSpan Duration;
    public object Output;
    public BCCmd Command;
    public BCMTaskStatus Status = BCMTaskStatus.InProgress;
  }
}