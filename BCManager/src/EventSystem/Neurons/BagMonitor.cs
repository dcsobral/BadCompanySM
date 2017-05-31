using System;

namespace BCM.Neurons
{
  public class BagMonitor : NeuronAbstract
  {
    public BagMonitor()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement

      Log.Out(Config.ModPrefix + " BagMonitor");
      return true;
    }
  }
}
