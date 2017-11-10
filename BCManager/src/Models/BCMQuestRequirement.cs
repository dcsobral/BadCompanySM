namespace BCM.Models
{
  public class BCMQuestRequirement
  {
    public string Type;
    public string Id;
    public string Value;
    //public bool Complete;
    //public string Description;
    //public string StatusText;
    //public Quest OwnerQuest;

    public BCMQuestRequirement(BaseRequirement requirement)
    {
      Type = requirement.GetType().ToString();
      Id = requirement.ID;
      Value = requirement.Value;
    }
  }
}
