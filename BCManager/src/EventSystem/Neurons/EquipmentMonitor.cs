using System;

namespace BCM.Neurons
{
  public class EquipmentMonitor : NeuronAbstract
  {
    public EquipmentMonitor()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement

      Log.Out(Config.ModPrefix + " EquipmentMonitor");
      return true;
    }
  }
}
