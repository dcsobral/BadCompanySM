using System;

namespace BCM.Neurons
{
  public class PositionTracker : NeuronAbstract
  {
    public PositionTracker()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement


      Log.Out(Config.ModPrefix + " PositionTracker");
      return true;
    }
  }
}
