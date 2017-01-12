using System.Threading;

namespace BCM
{
  public static class Heartbeat
  {
    public static bool IsAlive = false;
    public static int BPM = 60;
    public static int beats = 0;

    public static void Start()
    {
      ThreadManager.StartThread(new ThreadManager.ThreadFunctionDelegate(HeartbeatPulse), ThreadPriority.Lowest, null, null);
      Log.Out(Config.ModPrefix + " Its Alive!!! (Pulse Started)");
    }
    private static void HeartbeatPulse(ThreadManager.ThreadInfo ti)
    {
      while (IsAlive)
      {
        beats++;
        Brain.FireNeurons(beats);
        Thread.Sleep(1000 * 60 / BPM);
      }
      Terminate();
    }
    public static void Terminate()
    {
      Log.Out(Config.ModPrefix + " It's Dead Jim! (Pulse Ended)");
    }
  }
}
