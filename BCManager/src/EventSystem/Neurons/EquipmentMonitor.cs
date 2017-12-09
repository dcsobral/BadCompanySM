namespace BCM.Neurons
{
  public class EquipmentMonitor : NeuronAbstract
  {
    public EquipmentMonitor(Synapse s) : base(s)
    {
    }
    public override void Fire(int b)
    {
      // todo: implement

      Log.Out(Config.ModPrefix + " EquipmentMonitor");
    }
  }
}
