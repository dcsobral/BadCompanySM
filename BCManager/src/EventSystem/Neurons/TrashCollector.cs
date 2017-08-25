using static System.GC;

namespace BCM.Neurons
{
  public class TrashCollector : NeuronAbstract
  {
    public override void Fire(int b)
    {
      Collect();
      WaitForPendingFinalizers();

      //Log.Out(Config.ModPrefix + " Trash Disposed");
    }
  }
}
