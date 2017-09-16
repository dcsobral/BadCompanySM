using System.Collections.Generic;

namespace BCM.Neurons
{
  public class SaveWorld : NeuronAbstract
  {
    public override void Fire(int b)
    {
      if (ConnectionManager.Instance.GetClients().Count == 0) return;

      if (!Steam.Network.IsServer)
      {
        Log.Out($"{Config.ModPrefix} World save failed. Not a network server");

        return;
      }

      var cmd = new ConsoleCmdSaveWorld();
      cmd.Execute(new List<string>(), new CommandSenderInfo());

      Log.Out($"{Config.ModPrefix} World saved");
    }
  }
}
