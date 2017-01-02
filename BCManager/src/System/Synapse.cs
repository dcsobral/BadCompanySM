using BCM.Neurons;
using System.Collections.Generic;

namespace BCM
{
  public class Synapse
  {
    public string name;
    public bool IsEnabled;
    public int beats;
    public int lastfired;
    public List<NeuronAbstract> neurons = new List<NeuronAbstract>();

    public Synapse()
    {
    }
    public void WireNeurons()
    {
      // todo: change to use the config files to define which neurons should be wired for a synapse
      switch (name)
      {
        case "questmonitor":
          neurons.Add(new QuestMonitor());
          break;
        default:
          Log.Out(Config.ModPrefix + " Unable to find unknown Synapse " + name);
          break;
      }
    }
    public bool FireNeurons(int b)
    {
      if ((b >= lastfired + beats) && IsEnabled)
      {
        lastfired = b;
        foreach (NeuronAbstract n in neurons)
        {
          n.Fire(b);
        }
      }
      return true;
    }
  }
}
