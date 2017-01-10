using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class Test : BCCommandAbstract
  {
    public override void Process()
    {

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
