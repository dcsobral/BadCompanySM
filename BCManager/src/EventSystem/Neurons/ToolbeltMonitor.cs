namespace BCM.Neurons
{
  public class ToolbeltMonitor : NeuronAbstract
  {
    public ToolbeltMonitor(Synapse s) : base(s)
    {
    }
    public override void Fire(int b)
    {
      // todo: implement


      Log.Out(Config.ModPrefix + " ToolbeltMonitor");
    }
  }
}
