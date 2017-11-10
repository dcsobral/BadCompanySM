namespace BCM.Models
{
  public class BCMQuestReward
  {
    public string Type;
    public string Id;
    public string Value;
    //public string OwnerQuest;

    public BCMQuestReward(BaseReward reward)
    {
      Type = reward.GetType().ToString();
      Id = reward.ID;
      Value = reward.Value;
    }
  }
}
