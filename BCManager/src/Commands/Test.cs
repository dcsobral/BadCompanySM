namespace BCM.Commands
{
  public class Test : BCCommandAbstract
  {
    public override void Process()
    {
      SendOutput("test");
    }
  }
}
