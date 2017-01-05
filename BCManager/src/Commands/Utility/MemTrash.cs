using System;

namespace BCM.Commands
{
  public class MemTrash : BCCommandAbstract
  {
    public override void Process()
    {
      GC.Collect();
      GC.WaitForPendingFinalizers();
      SdtdConsole.Instance.Output("Trash Disposed");
    }
  }
}
