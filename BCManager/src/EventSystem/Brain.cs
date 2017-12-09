using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM
{
  public static class Brain
  {
    public static List<Synapse> Synapses = new List<Synapse>();

    public static void FireNeurons(int b)
    {
      foreach (var s in Synapses)
      {
        s.FireNeurons(b);
      }
    }

    public static void MakeConscious()
    {
      foreach (var synapse in Synapses)
      {
        if (!synapse.IsEnabled) continue;

        foreach (var n in synapse.GetNeurons())
        {
          n.Awake();
        }
      }
    }

    public static void BondSynapse(Synapse s)
    {
      Synapses.Add(s);
    }

    public static void FrySynapse(Synapse s)
    {
      Synapses.Remove(s);
    }

    [CanBeNull]
    public static Synapse GetSynapse(string name)
    {
      return Synapses.FirstOrDefault(s => s.Name == name);
    }

    [CanBeNull]
    public static List<NeuronAbstract> GetSynapseNeurons(string name)
    {
      return Synapses.FirstOrDefault(s => s.Name == name)?.GetNeurons();
    }
  }
}
