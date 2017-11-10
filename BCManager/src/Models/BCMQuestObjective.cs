namespace BCM.Models
{
  public class BCMQuestObjective
  {
    public string Type;
    public string Id;
    public string Value;
    //public byte Version;
    //public bool Complete;
    //public byte Phase;
    //public Quest OwnerQuest;

    public BCMQuestObjective(BaseObjective objective)
    {
      Type = objective.GetType().ToString();
      Id = objective.ID;
      Value = objective.Value;
    }
  }
}
