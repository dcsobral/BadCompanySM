using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBiomePrefabDecoration
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public double Prob;

    public BCMBiomePrefabDecoration(BiomePrefabDecoration prefab)
    {
      Name = prefab.m_sPrefabName;
      Prob = Math.Round(prefab.m_Prob, 6);
    }
  }
}
