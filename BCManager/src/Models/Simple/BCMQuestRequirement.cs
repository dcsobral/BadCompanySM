using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMQuestRequirement
  {
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public string Value;

    public BCMQuestRequirement([NotNull] BaseRequirement requirement)
    {
      Type = requirement.GetType().ToString();
      Id = requirement.ID;
      Value = requirement.Value;
    }
  }
}
