using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Neurons
{
  public class SpawnManager : NeuronAbstract
  {
    public SpawnManager()
    {
    }
    public override bool Fire(int b)
    {
      int count = 0;
      try
      {
        count = GameManager.Instance.World.Players.Count;
      } catch
      {
        return false;
      }
      if (count > 0)
      {

        //******** demo *******//
        Log.Out(Config.ModPrefix + " Attempting to Spawn");
        List<EntityPlayer> lep = GameManager.Instance.World.Players.list;
        foreach (EntityPlayer ep in lep)
        {
          Log.Out(Config.ModPrefix + " Attempting to Spawn - for " + ep.EntityName);
          foreach (int entityClassID in EntityClass.list.Keys)
          {
            Log.Out(Config.ModPrefix + " Attempting to Spawn - in " + EntityClass.list[entityClassID].entityClassName);
            if (EntityClass.list[entityClassID].bAllowUserInstantiate)
            {
              Log.Out(Config.ModPrefix + " Attempting to Spawn - allowed " + EntityClass.list[entityClassID].entityClassName);

              BCMSpawner spawner = new BCMSpawner();
              spawner.entityClassID = entityClassID;
              spawner.pos = ep.position;
              API.SpawnQueue.Enqueue(spawner);
              Log.Out(API.SpawnQueue.Count.ToString());

              break;
            }
          }
        }
        //******** demo *******//

        return true;
      }
      return false;
    }
  }
}
