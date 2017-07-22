using System;

namespace BCM.Neurons
{
  public class TrashCollector : NeuronAbstract
  {
    public TrashCollector()
    {
    }
    public override bool Fire(int b)
    {
      GC.Collect();
      GC.WaitForPendingFinalizers();
      //Log.Out(Config.ModPrefix + " Trash Disposed");
      return true;
    }
  }
}
