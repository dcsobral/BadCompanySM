using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMQuestReward
  {
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public string Value;

    public BCMQuestReward([NotNull] BaseReward reward)
    {
      Type = reward.GetType().ToString();
      Id = reward.ID;
      Value = reward.Value;
    }
  }
}
