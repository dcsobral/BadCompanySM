using System;

namespace BCM
{
  public class StateManager
  {
    public static void Awake()
    {
      try
      {
        PersistentData.PersistentContainer.Load();
      }
      catch (Exception e)
      {
        Log.Out("(" + Config.ModPrefix + ") Error in StateManager.Awake: " + e);
      }
    }

    public static void Shutdown()
    {
      try
      {
        PersistentData.PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out("(" + Config.ModPrefix + ") Error in StateManager.Shutdown: " + e);
      }
    }
  }
}
