using System;
using System.Reflection;

namespace BCM
{
  public static class StateManager
  {
    public static void Init()
    {
      try
      {
        PersistentData.PersistentContainer.Load();
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in StateManager.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }

    public static void WorldAlive()
    {
      Brain.MakeConscious();
    }

    public static void Awake()
    {
    }

    public static void Shutdown()
    {
      try
      {
        //Save All
        PersistentData.PersistentContainer.Instance.Save(null);
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in StateManager.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }
  }
}
