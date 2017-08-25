using System;

namespace BCM.Commands
{
  public class BCMemTrash : BCCommandAbstract
  {
    public override void Process()
    {
      GC.Collect();
      GC.WaitForPendingFinalizers();
      SendOutput("Trash Disposed");
    }
  }
}
