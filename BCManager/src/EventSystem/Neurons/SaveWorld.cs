using System;

namespace BCM.Neurons
{
  public class SaveWorld : NeuronAbstract
  {
    public SaveWorld()
    {
    }
    public override bool Fire(int b)
    {
      if (!Steam.Network.IsServer)
      {
        Log.Out(Config.ModPrefix + "World save failed. Not a network server");
        return false;
      }
      GameManager.Instance.SaveLocalPlayerData();
      GameManager.Instance.SaveWorld();

      Log.Out(Config.ModPrefix + " World saved");
      return true;
    }
  }
}
