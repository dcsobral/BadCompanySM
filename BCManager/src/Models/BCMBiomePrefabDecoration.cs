using System;

namespace BCM.Models
{
  public class BCMBiomePrefabDecoration
  {
    public string Name;
    public double Prob;

    public BCMBiomePrefabDecoration(BiomePrefabDecoration prefab)
    {
      Name = prefab.m_sPrefabName;
      Prob = Math.Round(prefab.m_Prob, 6);
    }
  }
}
