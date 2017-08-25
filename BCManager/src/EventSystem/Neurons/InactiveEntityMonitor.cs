namespace BCM.Neurons
{
  public class InactiveEntityMonitor : NeuronAbstract
  {
    public InactiveEntityMonitor()
    {
    }
    public override void Fire(int b)
    {
      //scan entities and if they are observers either kill them or teleport them closer to nearest player
      //scan falling blocks and if count becomes too high and server fps drops then remove them
      //do zombie cleanup before horde night

      Log.Out(Config.ModPrefix + " InactiveEntityMonitor");
    }
  }
}
