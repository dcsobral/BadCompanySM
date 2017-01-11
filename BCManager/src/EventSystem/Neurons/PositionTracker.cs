using BCM.Models;
using BCM.PersistentData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BCM.Neurons
{
  public class PositionTracker : NeuronAbstract
  {
    public PositionTracker()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement
      return true;
    }
  }
}
