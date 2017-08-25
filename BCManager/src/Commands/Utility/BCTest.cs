namespace BCM.Commands
{
  public class BCTest : BCCommandAbstract
  {
    public override void Process()
    {
      SendOutput("test");
    }
  }
}
