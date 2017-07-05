namespace BCM.Commands
{
  public class BCSpawn : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count == 0)
      {
        SendOutput("Specify a command.");
        SendOutput(GetHelp());

        return;
      }

      if (_params[0] == "horde")
      {
        if (_params.Count == 1)
        {
          //count = 25;
          //target = comand sender
        }
      }
    }
  }
}
