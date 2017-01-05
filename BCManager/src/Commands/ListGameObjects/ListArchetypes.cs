namespace BCM.Commands
{
  public class ListArchetypes : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      foreach (string name in Archetypes.Instance.GetArchetypeNames())
      {
        output += name + _sep;
        //Archetype a = Archetypes.Instance.GetArchetype(name);
        //output += a.Name + _sep;
      }
      SendOutput(output);
    }
  }
}
