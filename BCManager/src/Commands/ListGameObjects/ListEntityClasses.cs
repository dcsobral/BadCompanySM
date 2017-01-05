namespace BCM.Commands
{
  public class ListEntityClasses : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      foreach (EntityClass ec in EntityClass.list.Values)
      {
        output += ec.entityClassName + _sep;
      }
      SendOutput(output);
    }
  }
}
