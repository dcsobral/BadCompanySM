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
  public class DeadIsDead : NeuronAbstract
  {
    public DeadIsDead()
    {
    }
    public override bool Fire(int b)
    {
      // todo: implement
      // todo: IsDead checker, after death animation kick player and then rename ttp file and move to sub folder for archive
      return true;
    }
  }
}
