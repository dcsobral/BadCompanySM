namespace BCM.Models
{
  public class BCMCommand
  {
    public string Name;
    public string[] Commands;
    public int DefaultPermission;
    public string Help;
    public string Description;
    public string DefaultOptions;

    public BCMCommand()
    {
      Name = string.Empty;
      Commands = new [] { string.Empty };
      DefaultPermission = 0;
      Help = string.Empty;
      Description = string.Empty;
      DefaultOptions = string.Empty;
    }
  }
}
