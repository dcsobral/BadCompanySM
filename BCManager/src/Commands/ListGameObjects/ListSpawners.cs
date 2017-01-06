namespace BCM.Commands
{
  public class ListSpawners : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      //GameManager.Instance.World.GetDynamiceSpawnManager();
      DictionarySave<string, EntitySpawnerClassForDay> esc = EntitySpawnerClass.list;
      foreach (string name in esc.Keys)
      {
        EntitySpawnerClassForDay escfd = esc[name];
        output += name + ":(" + escfd.Count() + ") [clamp=" + escfd.bClampDays + ",dynamic=" + escfd.bDynamicSpawner + ",wrap=" + escfd.bWrapDays + "]" + _sep;
        for (int i = 1; i< escfd.Count(); i++)
        {
          EntitySpawnerClass escday = escfd.Day(i);
          // todo: show groups on days
        }

      }
      SendOutput(output);
    }
  }
}
