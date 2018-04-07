using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMQuestAction
  {
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public string Value;

    public BCMQuestAction([NotNull] BaseQuestAction action)
    {
      Type = action.GetType().ToString();
      Id = action.ID;
      Value = action.Value;
    }
  }
}
