using System;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCMemTrash : BCCommandAbstract
  {
    protected override void Process()
    {
      GC.Collect();
      GC.WaitForPendingFinalizers();
      SendOutput("Trash Disposed");
    }
  }
}
