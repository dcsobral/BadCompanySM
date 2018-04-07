using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCTime : BCCommandAbstract
  {
    protected override void Process()
    {
      var time = new BCMTime(Options);

      if (Options.ContainsKey("t"))
      {
        SendJson(time.Time);
      }
      else if (Options.ContainsKey("s"))
      {
        SendJson(new[] { time.Time["D"], time.Time["H"], time.Time["M"] });
      }
      else
      {
        SendJson(time);
      }
    }
  }
}
