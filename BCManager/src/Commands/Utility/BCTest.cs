using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCTest : BCCommandAbstract
  {
    protected override void Process()
    {
      SendOutput("Warning: This is not a warning!");
    }
  }
}
