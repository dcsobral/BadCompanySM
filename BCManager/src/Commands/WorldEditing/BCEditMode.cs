using System.Reflection;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCEditMode : BCCommandAbstract
  {
    private string _editModeField = "PH";

    protected override void Process()
    {
      if (!BCUtils.CheckWorld()) return;

      var editMode = GameManager.Instance.GetType().GetField(_editModeField, BindingFlags.NonPublic | BindingFlags.Instance);

      if (editMode == null)
      {
        SendOutput("Error getting editmode field");

        return;
      }

      if (!(editMode.GetValue(GameManager.Instance) is bool mode))
      {
        SendOutput("Error getting current value of editmode");

        return;
      }

      switch (Params.Count)
      {
        case 0:
          editMode.SetValue(GameManager.Instance, !mode);
          break;

        case 1:
          if (!bool.TryParse(Params[0], out var flag))
          {
            SendOutput("Unable to parse param as a boolean");

            return;
          }
          editMode.SetValue(GameManager.Instance, flag);
          break;

        default:
          SendOutput(GetHelp());
          return;
      }

      SendOutput($"Edit Mode set to {GameManager.Instance.IsEditMode()}");
    }
  }
}
