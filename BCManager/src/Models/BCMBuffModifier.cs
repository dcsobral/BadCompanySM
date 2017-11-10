using System;

namespace BCM.Models
{
  public class BCMBuffModifier
  {
    public string Stat;
    public string Type;
    public string Cat;
    public int Max;
    public int IDur;
    public int UID;
    public double FDur;
    public double ValStart;
    public double ValEnd;
    public double Freq;
    public double ApplyTime;
    public string Target;

    public BCMBuffModifier(MultiBuffClass.Modifier modifier)
    {
      if (modifier == null) return;

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
