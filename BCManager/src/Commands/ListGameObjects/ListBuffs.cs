namespace BCM.Commands
{
  public class ListBuffs : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      foreach (MultiBuffClass mbc in MultiBuffClass.s_classes.Values)
      {
        output += mbc.Id + _sep;
      }
      SendOutput(output);
    }
  }
}
