using System;

namespace BCM.Neurons
{
  public class PingKicker : NeuronAbstract
  {
    public PingKicker()
    {
    }
    public override bool Fire(int b)
    {
      if (!Steam.Network.IsServer)
      {
        Log.Out(Config.ModPrefix + "Can't kick players for high ping. Not a network server");
        return false;
      }

      //todo: check player pings vs limit and whilelists, kick any that fail test

      return true;
    }
  }
}
