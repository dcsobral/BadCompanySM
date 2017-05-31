using System;

namespace BCM.Neurons
{
  public class ToolbeltMonitor : NeuronAbstract
  {
    public ToolbeltMonitor()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement


      Log.Out(Config.ModPrefix + " ToolbeltMonitor");
      return true;
    }
  }
}
