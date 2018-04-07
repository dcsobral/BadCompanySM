using RWG2.Rules;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMStreetGenerationData
  {
    [UsedImplicitly] public int Level;
    [UsedImplicitly] public int Mult;
    [UsedImplicitly] public string Axiom;
    [UsedImplicitly] public Dictionary<string, string> Rules;
    [UsedImplicitly] public string[] Alts;

    public BCMStreetGenerationData([NotNull] StreetGenerationData streetGenData)
    {
      Level = streetGenData.Level;
      Mult = streetGenData.LengthMultiplier;
      Axiom = streetGenData.Axiom;
      Rules = streetGenData.Rules;
      Alts = streetGenData.AltCommands;
    }
  }
}
