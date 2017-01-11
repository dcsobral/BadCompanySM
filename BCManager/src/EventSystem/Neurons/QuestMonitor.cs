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
  public class QuestMonitor : NeuronAbstract
  {
    private PersistentContainer _pcCache = null;
    public QuestMonitor()
    {
      //Log.Out(Config.ModPrefix + " Added Quest Monitor");
    }
    public override bool Fire(int b)
    {
      // todo: move this to the abstract object??
      if (_pcCache == null && PersistentContainer.IsLoaded)
      {
        try
        {
          _pcCache = PersistentContainer.Instance.Clone();
        }
        catch
        {
          Log.Out(Config.ModPrefix + " CACHE CLONE ERROR");
        }
      }

      if (ConnectionManager.Instance.ClientCount() > 0 && _pcCache != null)
      {
        using (List<ClientInfo>.Enumerator enumerator = ConnectionManager.Instance.GetClients().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string playerName = enumerator.Current.playerName;
            string playerId = enumerator.Current.playerId;

            PDQuests _currentQuests = new PDQuests();
            PDQuests _cacheQuests = new PDQuests();
            try
            {
              Player pcurr = PersistentContainer.Instance.Players[playerId, false];
              Player pcache = _pcCache.Players[playerId, true]; // allow player to be created if it doesn't exist in cache
              if (pcurr != null)
              {
                _currentQuests = pcurr.Quests;
              }
              if (pcache != null)
              {
                _cacheQuests = pcache.Quests;
              }
              //else
              //{
              //  // test: need to create player in cache if they have just joined for the first time after deleting the data file
              //  try
              //  {
              //    _pcCache.Players[playerId, true].Quests.quests = pcurr.Quests.quests.Clone();
              //    pcache = 
              //  } catch (Exception e)
              //  {
              //    Log.Out(Config.ModPrefix + " QuestMonitoring.Fire Exception couldn't update new player quests: " + e);
              //  }
              //}
            }
            catch (Exception e)
            { Log.Out(Config.ModPrefix + " QuestMonitoring.Fire Exception getting quest lists: " + e); }

            List<PDQuest> _changedQuests = new List<PDQuest>();
            try
            {
              foreach (PDQuest q in _currentQuests.quests)
              {
                if (_cacheQuests != null)
                {
                  PDQuest qq = _cacheQuests.quests.Find(x => x.ID == q.ID && x.CurrentState == q.CurrentState);
                  if (qq == null)
                  {
                    //not found in list, so must be new or status changed
                    _changedQuests.Add(q);
                  }
                  else
                  {
                    //found a match so remove 1 instance of quest
                    _cacheQuests.quests.RemoveAt(_cacheQuests.quests.LastIndexOf(qq));
                  }
                }
                else
                {
                  _changedQuests.Add(q);
                }
              }
              // update cached quests to current quests
              try
              {
                _cacheQuests.quests = _currentQuests.quests.Clone();
              }
              catch (Exception e)
              {
                Log.Out(Config.ModPrefix + " QuestMonitoring.Fire Exception assigning current quests to cache: " + e);
              }
            }
            catch (Exception e)
            {
              Log.Out(Config.ModPrefix + " QuestMonitoring.Fire Exception computing changed quests: " + e);
              _changedQuests = new List<PDQuest>(); //if an error computing changes then don't return a list or it spams log files
            }

            try
            {
              foreach (PDQuest q in _changedQuests)
              {
                Log.Out(Config.ModPrefix + " " + playerName + " Quest Status Changed to " + q.CurrentState + ":" + q.ID + "");
              }
            }
            catch (Exception e)
            { Log.Out(Config.ModPrefix + " QuestMonitoring.Fire Exception: (changedquests.foreach) " + e); }
          }
        }
      }

      return true;
    }
  }
}
