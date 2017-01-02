using System.Collections.Generic;

namespace BCM
{
  public static class Brain
  {
    public static List<Synapse> synapses = new List<Synapse>();

    public static void FireNeurons(int b)
    {
      foreach (Synapse s in synapses)
      {
        s.FireNeurons(b);
      }
    }
    public static void BondSynapse(Synapse s)
    {
      synapses.Add(s);
    }
    public static void FrySynapse(Synapse s)
    {
      synapses.Remove(s);
    }
  }
}
