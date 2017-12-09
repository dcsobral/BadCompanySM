namespace BCM.Neurons
{
  public class DeathWatch : NeuronAbstract
  {
    public DeathWatch(Synapse s) : base(s)
    {
    }
    public override void Fire(int b)
    {
      //watch the log for player deaths, and pvp kills
      //also attempt to match players kills to weapon held at the time the kill was awarded
      //and if possible match to nearby zombies that changed from live to dead at the same time

      Log.Out(Config.ModPrefix + " DeathWatch");
    }
  }
}
