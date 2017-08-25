using System;
using System.Reflection;

namespace BCM
{
  public static class StateManager
  {
    public static void Awake()
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

    public static void Shutdown()
    {
      try
      {
        PersistentData.PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in StateManager.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }
  }
}
