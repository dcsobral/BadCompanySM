using System.Threading;

namespace BCM
{
  public static class Heartbeat
  {
    public static bool IsAlive = false;
    public static int Bpm = 60;
    public static int Beats;

    public static void Start()
    {
      ThreadManager.StartThread(HeartbeatPulse, ThreadPriority.Lowest);
      Log.Out(Config.ModPrefix + " Its Alive!!! (Pulse Started)");
    }
    private static void HeartbeatPulse(ThreadManager.ThreadInfo ti)
    {
      while (IsAlive)
      {
        Beats++;
        Brain.FireNeurons(Beats);
        Thread.Sleep(1000 * 60 / Bpm);
      }
      Terminate();
    }
    public static void Terminate()
    {
      Log.Out(Config.ModPrefix + " It's Dead Jim! (Pulse Ended)");
    }
  }
}
