using System.Threading;

namespace BCM
{
  public static class Heartbeat
  {
    public static bool IsAlive = false;
    public static int Bpm = 60;
    private static int _beats;

    public static void Start()
    {
      ThreadManager.StartThread(HeartbeatPulse, ThreadPriority.Lowest);
      Log.Out($"{Config.ModPrefix} It\'s Alive!!! (Pulse Started)");
    }
    private static void HeartbeatPulse(ThreadManager.ThreadInfo ti)
    {
      while (IsAlive)
      {
        _beats++;
        Brain.FireNeurons(_beats);
        Thread.Sleep(1000 * 60 / Bpm);
      }
      Terminate();
    }

    private static void Terminate()
    {
      Log.Out($"{Config.ModPrefix} It\'s Dead Jim! (Pulse Ended)");
    }
  }
}
