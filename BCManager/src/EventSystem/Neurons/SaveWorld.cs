namespace BCM.Neurons
{
  public class SaveWorld : NeuronAbstract
  {
    public override void Fire(int b)
    {
      if (ConnectionManager.Instance.GetClients().Count <= 0) return;

      if (!Steam.Network.IsServer)
      {
        Log.Out(Config.ModPrefix + "World save failed. Not a network server");

        return;
      }

      GameManager.Instance.SaveLocalPlayerData();
      GameManager.Instance.SaveWorld();

      Log.Out(Config.ModPrefix + " World saved");
    }
  }
}
