using RWG2.Rules;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMPrefabSpawnRule
  {
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public readonly List<BCMPrefabInfo> Prefabs = new List<BCMPrefabInfo>();

    public BCMPrefabSpawnRule([NotNull] PrefabSpawnRule spawnRule)
    {
      Name = spawnRule.Name;
      if (spawnRule.prefabs == null) return;

      foreach (var prefab in spawnRule.prefabs)
      {
        Prefabs.Add(new BCMPrefabInfo(prefab));
      }
    }
  }
}
