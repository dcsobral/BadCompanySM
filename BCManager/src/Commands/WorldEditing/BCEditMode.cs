using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCEditMode : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      switch (Params.Count)
      {
        case 0:
          world.bEditorMode = !world.bEditorMode;
          break;
        case 1:
          if (!bool.TryParse(Params[0], out var flag))
          {
            SendOutput("Unable to parse param as a boolean");

            return;
          }
          world.bEditorMode = flag;
          break;
      }

      SendOutput($"Edit Mode set to {world.bEditorMode}");
    }
  }
}
