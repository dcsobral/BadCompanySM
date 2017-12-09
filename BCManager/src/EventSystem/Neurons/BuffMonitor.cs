namespace BCM.Neurons
{
  public class BuffMonitor : NeuronAbstract
  {
    public BuffMonitor(Synapse s) : base(s)
    {
    }
    public override void Fire(int b)
    {
      // todo: implement

      Log.Out(Config.ModPrefix + " BuffMonitor");
    }
  }
}
