namespace BCM.Models
{
  public class BCMBuffCondition
  {
    public string Counter;
    public string Type;
    public double Value;

    public BCMBuffCondition(MultiBuffClassCondition condition)
    {
      if (condition == null) return;

      Counter = condition.Counter;
      Type = condition.ConditionType.ToString();
      Value = condition.Value;
    }
  }
}
