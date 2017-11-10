using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  public class BCMExpressionData
  {
    public bool Blink;
    public bool Saccades;
    public double BlinkDur;
    public int BlinkMin;
    public int BlinkMax;
    public Dictionary<string, double> Values;
    public BCMExpressionData(UMAExpressionData expression)
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
