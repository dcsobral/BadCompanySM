using BCM.PersistentData;

namespace BCM.Commands
{
  public class BCSettings : BCCommandAbstract
  {
    public override void Process()
    {
      var settings = PersistentContainer.Instance.Settings;

      switch (Params.Count)
      {
        case 0:
          SendJson(settings.Data());

          break;

        case 1:
          //todo: report details on setting

          //same output as 0 params but with deeper root node. root=param1 

          break;

        case 2:
          var path = Params[0].Split('.');
          if (path.Length != 4)
          {
            SendOutput("Incorrect format for the key string (four strings seperated by a dot.)");

            //todo: allow 3 parts + an array of kvp from options. i.e. Player.8200889977663366.SpawnManager /opt1=val1 /opt2=val2
            return;
          }


          var value = settings.GetValue(path[0], path[1], path[2], path[3]);
          if (value != null)
          {
            SendOutput($"New Value:{Params[0]}={Params[1]}");
            SendOutput($"Prev Value:{value}");
          }
          settings.SetValue(path[0], path[1], path[2], path[3], Params[1]);
          PersistentContainer.Instance.Save("settings");

          break;

        default:
          SendOutput("Incorrect params");
          SendOutput(GetHelp());

          return;
      }

    }
  }
}
