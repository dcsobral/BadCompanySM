using System.Threading;

namespace BCM
{
  public static class Heartbeat
  {
    private static Thread pulse;
    public static bool IsAlive = false;
    public static int BPM = 60;
    public static int beats = 0;

    public static void Start()
    {
      pulse = new Thread(new ThreadStart(Revive));
      pulse.IsBackground = true;
      pulse.Start();
      Log.Out(Config.ModPrefix + " Its Alive!!! (Pulse Started)");
    }
    private static void Revive()
    {
      while (IsAlive)
      {
        //Log.Out(Config.ModPrefix + " " + beats + " " + IsAlive);
        beats++;
        Brain.FireNeurons(beats);
        Thread.Sleep(1000 * 60 / BPM);
      }
      Terminate();
    }
    public static void Terminate()
    {
      pulse.Abort();
      Log.Out(Config.ModPrefix + " It's Dead Jim! (Pulse Ended)");
    }
  }
}
