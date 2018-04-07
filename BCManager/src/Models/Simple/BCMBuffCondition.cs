using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBuffCondition
  {
    [UsedImplicitly] public string Counter;
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public double Value;

    public BCMBuffCondition([NotNull] MultiBuffClassCondition condition)
    {
      Counter = condition.Counter;
      Type = condition.ConditionType.ToString();
      Value = condition.Value;
    }
  }
}
