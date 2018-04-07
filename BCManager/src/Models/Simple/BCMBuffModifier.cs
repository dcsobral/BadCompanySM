using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBuffModifier
  {
    [UsedImplicitly] public string Stat;
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public string Cat;
    [UsedImplicitly] public int Max;
    [UsedImplicitly] public int IDur;
    [UsedImplicitly] public int UID;
    [UsedImplicitly] public double FDur;
    [UsedImplicitly] public double ValStart;
    [UsedImplicitly] public double ValEnd;
    [UsedImplicitly] public double Freq;
    [UsedImplicitly] public double ApplyTime;
    [UsedImplicitly] public string Target;

    public BCMBuffModifier([NotNull] MultiBuffClass.Modifier modifier)
    {
      Stat = modifier.TargetStat.ToString();
      Type = modifier.TypeOfModifier.ToString();
      Cat = modifier.CategoryFlags.ToString();
      Max = modifier.ModifierStackMax;
      IDur = modifier.ModifierIDuration;
      UID = modifier.ModifierUID;
      FDur = Math.Round(modifier.ModifierFDuration, 6);
      ValStart = Math.Round(modifier.ModifierValueStart, 6);
      ValEnd = Math.Round(modifier.ModifierValueEnd, 6);
      Freq = Math.Round(modifier.ModifierFrequency, 6);
      ApplyTime = Math.Round(modifier.ModifierApplyTime, 6);
      Target = modifier.TargetBuff;
    }
  }
}
