using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMQuestObjective
  {
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public string Value;

    public BCMQuestObjective([NotNull] BaseObjective objective)
    {
      Type = objective.GetType().ToString();
      Id = objective.ID;
      Value = objective.Value;
    }
  }
}
