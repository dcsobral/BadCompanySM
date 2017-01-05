namespace BCM.Commands
{
  public class ListQuests : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      foreach (QuestClass qc in QuestClass.s_Quests.Values)
      {
        output += qc.Name + "(" + qc.ID + "):" + qc.SubTitle;
      }
      SendOutput(output);
    }
  }
}
