using System.Collections.Generic;

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
    public static void BondSynapse(Synapse s)
    {
      Synapses.Add(s);
    }
    public static void FrySynapse(Synapse s)
    {
      Synapses.Remove(s);
    }
  }
}
