using System;
using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class Test : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        EntityPlayer _pl = null;
        _pl = (EntityPlayer)GameManager.Instance.World.Entities.dict[171];
        ItemValue _iv = new ItemValue(165);
        ItemStack _is = new ItemStack(_iv, 25);
        _pl.bag.AddItem(_is);
        SdtdConsole.Instance.Output("Attempted to add Item" + _is.ToString());
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }





      //Particle Effects
      //try
      //{
      //  Dictionary<int, Transform> pe = new Dictionary<int, Transform>();

      //  object[] array = Resources.LoadAll("ParticleEffects", typeof(Transform));
      //  object[] array2 = array;
      //  for (int i = 0; i < array2.Length; i++)
      //  {
      //    object obj = array2[i];
      //    string text = ((Transform)obj).gameObject.name;
      //    if (!text.StartsWith("p_"))
      //    {
      //      continue;
      //    }
      //    else
      //    {
      //      Log.Out(text);
      //    }
      //  }

      //}
      //catch (Exception e)
      //{
      //  Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      //}
    }
  }
}
