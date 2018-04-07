using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMExpressionData
  {
    [UsedImplicitly] public bool Blink;
    [UsedImplicitly] public bool Saccades;
    [UsedImplicitly] public double BlinkDur;
    [UsedImplicitly] public int BlinkMin;
    [UsedImplicitly] public int BlinkMax;
    [UsedImplicitly] public Dictionary<string, double> Values;

    public BCMExpressionData([NotNull] UMAExpressionData expression)
    {
      Blink = expression.BlinkingEnabled;
      Saccades = expression.SaccadesEnabled;
      BlinkDur = Math.Round(expression.BlinkDuration, 3);
      BlinkMin = expression.BlinkMinDelay;
      BlinkMax = expression.BlinkMaxDelay;
      Values = expression.ExpressionValues.ToDictionary(v => v.Key, v => Math.Round(v.Value, 3));
    }
  }
}
