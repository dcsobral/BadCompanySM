using System;

namespace BCM.Neurons
{
  public class Motd : NeuronAbstract
  {
    public Motd()
    {
    }
    public override bool Fire(int b)
    {
      //preiodic messages sent to chat from server



      Log.Out(Config.ModPrefix + " Motd");
      return true;
    }
  }
}
