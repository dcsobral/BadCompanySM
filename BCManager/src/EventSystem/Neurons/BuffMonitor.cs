using System;

namespace BCM.Neurons
{
  public class BuffMonitor : NeuronAbstract
  {
    public BuffMonitor()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement

      Log.Out(Config.ModPrefix + " BuffMonitor");
      return true;
    }
  }
}
