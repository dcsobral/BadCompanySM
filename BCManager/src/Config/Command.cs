namespace BCM.ConfigModels
{
  public class Command
  {
    public string name;
    public string[] commands;
    public int dpl;
    public string help;
    public string description;
    public string defaultoptions;

    public Command()
    {
      name = "";
      commands = new string[] { string.Empty };
      dpl = 0;
      help = "";
      description = "";
      defaultoptions = "";
    }
    public Command(string _name, string[] _commands, int _dpl, string _help, string _description, string _defaultoptions)
    {
      name = _name;
      commands = _commands;
      dpl = _dpl;
      help = _help;
      description = _description;
      defaultoptions = _defaultoptions;
    }
  }
}
