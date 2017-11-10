namespace BCM.Models
{
  public class BCMQuestAction
  {
    public string Type;
    public string Id;
    public string Value;
    //public string OwnerQuest;

    public BCMQuestAction(BaseQuestAction action)
    {
      Type = action.GetType().ToString();
      Id = action.ID;
      Value = action.Value;
    }
  }
}
